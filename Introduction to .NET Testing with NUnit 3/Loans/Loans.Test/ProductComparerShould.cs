using System;
using System.Collections.Generic;
using Loans.Domain.Applications;
using NUnit.Framework;

namespace Loans.Test
{
    public class ProductComparerShould
    {
    
        private List<LoanProduct> products;
        private ProductComparer sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            products = new List<LoanProduct>
            {
                new LoanProduct(1, "a", 1),
                new LoanProduct(2,"b",2),
                new LoanProduct(3,"c",3),
            };
            
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //product.Disponse()
        }

        [SetUp]
        public void Setup()
        {
            sut = new ProductComparer(new LoanAmount("USD",200_000m), products);
        }

        [TearDown]
        public void TearDown()
        {
            //Run after each test executes
            //sut.Disponse()
        }
        
        
        [Test]
        //[Category("Product Comparison")]
        [ProductComparison]
        public void ReturnCorrectNumberOfComparisons()
        {
            List<MonthlyRepaymentComparison> comparisons =
                sut.CompareMonthlyRepayments(new LoanTerm(30));
            
            Assert.That(products,Has.Exactly(3).Items);
        }
        
        [Test]
        [Ignore("test")] 
        public void NotReturnDupplicateComparisons()
        {
            List<MonthlyRepaymentComparison> comparisons =
                sut.CompareMonthlyRepayments(new LoanTerm(30));
            
            Assert.That(products,Is.Unique);

        }

        [Test]
        public void ReturnComparisonForFirstProduct()
        {
            
            List<MonthlyRepaymentComparison> comparisons =
                sut.CompareMonthlyRepayments(new LoanTerm(30));
            
            var expectedProduct = new MonthlyRepaymentComparison("a",1,643.28m);

            Assert.That(comparisons,Does.Contain(expectedProduct));
            
        }
        [Test] 
        public void ReturnComparisonForFirstProduct_WithPartialKnownExpectedValues()
        {
            List<MonthlyRepaymentComparison> comparisons =
                sut.CompareMonthlyRepayments(new LoanTerm(30));

            Assert.That(comparisons,Has.Exactly(1)
                    .Property("ProductName").EqualTo("a")
                    .And
                    .Property("InterestRate").EqualTo(1)
                    .And 
                    .Property("MonthlyRepayment").GreaterThan(0));
            
            // Assert.That(comparisons,Has.Exactly(1)
            //     .Matches<MonthlyRepaymentComparison>(
            //   
            //         item => item.ProductName == "a" &&
            //                                                    item.InterestRate == 1 &&
            //                                                    item.MonthlyRepayment > 0));
            
            Assert.That(comparisons,
                Has.Exactly(1)
                            .Matches(new MonthlyRepaymentGreaterThanZeroConstraint("a",1)));
        }

        [Test]
        public void NotAllowZeroYears()
        {
            
            Assert.That(() => new LoanTerm(0),Throws.TypeOf<ArgumentOutOfRangeException>());
            
            Assert.That(() => new LoanTerm(0), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With
                .Property("Message")
                .EqualTo("Please specify a value greater than 0.\nParameter name: years"));
            
            Assert.That(()=> new LoanTerm(0),Throws.TypeOf<ArgumentOutOfRangeException>()
                .With
                .Message
                .EqualTo("Please specify a value greater than 0.\nParameter name: years"));
            
            Assert.That(()=> new LoanTerm(0),Throws.TypeOf<ArgumentOutOfRangeException>()
                .With
                .Property("ParamName")
                .EqualTo("years"));
            
            Assert.That(()=>new LoanTerm(0),Throws.TypeOf<ArgumentOutOfRangeException>()
                .With
                .Matches<ArgumentOutOfRangeException>(
                ex=>ex.ParamName=="years"));
        }
    }
}
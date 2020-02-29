using Loans.Domain.Applications;
using NUnit.Framework;

namespace Loans.Test
{    
    [TestFixture]
    public class LoanTermShould
    {
        [Test]
        public void ReturnTermInMounths()
        {
            var sut = new LoanTerm(1);
            
            Assert.That(sut.ToMonths(),Is.EqualTo(12),"Months should be 12 * number of years");
        }

        [Test]
        public void StoreYears()
        {
             var sult = new LoanTerm(1);
             
             Assert.That(sult.Years,Is.EqualTo(1));
        }

        [Test]
        public void RespectValueEquality()
        {
            var a = new LoanTerm(1);
            var b = new LoanTerm(1);
            
            Assert.That(a,Is.EqualTo(b));
            
        }

        [Test]
        public void RespectValueInequality()
        {
            var a = new LoanTerm(1);
            var b = new LoanTerm(2);
            
            Assert.That(a,Is.Not.EqualTo(b));
        }

        [Test]
        public void RefarenceEqualityExample()
        {
            
            var a = new LoanTerm(1);
            var b = a;
            var c = new LoanTerm(1);
            
            Assert.That(a,Is.SameAs(b));
            Assert.That(a,Is.Not.SameAs(c));
            
        }

        [Test]
        public void Double()
        {
        
            double a = 1.0 / 3.0;
            
            Assert.That(a,Is.EqualTo(0.33).Within(0.004));
            Assert.That(a,Is.EqualTo(0.33).Within(10).Percent);
        }
        


    }
}
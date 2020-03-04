using System;
using Loans.Domain.Applications;
using NUnit.Framework;
using  Moq;
namespace Loans.Tests
{
    public class LoanApplicationProcessorShould
    {
        [Test]
        public void DeclineLowSalary()
        {
            LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
            LoanAmount amount = new LoanAmount("USD", 200_000);
            var application = new LoanApplication(42,
                                                  product,
                                                  amount,
                                                  "Sarah",
                                                  25,
                                                  "133 Pluralsight Drive, Draper, Utah",
                                                  64_999);
            
            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            var mockCreditScorer = new Mock<ICreditScorer>();

            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object,mockCreditScorer.Object);

            sut.Process(application);

            Assert.That(application.GetIsAccepted(), Is.False);
        }

        delegate void ValidateCallBack(string applicantName,
                                       int applicantAge,
                                       string applicantAddress,
                                       ref IdentityVerificationStatus status);
        
        [Test]
        public void Accept()
        {
            LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
            LoanAmount amount = new LoanAmount("USD", 200_000);
            var application = new LoanApplication(42,
                product,
                amount,
                "Sarah",
                25,
                "133 Pluralsight Drive, Draper, Utah",
                65_000);
            
            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            
            //Değişken tipine baktığı için eşlemenin yapıldığı fonskiyona dikkat edilmeldir.
            // var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            // mockIdentityVerifier.Setup(x => x.Validate(It.IsAny<string>(),
            //                                        It.IsAny<int>(),
            //                                        It.IsAny<string>()))
            //                                    .Returns(true);

            // bool isValidOutValue = true;
            // mockIdentityVerifier.Setup(x => x.Validate("Sarah",
            //         25,
            //         "133 Pluralsight Drive, Draper, Utah",
            //     out isValidOutValue));

            // mockIdentityVerifier
            //     .Setup(x => x.Validate("Sarah",
            //         25,
            //         "133 Pluralsight Drive, Draper, Utah",
            //         ref It.Ref<IdentityVerificationStatus>.IsAny))
            //     .Callback(new ValidateCallBack(
            //     (string applicantName,
            //             int applicantAge,
            //             string applicantAddress,
            //             ref IdentityVerificationStatus status) =>
            //         status = new IdentityVerificationStatus(true)));
            
            mockIdentityVerifier.Setup(x => x.Validate("Sarah",
                    25,
                    "133 Pluralsight Drive, Draper, Utah"))
                .Returns(true);
                
            
            var mockCreditScorer = new Mock<ICreditScorer>();
            
            // Birden fazla izlememiz gereken property varsa bu yöntemi kullanabiliriz.
            //Setup'ın altında kullanırsak ayarladığımız return 300 işlemini bozar ve test başarısız olur
            // mockCreditScorer.SetupAllProperties();
            
            mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);
            mockCreditScorer.SetupProperty(x => x.Cout,10);

            // var mockScoreValue = new Mock<ScoreValue>();
            // mockScoreValue.Setup(x => x.Score).Returns(300);
            //
            // var mockScoreResult = new Mock<ScoreResult>();
            // mockScoreResult.Setup(x => x.ScoreValue).Returns(mockScoreValue.Object);
            //
            // mockCreditScorer.Setup(x => x.ScoreResult).Returns(mockScoreResult.Object);
            
            

            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object,mockCreditScorer.Object);

            sut.Process(application);
            
            mockCreditScorer.VerifyGet(x=>x.ScoreResult.ScoreValue.Score,Times.Exactly(1));
            mockCreditScorer.VerifySet(x=>x.Cout = It.IsAny<int>(),Times.Once);
            
            Assert.That(application.GetIsAccepted(), Is.True);
            Assert.That(mockCreditScorer.Object.Cout,Is.EqualTo(11));
        }

        [Test]
        public void InitializeIdentityVerifier()
        {
                    LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
            LoanAmount amount = new LoanAmount("USD", 200_000);
            var application = new LoanApplication(42,
                product,
                amount,
                "Sarah",
                25,
                "133 Pluralsight Drive, Draper, Utah",
                65_000);
            
            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            
            mockIdentityVerifier.Setup(x => x.Validate("Sarah",
                    25,
                    "133 Pluralsight Drive, Draper, Utah"))
                .Returns(true);
                
            
            var mockCreditScorer = new Mock<ICreditScorer>();

            mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);

            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object,mockCreditScorer.Object);

            sut.Process(application);
            
            mockIdentityVerifier.Verify(x=>x.Initialize());
            
            mockIdentityVerifier.Verify(x=>x.Validate(It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>()));
            
            mockIdentityVerifier.VerifyNoOtherCalls();
        }
        [Test]
        public void CalculateScore()
        {
            LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
            LoanAmount amount = new LoanAmount("USD", 200_000);
            var application = new LoanApplication(42,
                product,
                amount,
                "Sarah",
                25,
                "133 Pluralsight Drive, Draper, Utah",
                65_000);
            
            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            
            mockIdentityVerifier.Setup(x => x.Validate("Sarah",
                    25,
                    "133 Pluralsight Drive, Draper, Utah"))
                .Returns(true);
                
            
            var mockCreditScorer = new Mock<ICreditScorer>();

            mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);

            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object,mockCreditScorer.Object);

            sut.Process(application);
            
            mockCreditScorer.Verify(x=>x.CalculateScore("Sarah",
                "133 Pluralsight Drive, Draper, Utah"),Times.Once);
        }

        [Test]
        public void NullReturnExample()
        {
            var mock = new Mock<INullExample>();

            //Moq varsayılan referans türlerini döndüren sahte yöntemleri otomatik olarak null döndürür.
            mock.Setup(x => x.SomeMethod());
                //.Returns<string>(null);
                

            string mockReturnValue = mock.Object.SomeMethod();
            
            Assert.That(mockReturnValue,Is.Null);
        }
        
    }

    public interface INullExample
    {
        string SomeMethod();
    }
    
}

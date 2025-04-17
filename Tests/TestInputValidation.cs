using NUnit.Framework;
using SafeVault.Models;

namespace SafeVault.Tests
{
    [TestFixture]
    public class TestInputValidation
    {
        [Test]
        public void TestForSQLInjection()
        {
            string maliciousInput = "'; DROP TABLE Users; --";
            Assert.That(InputValidation.IsValid(maliciousInput), Is.False);
        }

        [Test]
        public void TestForXSS()
        {
            string maliciousInput = "<script>alert('XSS');</script>";
            Assert.That(InputValidation.IsValid(maliciousInput), Is.False);
        }

        [Test]
        public void TestValidInput()
        {
            string validInput = "ValidUsername123";
            Assert.That(InputValidation.IsValid(validInput), Is.True);
        }
    }
}

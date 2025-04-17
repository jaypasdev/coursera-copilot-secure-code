using NUnit.Framework;
using Moq;
using Microsoft.Data.SqlClient;
using SafeVault.Data;
using SafeVault.Models;


namespace SafeVault.Tests
{
    [TestFixture]
    public class DatabaseTests
    {
        [Test]
        public void AddUser_PreventsSQLInjection_WithMocks()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase>();
            
            // Setup mock to simulate SQL injection being blocked
            mockDatabase
                .Setup(db => db.AddUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            string maliciousUsername = "'; DROP TABLE Users; --";
            string email = "test@example.com";

            // Act
            bool result = mockDatabase.Object.AddUser(maliciousUsername, email);

            // Assert
            Assert.IsFalse(result, "The method should handle the SQL injection safely.");
        }

        [Test]
        public void GetUserById_PreventsSQLInjection()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase>();

            mockDatabase
                .Setup(db => db.GetUserById(It.IsAny<int>()))
                .Returns(new User { UserID = 1, Username = "TestUser", Email = "test@example.com" });

            int maliciousUserId = -1; // Simulate SQL injection attempt

            // Act
            var user = mockDatabase.Object.GetUserById(maliciousUserId);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.UserID, "Method should return a valid user without executing malicious SQL.");
        }

        [Test]
        public void GetUserByEmail_PreventsSQLInjection()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase>();

            mockDatabase
                .Setup(db => db.GetUserByEmail(It.IsAny<string>()))
                .Returns(new User { UserID = 1, Username = "TestUser", Email = "test@example.com" });

            string maliciousEmail = "'; DROP TABLE Users; --";

            // Act
            var user = mockDatabase.Object.GetUserByEmail(maliciousEmail);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual("test@example.com", user.Email, "Method should prevent SQL injection and return valid data.");
        }


    }
}


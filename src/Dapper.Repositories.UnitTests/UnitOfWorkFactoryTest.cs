namespace Dapper.Repositories.UnitTests
{
    using System;
    using System.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class UnitOfWorkFactoryTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateShouldThrowArgumentNullException()
        {
            // arrange
            using (var factory = new UnitOfWorkFactory())
            {
                // act
                factory.Create(null);

                // assert
                // should throw exception
            }
        }

        [TestMethod]
        public void CreateShouldReturnInstance()
        {
            // arrange
            using (var factory = new UnitOfWorkFactory())
            {
                var connection = new Mock<IDbConnection>();
                var transaction = new Mock<IDbTransaction>();

                connection.Setup(cn => cn.BeginTransaction()).Returns(transaction.Object).Verifiable();

                // act
                var unitOfWork = factory.Create(connection.Object);

                // assert
                connection.Verify();
                transaction.Verify();
                Assert.IsNotNull(unitOfWork, "TEST FAILED: did not create a UnitOfWork");
            }
        }
    }
}

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
        public void Create_ShouldThrowArgumentNullException()
        {
            // arrange
            var repositoryFactoryMock = new Mock<IRepositoryFactory>();

            using (var factory = new UnitOfWorkFactory(repositoryFactoryMock.Object))
            {
                // act
                factory.Create(null);

                // assert
                // should throw exception
            }
        }

        [TestMethod]
        public void Create_ShouldReturnInstance()
        {
            // arrange
            var repositoryFactoryMock = new Mock<IRepositoryFactory>();
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();

            connectionMock.Setup(c => c.BeginTransaction()).Returns(transactionMock.Object);

            using (var factory = new UnitOfWorkFactory(repositoryFactoryMock.Object))
            {
                // act
                var unitOfWork = factory.Create(connectionMock.Object);

                // assert
                Assert.IsNotNull(unitOfWork);
            }
        }
    }
}

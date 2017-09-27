namespace Dapper.Repositories.UnitTests
{
    using System;
    using System.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class RepositoryFactoryTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateShouldThrowArgumentNullException()
        {
            // arrange
            var factory = new RepositoryFactory();

            // act
            factory.Create<object>(null);

            // assert
            // should throw exception
        }

        [TestMethod]
        public void CreateShouldReturnInstance()
        {
            // arrange
            var factory = new RepositoryFactory();
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            
            // act
            var repository = factory.Create<object>(connection.Object, transaction.Object);

            // assert
            transaction.Verify();
            connection.Verify();
            Assert.IsNotNull(repository);
        }
    }
}

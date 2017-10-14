namespace Dapper.Repositories.UnitTests
{
    using System;
    using System.Data;
    using Dapper.Repositories.UnitTests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class RepositoryFactoryTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_ShouldThrowArgumentNullException()
        {
            // arrange
            using (var factory = new RepositoryFactory())
            {
                // act
                factory.Create<TestEntity>(null);

                // assert
                // should throw exception
            }
        }

        [TestMethod]
        public void Create_ShouldReturnInstance()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();

            using (var factory = new RepositoryFactory())
            {

                // act
                var repository = factory.Create<TestEntity>(connectionMock.Object, transactionMock.Object);

                // assert
                Assert.IsNotNull(repository);
            }
        }
    }
}

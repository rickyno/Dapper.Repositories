namespace Dapper.Repositories.UnitTests
{
    using System;
    using System.Data;
    using Dapper.Repositories.UnitTests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class UnitOfWorkTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowException()
        {
            // arrange
            // nothing to arrange
            
            // act
            // ReSharper disable once UnusedVariable
            using (var unitOfWork = new UnitOfWork(null, null))
            {
            }

            // assert
            // should throw exception
        }

        [TestMethod]
        public void Repository_ShouldReturnInstance()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var repositoryFactoryMock = new Mock<IRepositoryFactory>();
            var repositoryMock = new Mock<IRepository<TestEntity>>();

            connectionMock.Setup(c => c.BeginTransaction()).Returns(transactionMock.Object).Verifiable();
            repositoryFactoryMock.Setup(c => c.Create<TestEntity>(connectionMock.Object, transactionMock.Object)).Returns(repositoryMock.Object).Verifiable();

            // act
            using (var unitOfWork = new UnitOfWork(repositoryFactoryMock.Object, connectionMock.Object))
            {
                var repository = unitOfWork.Repository<TestEntity>();

                // assert
                connectionMock.Verify();
                repositoryFactoryMock.Verify();

                Assert.IsNotNull(repository);
                Assert.AreSame(repository, repositoryMock.Object);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Commit_ShouldThrowException()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var repositoryFactoryMock = new Mock<IRepositoryFactory>();

            connectionMock.Setup(c => c.BeginTransaction()).Returns(transactionMock.Object).Verifiable();
            transactionMock.Setup(t => t.Commit()).Throws<InvalidOperationException>().Verifiable();

            using (var unitOfWork = new UnitOfWork(repositoryFactoryMock.Object, connectionMock.Object))
            {
                // act
                unitOfWork.Commit();

                // assert
                connectionMock.Verify();
                transactionMock.Verify();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Rollback_ShouldThrowException()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var repositoryFactoryMock = new Mock<IRepositoryFactory>();

            connectionMock.Setup(c => c.BeginTransaction()).Returns(transactionMock.Object).Verifiable();
            transactionMock.Setup(t => t.Rollback()).Throws<InvalidOperationException>().Verifiable();

            using (var unitOfWork = new UnitOfWork(repositoryFactoryMock.Object, connectionMock.Object))
            {
                // act
                unitOfWork.Rollback();

                // assert
                connectionMock.Verify();
                transactionMock.Verify();
            }
        }

        [TestMethod]
        public void Commit_ShouldSucceed()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var repositoryFactoryMock = new Mock<IRepositoryFactory>();

            connectionMock.Setup(c => c.BeginTransaction()).Returns(transactionMock.Object).Verifiable();
            transactionMock.Setup(t => t.Commit()).Verifiable();

            using (var unitOfWork = new UnitOfWork(repositoryFactoryMock.Object, connectionMock.Object))
            {
                // act
                unitOfWork.Commit();

                // assert
                connectionMock.Verify();
                transactionMock.Verify();
            }
        }

        [TestMethod]
        public void Rollback_ShouldSucceed()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var repositoryFactoryMock = new Mock<IRepositoryFactory>();
            
            connectionMock.Setup(c => c.BeginTransaction()).Returns(transactionMock.Object).Verifiable();
            transactionMock.Setup(t => t.Rollback()).Verifiable();

            using (var unitOfWork = new UnitOfWork(repositoryFactoryMock.Object, connectionMock.Object))
            {
                // act
                unitOfWork.Rollback();

                // assert
                connectionMock.Verify();
                transactionMock.Verify();
            }
        }
    }
}

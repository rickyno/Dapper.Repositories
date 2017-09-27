namespace Dapper.Repositories.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class UnitOfWorkTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorShouldThrowException()
        {
            // arrange
            // nothing to arrange
            
            // act
            var unitOfWork = new UnitOfWork(null);

            // assert
            // should throw exception
            Assert.IsNull(unitOfWork);
        }

        [TestMethod]
        public void RepositoryShouldReturnInstance()
        {
            // arrange
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();

            connection.Setup(c => c.BeginTransaction()).Returns(transaction.Object).Verifiable();

            var unitOfWork = new UnitOfWork(connection.Object);

            // act
            var repository = unitOfWork.Repository<object>();

            // assert
            connection.Verify();
            Assert.IsNotNull(repository);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CommitShouldThrowException()
        {
            // arrange
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            
            transaction.Setup(t => t.Commit()).Throws<InvalidOperationException>().Verifiable();
            connection.Setup(c => c.BeginTransaction()).Returns(transaction.Object).Verifiable();

            var unitOfWork = new UnitOfWork(connection.Object);

            // act
            unitOfWork.Commit();

            // assert
            connection.Verify();
            transaction.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RollbackShouldThrowException()
        {
            // arrange
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            
            //transaction.SetupGet(t => t.Connection).Returns(connection.Object).Verifiable();
            transaction.Setup(t => t.Rollback()).Throws<InvalidOperationException>().Verifiable();
            connection.Setup(c => c.BeginTransaction()).Returns(transaction.Object).Verifiable();

            var unitOfWork = new UnitOfWork(connection.Object);

            // act
            unitOfWork.Rollback();

            // assert
            connection.Verify();
            transaction.Verify();
        }

        [TestMethod]
        public void CommitShouldSucceed()
        {
            // arrange
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();

            //transaction.SetupGet(t => t.Connection).Returns(connection.Object).Verifiable();
            transaction.Setup(t => t.Commit()).Verifiable();
            connection.Setup(c => c.BeginTransaction()).Returns(transaction.Object).Verifiable();

            var unitOfWork = new UnitOfWork(connection.Object);

            // act
            unitOfWork.Commit();

            // assert
            connection.Verify();
            transaction.Verify();
        }

        [TestMethod]
        public void RollbackShouldSucceed()
        {
            // arrange
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();

            //transaction.SetupGet(t => t.Connection).Returns(connection.Object).Verifiable();
            transaction.Setup(t => t.Rollback()).Verifiable();
            connection.Setup(c => c.BeginTransaction()).Returns(transaction.Object).Verifiable();

            var unitOfWork = new UnitOfWork(connection.Object);

            // act
            unitOfWork.Rollback();

            // assert
            connection.Verify();
            transaction.Verify();
        }
    }
}

namespace Dapper.Repositories.UnitTests
{
    using System;
    using Dapper.Repositories.UnitTests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConnectionFactoryTest
    {
        private const string DatabaseName = "TestDb";

        [TestInitialize]
        public void Initialize()
        {
            TestDatabase.Create(DatabaseName);
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestDatabase.Delete(DatabaseName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Register_ShouldThrowExceptionWhenAlreadyRegistered()
        {
            // arrange
            var factory = new ConnectionFactory();

            // act
            factory.Register("DefaultConnection", TestDatabase.GetConnectionString(DatabaseName), true);
            factory.Register("DefaultConnection", TestDatabase.GetConnectionString(DatabaseName));

            // assert
            // should throw exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_ShouldThrowExceptionWhenNullName()
        {
            // arrange
            var factory = new ConnectionFactory();

            // act
            factory.Register(null, TestDatabase.GetConnectionString(DatabaseName));

            // assert
            // should throw exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_ShouldThrowExceptionWhenNullConnectionString()
        {
            // arrange
            var factory = new ConnectionFactory();

            // act
            factory.Register("DefaultConnection", null);

            // assert
            // should throw exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_ShouldThrowExceptionWhenNameNotFound()
        {
            var factory = new ConnectionFactory();

            factory.Register("DefaultConnection", TestDatabase.GetConnectionString(DatabaseName));

            // create a connection using 'UndefinedConnection' string
            // connection not registered will throw an ArgumentException
            factory.Create("UndefinedConnection");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_ShouldThrowExceptionWhenDefaultNotRegistered()
        {
            var factory = new ConnectionFactory();

            factory.Register("NotDefaultConnection", TestDatabase.GetConnectionString(DatabaseName));

            // create a connection using the default connection string
            // the default is not defined above which will throw an ArgumentNullException
            factory.Create();
        }

        [TestMethod]
        public void Create_ShouldReturnDefaultConnection()
        {
            var factory = new ConnectionFactory();

            factory.Register("DefaultConnection", TestDatabase.GetConnectionString(DatabaseName), true);

            var connection = factory.Create();

            Assert.IsNotNull(connection);
        }

        [TestMethod]
        public void Create_ShouldReturnNamedConnection()
        {
            var factory = new ConnectionFactory();
            
            factory.Register("NotDefaultConnection", TestDatabase.GetConnectionString(DatabaseName));

            var connection = factory.Create("NotDefaultConnection");
            
            Assert.IsNotNull(connection);
        }
    }
}

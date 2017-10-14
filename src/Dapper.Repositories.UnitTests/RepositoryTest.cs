namespace Dapper.Repositories.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Dapper.Contrib.Extensions.Fakes;
    using Dapper.Repositories.UnitTests.Mocks;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    
    [TestClass]
    public class RepositoryTest
    {
        private const int DefaultTimeout = 30;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowExceptionNullArgument()
        {
            // arrange
            // nothing to arrange
            
            // act
            // ReSharper disable once UnusedVariable
            using (var repository = new Repository<TestEntity>(null))
            {
            }

            // assert
            // should throw exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowExceptionNullArguments()
        {
            // arrange
            // nothing to arrange

            // act
            // ReSharper disable once UnusedVariable
            using (var repository = new Repository<TestEntity>(null, null))
            {
            }

            // assert
            // should throw exception
        }

        [TestMethod]
        public void Get_ShouldReturnEntityInstance()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var entity = new TestEntity { Id = 1 };
            
            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.GetOf1IDbConnectionObjectIDbTransactionNullableOfInt32((connection, id, transaction, timeout) => entity);

                using (var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object))
                {
                    // act
                    var result = repository.Get(1, DefaultTimeout);

                    // assert
                    Assert.IsNotNull(result);
                    Assert.AreEqual(entity, result);
                }
            }
        }

        [TestMethod]
        public void Get_ShouldReturnNull()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            
            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.GetOf1IDbConnectionObjectIDbTransactionNullableOfInt32((connection, id, transaction, timeout) => default(TestEntity));

                using (var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object))
                {
                    // act
                    var result = repository.Get(1, DefaultTimeout);

                    // assert
                    Assert.IsNull(result);
                }
            }
        }

        [TestMethod]
        public void Query_ShouldReturnEntityCollection()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var comparer = new TestEntityComparer();
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 },
                new TestEntity { Id = 3 },
                new TestEntity { Id = 4 },
                new TestEntity { Id = 5 },
                new TestEntity { Id = 6 },
                new TestEntity { Id = 7 },
                new TestEntity { Id = 8 },
                new TestEntity { Id = 9 }
            };

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.GetAllOf1IDbConnectionIDbTransactionNullableOfInt32((connection, transaction, timeout) => entities);

                using (var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object))
                {
                    // act
                    var results = repository.Query(timeout: DefaultTimeout).OrderBy(x => x.Id).ToList();

                    // assert
                    CollectionAssert.AreEqual(entities, results, comparer);
                }
            }
        }

        [TestMethod]
        public void Query_ShouldReturnFilteredEntityCollection()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var comparer = new TestEntityComparer();

            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 },
                new TestEntity { Id = 3 },
                new TestEntity { Id = 4 },
                new TestEntity { Id = 5 },
                new TestEntity { Id = 6 },
                new TestEntity { Id = 7 },
                new TestEntity { Id = 8 },
                new TestEntity { Id = 9 }
            };

            var expected = new List<TestEntity> { new TestEntity { Id = 2 }, new TestEntity { Id = 4 }, new TestEntity { Id = 6 }, new TestEntity { Id = 8 } };

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.GetAllOf1IDbConnectionIDbTransactionNullableOfInt32((connection, transaction, timeout) => entities);

                using (var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object))
                {
                    // act
                    var results = repository.Query(x => x.Id % 2 == 0, timeout: DefaultTimeout).OrderBy(x => x.Id).ToList();

                    // assert
                    Assert.IsNotNull(results);
                    CollectionAssert.AreEqual(expected, results, comparer);
                }
            }
        }

        [TestMethod]
        public void Query_ShouldReturnPagedEntityCollection()
        {
            // arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var comparer = new TestEntityComparer();
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 },
                new TestEntity { Id = 3 },
                new TestEntity { Id = 4 },
                new TestEntity { Id = 5 },
                new TestEntity { Id = 6 },
                new TestEntity { Id = 7 },
                new TestEntity { Id = 8 },
                new TestEntity { Id = 9 }
            };

            var expected = new List<TestEntity> { new TestEntity { Id = 6 }, new TestEntity { Id = 7 }, new TestEntity { Id = 8 } };

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.GetAllOf1IDbConnectionIDbTransactionNullableOfInt32((connection, transaction, timeout) => entities);

                using (var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object))
                {
                    // act
                    var results = repository.Query(skip: 5, take: 3, timeout: DefaultTimeout).OrderBy(x => x.Id).ToList();

                    // assert
                    Assert.IsNotNull(results);
                    CollectionAssert.AreEqual(expected, results, comparer);
                }
            }
        }

        [TestMethod]
        public void Add_ShouldSucceed()
        {
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var comparer = new TestEntityComparer();
            var database = new List<TestEntity>();
            var expected = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "One" }
            };

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.InsertOf1IDbConnectionM0IDbTransactionNullableOfInt32<TestEntity>((connection, entity, transaction, timeout) =>
                {
                    database.Add(entity);
                    return entity.Id;
                });

                using (var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object))
                {
                    // act
                    repository.Add(new TestEntity { Id = 1, Name = "One" }, DefaultTimeout);

                    // assert
                    Assert.AreEqual(1, database.Count);
                    CollectionAssert.AreEqual(expected, database, comparer);
                }
            }
        }

        [TestMethod]
        public void Add_Collection_ShouldSucceed()
        {
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var comparer = new TestEntityComparer();
            var database = new List<TestEntity>();
            var expected = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "One" },
                new TestEntity { Id = 2, Name = "Two" },
                new TestEntity { Id = 3, Name = "Three" },
                new TestEntity { Id = 4, Name = "Four" },
                new TestEntity { Id = 5, Name = "Five" }
            };

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.InsertOf1IDbConnectionM0IDbTransactionNullableOfInt32<IEnumerable<TestEntity>>((connection, entities, transaction, timeout) =>
                {
                    var id = 0;

                    foreach (var entity in entities.ToList())
                    {
                        database.Add(entity);
                        id = entity.Id;
                    }

                    return id;
                });

                using (var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object))
                {
                    // act
                    repository.Add(expected, DefaultTimeout);

                    // assert
                    CollectionAssert.AreEqual(expected, database, comparer);
                }
            }
        }

        [TestMethod]
        public void Update_ShouldSucceed()
        {
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var database = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "One" },
                new TestEntity { Id = 2, Name = "Two" },
                new TestEntity { Id = 3, Name = "Three" },
                new TestEntity { Id = 4, Name = "Four" },
                new TestEntity { Id = 5, Name = "Five" }
            };

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.UpdateOf1IDbConnectionM0IDbTransactionNullableOfInt32<TestEntity>((connection, entity, transaction, timeout) =>
                {
                    var storeEntity = database.Single(x => x.Id == entity.Id);
                    storeEntity.Name = entity.Name;
                    return true;
                });

                var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object);

                // act
                repository.Update(new TestEntity { Id = 1, Name = "Gonzo" }, DefaultTimeout);

                // assert
                Assert.AreEqual("Gonzo", database.First(x => x.Id == 1).Name);
            }
        }

        [TestMethod]
        public void Update_Collection_ShouldSucceed()
        {
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var comparer = new TestEntityComparer();
            var database = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "One" },
                new TestEntity { Id = 2, Name = "Two" },
                new TestEntity { Id = 3, Name = "Three" },
                new TestEntity { Id = 4, Name = "Four" },
                new TestEntity { Id = 5, Name = "Five" }
            };

            var expected = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "1" },
                new TestEntity { Id = 2, Name = "2" },
                new TestEntity { Id = 3, Name = "3" },
                new TestEntity { Id = 4, Name = "4" },
                new TestEntity { Id = 5, Name = "5" }
            };

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.UpdateOf1IDbConnectionM0IDbTransactionNullableOfInt32<IEnumerable<TestEntity>>((connection, entities, transaction, timeout) =>
                {
                    foreach (var entity in entities)
                    {
                        var storeEntity = database.First(x => x.Id == entity.Id);
                        storeEntity.Name = entity.Name;
                    }

                    return true;
                });

                using (var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object))
                {
                    // act
                    repository.Update(expected, DefaultTimeout);

                    // assert
                    CollectionAssert.AreEqual(expected, database, comparer);
                }
            }
        }

        [TestMethod]
        public void Delete_ShouldSucceed()
        {
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var comparer = new TestEntityComparer();
            var database = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "One" },
                new TestEntity { Id = 2, Name = "Two" },
                new TestEntity { Id = 3, Name = "Three" },
                new TestEntity { Id = 4, Name = "Four" },
                new TestEntity { Id = 5, Name = "Five" }
            };

            var expected = new List<TestEntity>
            {
                new TestEntity { Id = 2, Name = "Two" },
                new TestEntity { Id = 3, Name = "Three" },
                new TestEntity { Id = 4, Name = "Four" },
                new TestEntity { Id = 5, Name = "Five" }
            };

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.DeleteOf1IDbConnectionM0IDbTransactionNullableOfInt32<TestEntity>((connection, entity, transaction, timeout) => database.Remove(entity));

                using (var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object))
                {
                    // act
                    repository.Delete(database.First(), DefaultTimeout);

                    // assert
                    CollectionAssert.AreEqual(expected, database, comparer);
                }
            }
        }

        [TestMethod]
        public void Delete_Collection_ShouldSucceed()
        {
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();
            var comparer = new TestEntityComparer();
            var database = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "One" },
                new TestEntity { Id = 2, Name = "Two" },
                new TestEntity { Id = 3, Name = "Three" },
                new TestEntity { Id = 4, Name = "Four" },
                new TestEntity { Id = 5, Name = "Five" }
            };

            var expected = new List<TestEntity>
            {
                new TestEntity { Id = 5, Name = "Five" }
            };

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.DeleteOf1IDbConnectionM0IDbTransactionNullableOfInt32<IEnumerable<TestEntity>>((connection, entities, transaction, timeout) =>
                {
                    foreach (var entity in entities)
                    {
                        database.Remove(entity);
                    }

                    return true;
                });

                using (var repository = new Repository<TestEntity>(connectionMock.Object, transactionMock.Object))
                {
                    // act
                    repository.Delete(database.Take(4).ToList(), DefaultTimeout);

                    // assert
                    CollectionAssert.AreEqual(expected, database, comparer);
                }
            }
        }
    }
}

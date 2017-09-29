namespace Dapper.Repositories.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Castle.Components.DictionaryAdapter.Xml;
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
        public void ConstructorShouldThrowExceptionNullConnection()
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
        public void ConstructorShouldThrowExceptionNullConnectionNullTransaction()
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
        public void GetShouldReturnEntityInstance()
        {
            // arrange
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            var entity = new TestEntity { Id = 1 };
            
            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.GetOf1IDbConnectionObjectIDbTransactionNullableOfInt32((c, id, t, s) => entity);

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);

                // act
                var result = repository.Get(1, DefaultTimeout);

                // assert
                Assert.IsNotNull(result);
                Assert.AreEqual(entity, result);
            }
        }

        [TestMethod]
        public void GetShouldReturnNull()
        {
            // arrange
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            
            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.GetOf1IDbConnectionObjectIDbTransactionNullableOfInt32((c, id, t, s) => default(TestEntity));

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);

                // act
                var result = repository.Get(1, DefaultTimeout);

                // assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void QueryShouldReturnEntityCollection()
        {
            // arrange
            var entities = new List<TestEntity>();
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();

            entities.Add(new TestEntity { Id = 1 });
            entities.Add(new TestEntity { Id = 2 });
            entities.Add(new TestEntity { Id = 3 });
            entities.Add(new TestEntity { Id = 4 });
            entities.Add(new TestEntity { Id = 5 });
            entities.Add(new TestEntity { Id = 6 });
            entities.Add(new TestEntity { Id = 7 });
            entities.Add(new TestEntity { Id = 8 });
            entities.Add(new TestEntity { Id = 9 });

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.GetAllOf1IDbConnectionIDbTransactionNullableOfInt32((c, t, to) => entities);

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);

                // act
                var results = repository.Query(timeout: DefaultTimeout).OrderBy(x => x.Id).ToList();

                // assert
                CollectionAssert.AreEqual(entities, results, new TestEntityComparer());
            }
        }

        [TestMethod]
        public void QueryShouldReturnFilteredEntityCollection()
        {
            // arrange
            var entities = new List<TestEntity>();
            var expected = new List<TestEntity>();
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();

            entities.Add(new TestEntity { Id = 1 });
            entities.Add(new TestEntity { Id = 2 });
            entities.Add(new TestEntity { Id = 3 });
            entities.Add(new TestEntity { Id = 4 });
            entities.Add(new TestEntity { Id = 5 });
            entities.Add(new TestEntity { Id = 6 });
            entities.Add(new TestEntity { Id = 7 });
            entities.Add(new TestEntity { Id = 8 });
            entities.Add(new TestEntity { Id = 9 });

            expected.Add(new TestEntity { Id = 2 });
            expected.Add(new TestEntity { Id = 4 });
            expected.Add(new TestEntity { Id = 6 });
            expected.Add(new TestEntity { Id = 8 });

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.GetAllOf1IDbConnectionIDbTransactionNullableOfInt32((c, t, to) => entities);

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);

                // act
                var results = repository.Query(x => x.Id % 2 == 0, timeout: DefaultTimeout).OrderBy(x => x.Id).ToList();

                // assert
                Assert.IsNotNull(results);
                CollectionAssert.AreEqual(expected, results, new TestEntityComparer());
            }
        }

        [TestMethod]
        public void QueryShouldReturnPagedEntityCollection()
        {
            // arrange
            var entities = new List<TestEntity>();
            var expected = new List<TestEntity>();
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();

            entities.Add(new TestEntity { Id = 1 });
            entities.Add(new TestEntity { Id = 2 });
            entities.Add(new TestEntity { Id = 3 });
            entities.Add(new TestEntity { Id = 4 });
            entities.Add(new TestEntity { Id = 5 });
            entities.Add(new TestEntity { Id = 6 });
            entities.Add(new TestEntity { Id = 7 });
            entities.Add(new TestEntity { Id = 8 });
            entities.Add(new TestEntity { Id = 9 });
            
            expected.Add(new TestEntity { Id = 6 });
            expected.Add(new TestEntity { Id = 7 });
            expected.Add(new TestEntity { Id = 8 });

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.GetAllOf1IDbConnectionIDbTransactionNullableOfInt32((c, t, to) => entities);

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);

                // act
                var results = repository.Query(skip: 5, take: 3, timeout: DefaultTimeout).OrderBy(x => x.Id).ToList();

                // assert
                Assert.IsNotNull(results);
                CollectionAssert.AreEqual(expected, results, new TestEntityComparer());
            }
        }

        [TestMethod]
        public void AddEntityShouldSucceed()
        {
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            var store = new List<TestEntity>();
            var expected = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "One" }
            };

            using (ShimsContext.Create())
            {
                // fake the dapper extension method call
                ShimSqlMapperExtensions.InsertOf1IDbConnectionM0IDbTransactionNullableOfInt32<TestEntity>((c, e, t, to) =>
                {
                    store.Add(e);
                    return e.Id;
                });

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);
               
                // act
                repository.Add(new TestEntity { Id = 1, Name = "One" }, DefaultTimeout);

                // assert
                Assert.AreEqual(1, store.Count);
                CollectionAssert.AreEqual(expected, store, new TestEntityComparer());
            }
        }

        [TestMethod]
        public void AddEntityCollectionShouldSucceed()
        {
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            var store = new List<TestEntity>();
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
                ShimSqlMapperExtensions.InsertOf1IDbConnectionM0IDbTransactionNullableOfInt32<IEnumerable<TestEntity>>((c, e, t, to) =>
                {
                    var id = 0;

                    foreach (var entity in e.ToList())
                    {
                        store.Add(entity);
                        id = entity.Id;
                    }

                    return id;
                });

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);

                // act
                repository.Add(expected, DefaultTimeout);

                // assert
                CollectionAssert.AreEqual(expected, store, new TestEntityComparer());
            }
        }

        [TestMethod]
        public void UpdateEntityShouldSucceed()
        {
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            var store = new List<TestEntity>
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
                ShimSqlMapperExtensions.UpdateOf1IDbConnectionM0IDbTransactionNullableOfInt32<TestEntity>((c, e, t, to) =>
                {
                    var entity = store.Single(x => x.Id == e.Id);
                    entity.Name = e.Name;
                    return true;
                });

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);

                // act
                repository.Update(new TestEntity { Id = 1, Name = "Gonzo" }, DefaultTimeout);

                // assert
                Assert.AreEqual("Gonzo", store.Single(x => x.Id == 1).Name);
            }
        }

        [TestMethod]
        public void UpdateEntityCollectionShouldSucceed()
        {
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            var store = new List<TestEntity>
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
                ShimSqlMapperExtensions.UpdateOf1IDbConnectionM0IDbTransactionNullableOfInt32<IEnumerable<TestEntity>>((c, e, t, to) =>
                {
                    foreach (var entity in e)
                    {
                        var se = store.First(x => x.Id == entity.Id);
                        se.Name = entity.Name;
                    }

                    return true;
                });

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);

                // act
                repository.Update(expected, DefaultTimeout);

                // assert
                CollectionAssert.AreEqual(expected, store, new TestEntityComparer());
            }
        }

        [TestMethod]
        public void DeleteEntityShouldSucceed()
        {
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            var store = new List<TestEntity>
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
                ShimSqlMapperExtensions.DeleteOf1IDbConnectionM0IDbTransactionNullableOfInt32<TestEntity>((c, e, t, to) => store.Remove(e));

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);

                // act
                repository.Delete(store.First(), DefaultTimeout);

                // assert
                Assert.AreEqual(4, store.Count);
                CollectionAssert.AreEqual(expected, store, new TestEntityComparer());
            }
        }

        [TestMethod]
        public void DeleteEntityCollectionShouldSucceed()
        {
            var connection = new Mock<IDbConnection>();
            var transaction = new Mock<IDbTransaction>();
            var store = new List<TestEntity>
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
                ShimSqlMapperExtensions.DeleteOf1IDbConnectionM0IDbTransactionNullableOfInt32<IEnumerable<TestEntity>>((c, e, t, to) =>
                {
                    foreach (var entity in e)
                    {
                        store.Remove(entity);
                    }

                    return true;
                });

                var repository = new Repository<TestEntity>(connection.Object, transaction.Object);
                
                // act
                repository.Delete(store.Take(4).ToList(), DefaultTimeout);

                // assert
                Assert.AreEqual(1, store.Count);
                CollectionAssert.AreEqual(expected, store, new TestEntityComparer());
            }
        }

        public void FinalizerShouldBeInvokedButNotDispose()
        {
            var connection = new Mock<IDbConnection>();
            connection.Setup(c => c.Dispose()).Verifiable();

            var repository = new Mock<Repository<TestEntity>>(connection.Object);
            repository.Setup(r => r.Dispose()).Verifiable();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            repository.Verify(r => r.Dispose(), Times.Never);
            repository.Verify(c => c.Dispose(), Times.Never);
        }
    }
}

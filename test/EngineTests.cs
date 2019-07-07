using AngelORM.Tests.Core;
using AngelORM.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AngelORM.Tests
{
    [Collection("Transaction")]
    public class EngineTests
    {
        private Engine _engine = new Engine(Settings.DatabaseConnectionString);

        [Fact]
        public void Select_not_throws_exception()
        {
            List<User> users = _engine.Select<User>().ToList();
        }

        [Fact]
        public void SelectWhere_property_equals_to_string_constant()
        {
            List<User> users = _engine.Select<User>().Where(x => x.Name == "Angel").ToList();
        }

        [Fact]
        public void SelectWhere_id_between_int_constants()
        {
            List<User> users = _engine.Select<User>().Where(x => x.Id > 5 && x.Id < 16).ToList();
        }

        [Fact]
        public void InsertTest()
        {
            using (Transaction transaction = _engine.BeginTransaction())
            {
                try
                {
                    User user = new User();
                    user.Name = "foo";
                    user.Username = "bar";
                    user.Password = "qwerty";
                    user.Email = "baz@qux.com";
                    user.CreatedDate = DateTime.Now;
                    user.Active = true;

                    _engine.Insert(user);

                    transaction.Rollback();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        [Fact]
        public void InsertTest_inserted_id_assings_to_id_property()
        {
            using (Transaction transaction = _engine.BeginTransaction())
            {
                try
                {
                    UserCreator.CreateNewUserWithName(_engine, "UpdateTest");

                    int lastUserId = (int)(decimal)_engine.ExecuteScalar("SELECT IDENT_CURRENT('User')");

                    User user = new User();
                    user.Name = "foo";
                    user.Username = "bar";
                    user.Password = "qwerty";
                    user.Email = "baz@qux.com";
                    user.CreatedDate = DateTime.Now;
                    user.Active = true;

                    _engine.Insert(user);

                    Assert.Equal(lastUserId + 1, user.Id);

                    transaction.Rollback();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        [Fact]
        public void UpdateTest()
        {
            using (Transaction transaction = _engine.BeginTransaction())
            {
                try
                {
                    List<User> users = _engine.Select<User>().ToList();
                    
                    if (users.Count == 0)
                    {
                        UserCreator.CreateNewUserWithName(_engine, "Name3");
                        UserCreator.CreateNewUserWithName(_engine, "Name1");
                        UserCreator.CreateNewUserWithName(_engine, "Name4");
                        UserCreator.CreateNewUserWithName(_engine, "Name2");
                    }

                    users = _engine.Select<User>().ToList();

                    User lastUser = users.Last();

                    lastUser.Username = Guid.NewGuid().ToString();

                    _engine.Update(lastUser);
                    
                    List<User> users2 = _engine.Select<User>().ToList();
                    User lastUser2 = users2.Last();

                    Assert.Equal(lastUser.Username, lastUser2.Username);

                    transaction.Rollback();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        [Fact]
        public void DeleteTest()
        {
            using (Transaction transaction = _engine.BeginTransaction())
            {
                try
                {
                    UserCreator.CreateNewUserWithName(_engine, "Name1");
                    UserCreator.CreateNewUserWithName(_engine, "Name2");

                    List<User> users = _engine.Select<User>().ToList();
                    
                    User lastUser = users.Last();

                    _engine.Delete(lastUser);
                    
                    List<User> users2 = _engine.Select<User>().ToList();
                    User lastUser2 = users2.Last();

                    Assert.NotEqual(lastUser.Id, lastUser2.Id);

                    transaction.Rollback();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        [Fact]
        public void OrderBy_with_one_column()
        {
            using (Transaction transaction = _engine.BeginTransaction())
            {
                try
                {
                    UserCreator.CreateNewUserWithName(_engine, "C");
                    UserCreator.CreateNewUserWithName(_engine, "A");
                    UserCreator.CreateNewUserWithName(_engine, "D");
                    UserCreator.CreateNewUserWithName(_engine, "B");

                    List<User> orderedByLinq = _engine.Select<User>().ToList().OrderBy(x => x.Username).ToList();
                    List<User> orderedByAngelORM = _engine.Select<User>().OrderBy(x => x.Username).ToList();

                    Assert.Equal(orderedByLinq.Count, orderedByAngelORM.Count);

                    for (int i = 0; i < orderedByLinq.Count; i++)
                    {
                        Assert.Equal(orderedByLinq[i].Id, orderedByAngelORM[i].Id);
                    }

                    transaction.Rollback();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        [Fact]
        public void OrderBy_with_multiple_column()
        {
            using (Transaction transaction = _engine.BeginTransaction())
            {
                try
                {
                    UserCreator.CreateNewUserWithName(_engine, "C");
                    UserCreator.CreateNewUserWithName(_engine, "A");
                    UserCreator.CreateNewUserWithName(_engine, "D");
                    UserCreator.CreateNewUserWithName(_engine, "B");

                    List<User> orderedByLinq = _engine.Select<User>().ToList().OrderBy(x => x.Username).ThenBy(x => x.Id).ToList();
                    List<User> orderedByAngelORM = _engine.Select<User>().OrderBy(x => x.Username).OrderBy(x => x.Id).ToList();

                    Assert.Equal(orderedByLinq.Count, orderedByAngelORM.Count);

                    for (int i = 0; i < orderedByLinq.Count; i++)
                    {
                        Assert.Equal(orderedByLinq[i].Id, orderedByAngelORM[i].Id);
                    }

                    transaction.Rollback();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        [Fact]
        public void OrderBy_with_multiple_column_and_multiple_order_type()
        {
            using (Transaction transaction = _engine.BeginTransaction())
            {
                try
                {
                    UserCreator.CreateNewUserWithName(_engine, "C");
                    UserCreator.CreateNewUserWithName(_engine, "A");
                    UserCreator.CreateNewUserWithName(_engine, "D");
                    UserCreator.CreateNewUserWithName(_engine, "B");

                    List<User> orderedByLinq = _engine.Select<User>().ToList().OrderBy(x => x.Name).ThenByDescending(x => x.Id).ToList();
                    List<User> orderedByAngelORM = _engine.Select<User>().OrderBy(x => x.Name).OrderByDescending(x => x.Id).ToList();

                    Assert.Equal(orderedByLinq.Count, orderedByAngelORM.Count);

                    for (int i = 0; i < orderedByLinq.Count; i++)
                    {
                        Assert.Equal(orderedByLinq[i].Id, orderedByAngelORM[i].Id);
                    }

                    transaction.Rollback();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }
    }
}

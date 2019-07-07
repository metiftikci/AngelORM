using AngelORM.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AngelORM.Tests
{
    public class EngineTests
    {
        public const string CS = "Server=(local)\\SQL2014;Database=AngelORM;User ID=sa;Password=Password12!";

        private Engine _engine = new Engine(CS);

        [Fact]
        public void Select_not_throws_exception()
        {
            List<User> users = _engine.Select<User>().ToList();
        }

        [Fact]
        public void SelectWhere_property_equals_to_string_constant()
        {
            List<User> users = _engine.Select<User>().Where(x => x.Name == "Muhammed").ToList();
        }

        [Fact]
        public void SelectWhere_id_between_int_constants()
        {
            List<User> users = _engine.Select<User>().Where(x => x.Id > 5 && x.Id < 16).ToList();
        }

        [Fact]
        public void InsertTest()
        {
            User user = new User();
            user.Name = "foo";
            user.Username = "bar";
            user.Password = "qwerty";
            user.Password = "baz@qux.com";
            user.CreatedDate = DateTime.Now;
            user.Active = true;

            _engine.Insert(user);
        }

        [Fact]
        public void InsertTest_inserted_id_assings_to_id_property()
        {
            int lastUserId = (int)(decimal)_engine.ExecuteScalar("SELECT IDENT_CURRENT('User')");

            User user = new User();
            user.Name = "foo";
            user.Username = "bar";
            user.Password = "qwerty";
            user.Password = "baz@qux.com";
            user.CreatedDate = DateTime.Now;
            user.Active = true;

            _engine.Insert(user);

            Assert.Equal(lastUserId + 1, user.Id);
        }

        [Fact]
        public void UpdateTest()
        {
            List<User> users = _engine.Select<User>().ToList();
            
            if (users.Count == 0)
            {
                AddNewUser();
                AddNewUser();
                AddNewUser();
                AddNewUser();
            }

            users = _engine.Select<User>().ToList();

            User lastUser = users.Last();

            lastUser.Username = Guid.NewGuid().ToString();

            _engine.Update(lastUser);
            
            List<User> users2 = _engine.Select<User>().ToList();
            User lastUser2 = users2.Last();

            Assert.Equal(lastUser.Username, lastUser2.Username);
        }

        [Fact]
        public void DeleteTest()
        {
            List<User> users = _engine.Select<User>().ToList();
            
            if (users.Count == 0)
            {
                AddNewUser();
                AddNewUser();
                AddNewUser();
                AddNewUser();
            }

            users = _engine.Select<User>().ToList();
            
            User lastUser = users.Last();

            _engine.Delete(lastUser);
            
            List<User> users2 = _engine.Select<User>().ToList();
            User lastUser2 = users2.Last();

            Assert.NotEqual(lastUser.Id, lastUser2.Id);
        }

        [Fact]
        public void OrderBy_with_one_column()
        {
            using (Transaction transaction = _engine.BeginTransaction())
            {
                try
                {
                    _engine.ExecuteNonQuery("TRUNCATE TABLE [User]");

                    AddNewUserWithName("C");
                    AddNewUserWithName("A");
                    AddNewUserWithName("D");
                    AddNewUserWithName("B");

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
                    _engine.ExecuteNonQuery("TRUNCATE TABLE [User]");

                    AddNewUserWithName("C");
                    AddNewUserWithName("A");
                    AddNewUserWithName("D");
                    AddNewUserWithName("B");

                    List<User> orderedByLinq = _engine.Select<User>().ToList().OrderBy(x => x.Username).OrderBy(x => x.Id).ToList();
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
                    _engine.ExecuteNonQuery("TRUNCATE TABLE [User]");

                    AddNewUserWithName("C");
                    AddNewUserWithName("A");
                    AddNewUserWithName("D");
                    AddNewUserWithName("B");

                    List<User> orderedByLinq = _engine.Select<User>().ToList().OrderBy(x => x.Username).OrderByDescending(x => x.Id).ToList();
                    List<User> orderedByAngelORM = _engine.Select<User>().OrderBy(x => x.Username).OrderByDescending(x => x.Id).ToList();

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
                }
            }
        }

        private void AddNewUser()
        {
            User user = new User();
            user.Name = "Foo";
            user.Username = Guid.NewGuid().ToString();
            user.Password = "qwerty";
            user.Password = "baz@qux.com";
            user.CreatedDate = DateTime.Now;
            user.Active = true;

            _engine.Insert(user);
        }

        private void AddNewUserWithName(string name)
        {
            User user = new User();
            user.Name = name;
            user.Username = Guid.NewGuid().ToString();
            user.Password = "qwerty";
            user.Password = "baz@qux.com";
            user.CreatedDate = DateTime.Now;
            user.Active = true;

            _engine.Insert(user);
        }
    }
}

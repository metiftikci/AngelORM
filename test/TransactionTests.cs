using AngelORM;
using AngelORM.Tests.Core;
using AngelORM.Tests.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace AngelORM.Tests
{
    [Collection("Transaction")]
    public class TransactionTests
    {
        private Engine _engine = new Engine(Settings.DatabaseConnectionString);

        [Fact]
        public void Rollback_check_users_count()
        {
            using (Transaction transaction = _engine.BeginTransaction())
            {
                bool didRollback = false;

                try
                {
                    UserCreator.CreateNewUserWithName(_engine, "transaction test");

                    int id = 0;

                    User user = new User();
                    user.Name = Guid.NewGuid().ToString();
                    user.Surname = "Foo";
                    user.Password = "Bar";
                    user.CreatedDate = DateTime.Now;

                    _engine.Insert(user);
                    Assert.NotEqual(0, user.Id);

                    id = user.Id;

                    transaction.Rollback();

                    didRollback = true;

                    var users = _engine.Select<User>().Where(x => x.Id == id).ToList();

                    int newCount = users.Count;

                    Assert.Equal(0, newCount);
                }
                catch
                {
                    if (!didRollback)
                    {
                        transaction.Rollback();
                    }

                    throw;
                }
            }
        }

        [Fact]
        public void Rollback_update()
        {
            using (Transaction transaction = _engine.BeginTransaction())
            {
                bool didRollback = false;

                try
                {
                    UserCreator.CreateNewUserWithName(_engine, "User1");
                    UserCreator.CreateNewUserWithName(_engine, "User2");
                    UserCreator.CreateNewUserWithName(_engine, "User3");
                    UserCreator.CreateNewUserWithName(_engine, "User4");

                    User user = _engine.Select<User>().ToList()[1];

                    string temp = user.Name;
                    user.Name = Guid.NewGuid().ToString();

                    int affectedRows = _engine.Update(user);

                    Assert.Equal(1, affectedRows);

                    transaction.Rollback();

                    didRollback = true;

                    int newCount = _engine.Select<User>().ToList().Count;

                    Assert.Equal(0, newCount);
                }
                catch
                {
                    if (!didRollback)
                    {
                        transaction.Rollback();
                    }

                    throw;
                }
            }
        }
    }
}

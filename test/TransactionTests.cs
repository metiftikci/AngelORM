using AngelORM;
using AngelORM.Tests.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace AngelORM.Tests
{
    public class TransactionTests
    {
        private Engine _engine = new Engine(EngineTests.CS);

        [Fact]
        public void Rollback_check_users_count()
        {
            int id = 0;

            using (Transaction transaction = _engine.BeginTransaction())
            {
                User user = new User();
                user.Name = Guid.NewGuid().ToString();
                user.Surname = "Foo";
                user.Password = "Bar";
                user.CreatedDate = DateTime.Now;

                _engine.Insert(user);
                Assert.NotEqual(0, user.Id);

                id = user.Id;

                transaction.Rollback();
            }

            var users = _engine.Select<User>().Where(x => x.Id == id).ToList();

            int newCount = users.Count;

            Assert.Equal(0, newCount);
        }

        [Fact]
        public void Rollback_update()
        {
            List<User> users = _engine.Select<User>().ToList();

            User user = null;
            
            if (users.Count == 0)
            {
                user = new User();
                user.Name = Guid.NewGuid().ToString();
                user.CreatedDate = DateTime.Now;

                _engine.Insert(user);
                user.Id = 0;
                _engine.Insert(user);
                user.Id = 0;
                _engine.Insert(user);
                user.Id = 0;
                _engine.Insert(user);
            }

            users = _engine.Select<User>().ToList();
            user = users[0];

            using (Transaction transaction = _engine.BeginTransaction())
            {
                string temp = user.Name;
                user.Name = Guid.NewGuid().ToString();

                int affectedRows = _engine.Update(user);

                Assert.Equal(1, affectedRows);

                transaction.Rollback();

                user = _engine.Select<User>().Where(x => x.Id == user.Id).ToList()[0];

                Assert.Equal(temp, user.Name);
            }
        }
    }
}

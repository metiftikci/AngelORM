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
            List<User> users = _engine.Select<User>().ToList();
            List<User> users2;
            List<User> users3;

            using (Transaction transaction = _engine.BeginTransaction())
            {
                User user = new User();
                user.Name = Guid.NewGuid().ToString();
                user.Surname = "Foo";
                user.Password = "Bar";
                user.CreatedDate = DateTime.Now;

                _engine.Insert(user);
                Assert.NotEqual(0, user.Id);
                
                user.Id = 0;
                _engine.Insert(user);
                Assert.NotEqual(0, user.Id);
                
                user.Id = 0;
                _engine.Insert(user);
                Assert.NotEqual(0, user.Id);

                users2 = _engine.Select<User>().ToList();

                Assert.Equal(users.Count, users2.Count - 3);

                transaction.Rollback();

                users3 = _engine.Select<User>().ToList();
            }

            Assert.Equal(users.Count, users3.Count);
            Assert.Equal(users2.Count, users3.Count + 3);
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

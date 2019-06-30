using System;
using System.Collections.Generic;
using System.Linq;
using AngelORM.Tests.Models;
using Xunit;

namespace AngelORM.Tests
{
    public class EngineTests
    {
        private const string CS = "Server=MONSTER\\MET; Database=AngelORM; Integrated Security=True;";

        private Engine _engine = new Engine(CS);

        [Fact]
        public void ToListTest()
        {
            List<User> users = _engine.ToList<User>();

            Assert.Equal("muhammed", users[0].Username);
            Assert.Equal("jaqra@hotmail.com", users[0].Email);
        }

        [Fact]
        public void InsertTest()
        {
            User user = new User();
            user.Name = "foo";
            user.Username = "bar";
            user.Password = "qwerty";
            user.Password = "baz@qux.com";

            _engine.Insert(user);
        }

        [Fact]
        public void InsertTest_check_inserted_key()
        {
            int lastUserId = (int)(decimal)_engine.ExecuteScalar("SELECT IDENT_CURRENT('User')");

            User user = new User();
            user.Name = "foo";
            user.Username = "bar";
            user.Password = "qwerty";
            user.Password = "baz@qux.com";

            _engine.Insert(user);

            Assert.Equal(lastUserId + 1, user.Id);
        }

        [Fact]
        public void UpdateTest()
        {
            List<User> users = _engine.ToList<User>();
            User lastUser = users.Last();

            lastUser.Username = Guid.NewGuid().ToString();

            _engine.Update(lastUser);
            
            List<User> users2 = _engine.ToList<User>();
            User lastUser2 = users2.Last();

            Assert.Equal(lastUser.Username, lastUser2.Username);
        }

        [Fact]
        public void DeleteTest()
        {
            List<User> users = _engine.ToList<User>();
            User lastUser = users.Last();

            _engine.Delete(lastUser);
            
            List<User> users2 = _engine.ToList<User>();
            User lastUser2 = users2.Last();

            Assert.NotEqual(lastUser.Id, lastUser2.Id);
        }
    }
}

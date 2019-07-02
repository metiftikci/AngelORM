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
        public void SelectTest()
        {
            List<User> users = _engine.Select<User>().ToList();

            Assert.Equal("muhammed", users[0].Username);
            Assert.Equal("jaqra@hotmail.com", users[0].Email);
        }

        [Fact]
        public void SelectWhereTest()
        {
            List<User> users = _engine.Select<User>().Where(x => x.Name == "Muhammed").ToList();

            Assert.Equal(1, users.Count);
            Assert.Equal("muhammed", users[0].Username);
            Assert.Equal("jaqra@hotmail.com", users[0].Email);

            users = _engine.Select<User>().Where(x => x.Id > 5 && x.Id < 16).ToList();

            Assert.Equal(5, users.Count);
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
            List<User> users = _engine.Select<User>().ToList();
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
            User lastUser = users.Last();

            _engine.Delete(lastUser);
            
            List<User> users2 = _engine.Select<User>().ToList();
            User lastUser2 = users2.Last();

            Assert.NotEqual(lastUser.Id, lastUser2.Id);
        }
    }
}

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
        [Fact]
        public void Select_not_throws_exception()
        {
            TestContext.Run(engine => {
                List<User> users = engine.Select<User>().ToList();
            });
        }

        [Fact]
        public void SelectWhere_property_equals_to_string_constant()
        {
            TestContext.Run(engine => {
                List<User> users = engine.Select<User>().Where(x => x.Name == "Angel").ToList();
            });
        }

        [Fact]
        public void SelectWhere_id_between_int_constants()
        {
            TestContext.Run(engine => {
                List<User> users = engine.Select<User>().Where(x => x.Id > 5 && x.Id < 16).ToList();
            });
        }

        [Fact]
        public void InsertTest()
        {
            TestContext.Run(engine => {
                User user = new User();
                user.Name = "foo";
                user.Username = "bar";
                user.Password = "qwerty";
                user.Email = "baz@qux.com";
                user.CreatedDate = DateTime.Now;
                user.Active = true;

                engine.Insert(user);
            });
        }

        [Fact]
        public void InsertTest_inserted_id_assings_to_id_property()
        {
            TestContext.Run(engine => {
                UserCreator.CreateNewUserWithName(engine, "UpdateTest");

                int lastUserId = (int)(decimal)engine.ExecuteScalar("SELECT IDENT_CURRENT('User')");

                User user = new User();
                user.Name = "foo";
                user.Username = "bar";
                user.Password = "qwerty";
                user.Email = "baz@qux.com";
                user.CreatedDate = DateTime.Now;
                user.Active = true;

                engine.Insert(user);

                Assert.Equal(lastUserId + 1, user.Id);
            });
        }

        [Fact]
        public void UpdateTest()
        {
            TestContext.Run(engine => {
                List<User> users = engine.Select<User>().ToList();
                
                if (users.Count == 0)
                {
                    UserCreator.CreateNewUserWithName(engine, "Name3");
                    UserCreator.CreateNewUserWithName(engine, "Name1");
                    UserCreator.CreateNewUserWithName(engine, "Name4");
                    UserCreator.CreateNewUserWithName(engine, "Name2");
                }

                users = engine.Select<User>().ToList();

                User lastUser = users.Last();

                lastUser.Username = Guid.NewGuid().ToString();

                engine.Update(lastUser);
                
                List<User> users2 = engine.Select<User>().ToList();
                User lastUser2 = users2.Last();

                Assert.Equal(lastUser.Username, lastUser2.Username);
            });
        }

        [Fact]
        public void DeleteTest()
        {
            TestContext.Run(engine => {
                UserCreator.CreateNewUserWithName(engine, "Name1");
                UserCreator.CreateNewUserWithName(engine, "Name2");

                List<User> users = engine.Select<User>().ToList();
                
                User lastUser = users.Last();

                engine.Delete(lastUser);
                
                List<User> users2 = engine.Select<User>().ToList();
                User lastUser2 = users2.Last();

                Assert.NotEqual(lastUser.Id, lastUser2.Id);
            });
        }

        [Fact]
        public void OrderBy_with_one_column()
        {
            TestContext.Run(engine => {
                UserCreator.CreateNewUserWithName(engine, "C");
                UserCreator.CreateNewUserWithName(engine, "A");
                UserCreator.CreateNewUserWithName(engine, "D");
                UserCreator.CreateNewUserWithName(engine, "B");

                List<User> orderedByLinq = engine.Select<User>().ToList().OrderBy(x => x.Username).ToList();
                List<User> orderedByAngelORM = engine.Select<User>().OrderBy(x => x.Username).ToList();

                Assert.Equal(orderedByLinq.Count, orderedByAngelORM.Count);

                for (int i = 0; i < orderedByLinq.Count; i++)
                {
                    Assert.Equal(orderedByLinq[i].Id, orderedByAngelORM[i].Id);
                }
            });
        }

        [Fact]
        public void OrderBy_with_multiple_column()
        {
            TestContext.Run(engine => {
                UserCreator.CreateNewUserWithName(engine, "C");
                UserCreator.CreateNewUserWithName(engine, "A");
                UserCreator.CreateNewUserWithName(engine, "D");
                UserCreator.CreateNewUserWithName(engine, "B");

                List<User> orderedByLinq = engine.Select<User>().ToList().OrderBy(x => x.Username).ThenBy(x => x.Id).ToList();
                List<User> orderedByAngelORM = engine.Select<User>().OrderBy(x => x.Username).OrderBy(x => x.Id).ToList();

                Assert.Equal(orderedByLinq.Count, orderedByAngelORM.Count);

                for (int i = 0; i < orderedByLinq.Count; i++)
                {
                    Assert.Equal(orderedByLinq[i].Id, orderedByAngelORM[i].Id);
                }
            });
        }

        [Fact]
        public void OrderBy_with_multiple_column_and_multiple_order_type()
        {
            TestContext.Run(engine => {
                UserCreator.CreateNewUserWithName(engine, "C");
                UserCreator.CreateNewUserWithName(engine, "A");
                UserCreator.CreateNewUserWithName(engine, "D");
                UserCreator.CreateNewUserWithName(engine, "B");

                List<User> orderedByLinq = engine.Select<User>().ToList().OrderBy(x => x.Name).ThenByDescending(x => x.Id).ToList();
                List<User> orderedByAngelORM = engine.Select<User>().OrderBy(x => x.Name).OrderByDescending(x => x.Id).ToList();

                Assert.Equal(orderedByLinq.Count, orderedByAngelORM.Count);

                for (int i = 0; i < orderedByLinq.Count; i++)
                {
                    Assert.Equal(orderedByLinq[i].Id, orderedByAngelORM[i].Id);
                }
            });
        }
    }
}

using System;
using AngelORM.Tests.Models;

namespace AngelORM.Tests.Core
{
    public static class UserCreator
    {
        public static void CreateNewUserWithName(Engine engine, string name)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            if (name == null) throw new ArgumentNullException(nameof(name));

            User user = new User();
            user.Name = name;
            user.Username = Guid.NewGuid().ToString();
            user.Password = "qwerty";
            user.Email = "baz@qux.com";
            user.CreatedDate = DateTime.Now;
            user.Active = true;

            engine.Insert(user);
        }
    }
}

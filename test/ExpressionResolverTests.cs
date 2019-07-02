using AngelORM.Tests.Models;
using System;
using Xunit;

namespace AngelORM.Tests
{
    public class ExpressionResolverTests
    {
        [Fact]
        public void Test()
        {
            string sql = string.Empty;

            ExpressionResolver resolver = new ExpressionResolver();

            sql = resolver.Resolve<User>(x => x.Id == 5);
            Assert.Equal("([Id] = 5)", sql);
            
            sql = resolver.Resolve<User>(x => x.Id >= 5 && x.Name == "Muhammed");
            Assert.Equal("(([Id] >= 5) AND ([Name] = 'Muhammed'))", sql);

            DateTime date = new DateTime(2019, 7, 3, 1, 5, 0);

            sql = resolver.Resolve<ModelWithDateProp>(x => x.CreatedDate < date);
            Assert.Equal("([CreatedDate] < '2019-07-03 01:05:00')", sql);
        }
    }
}

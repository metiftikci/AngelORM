using AngelORM.Tests.Models;
using System;
using Xunit;

namespace AngelORM.Tests
{
    public class ExpressionResolverTests
    {
        private ExpressionResolver _resolver = new ExpressionResolver();

        [Fact]
        public void Resolve_property_equals_to_int_constant()
        {
            string sql = _resolver.Resolve<User>(x => x.Id == 5);
            Assert.Equal("([Id] = 5)", sql);
        }

        [Fact]
        public void Resolve_property_gtoe_to_int_constant_and_operator_prop_equals_to_string_constant()
        {
            string sql = _resolver.Resolve<User>(x => x.Id >= 5 && x.Name == "Muhammed");
            Assert.Equal("(([Id] >= 5) AND ([Name] = 'Muhammed'))", sql);
        }

        [Fact]
        public void Resolve_property_less_than_datetime_constant()
        {
            DateTime date = new DateTime(2019, 7, 3, 1, 5, 0);

            string sql = _resolver.Resolve<User>(x => x.CreatedDate < date);
            Assert.Equal("([CreatedDate] < '2019-07-03 01:05:00')", sql);
        }

        [Fact]
        public void Resolve_property_not_equals_to_bool_constant()
        {
            string sql = _resolver.Resolve<User>(x => x.Active != true);
            Assert.Equal("([Active] <> 1)", sql);
        }
    }
}

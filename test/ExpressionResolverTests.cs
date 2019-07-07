using AngelORM.Tests.Models;
using System;
using System.Linq.Expressions;
using Xunit;

namespace AngelORM.Tests
{
    public class ExpressionResolverTests
    {
        private ExpressionResolver _resolver = new ExpressionResolver();

        [Fact]
        public void ResolveWhere_Tests()
        {
            var list = new[]
            {
                new {
                    SQL = _resolver.ResolveWhere<User>(x => x.Id == 5),
                    Result = "([Id] = 5)"
                },
                new {
                    SQL = _resolver.ResolveWhere<User>(x => 5 == x.Id),
                    Result = "(5 = [Id])"
                },
                new {
                    SQL = _resolver.ResolveWhere<User>(x => 5 == 8),
                    // TODO: This sould not be allowed => WHERE 0
                    Result = "0" // 5 == 8 is False, False => 0
                },
                new {
                    SQL = _resolver.ResolveWhere<User>(x => x.Name == x.Surname),
                    Result = "([Name] = [Surname])"
                },
                new {
                    SQL = _resolver.ResolveWhere<User>(x => x.Id == 5 && x.Email == "foo"),
                    Result = "(([Id] = 5) AND ([Email] = 'foo'))"
                },
                new {
                    SQL = _resolver.ResolveWhere<User>(x => (x.Id == 5 && x.Email == "foo") || x.Active == true),
                    Result = "((([Id] = 5) AND ([Email] = 'foo')) OR ([Active] = 1))"
                },
                new {
                    SQL = _resolver.ResolveWhere<User>(x => x.Id == 5 && (x.Email == "foo" || x.Active == true)),
                    Result = "(([Id] = 5) AND (([Email] = 'foo') OR ([Active] = 1)))"
                },
            };

            foreach (var item in list)
            {
                Assert.Equal(item.SQL, item.Result);
            }
        }

        [Fact]
        public void ResolveWhere_property_equals_to_int_constant()
        {
            string sql = _resolver.ResolveWhere<User>(x => x.Id == 5);
            Assert.Equal("([Id] = 5)", sql);
        }

        [Fact]
        public void ResolveWhere_property_gtoe_to_int_constant_and_operator_prop_equals_to_string_constant()
        {
            string sql = _resolver.ResolveWhere<User>(x => x.Id >= 5 && x.Name == "jaqra");
            Assert.Equal("(([Id] >= 5) AND ([Name] = 'jaqra'))", sql);
        }

        [Fact]
        public void ResolveWhere_property_less_than_datetime_constant()
        {
            DateTime date = new DateTime(2019, 7, 3, 1, 5, 0);

            string sql = _resolver.ResolveWhere<User>(x => x.CreatedDate < date);
            Assert.Equal("([CreatedDate] < '2019-07-03 01:05:00')", sql);
        }

        [Fact]
        public void ResolveWhere_property_not_equals_to_bool_constant()
        {
            string sql = _resolver.ResolveWhere<User>(x => x.Active != true);
            Assert.Equal("([Active] <> 1)", sql);
        }

        [Fact]
        public void ResolveWhere_property_contains_string_constant()
        {
            string sql = _resolver.ResolveWhere<User>(x => x.Name.Contains("uham"));
            Assert.Equal("([Name] LIKE '%uham%')", sql);
        }

        [Fact]
        public void ResolveWhere_property_contains_string_local_value()
        {
            string value = "uham";

            string sql = _resolver.ResolveWhere<User>(x => x.Name.Contains(value));
            Assert.Equal("([Name] LIKE '%uham%')", sql);
        }

        [Fact]
        public void ResolveWhere_property_starts_with_string_constant()
        {
            string sql = _resolver.ResolveWhere<User>(x => x.Name.StartsWith("uham"));
            Assert.Equal("([Name] LIKE 'uham%')", sql);
        }

        [Fact]
        public void ResolveWhere_property_starts_with_string_local_value()
        {
            string value = "uham";

            string sql = _resolver.ResolveWhere<User>(x => x.Name.StartsWith(value));
            Assert.Equal("([Name] LIKE 'uham%')", sql);
        }

        [Fact]
        public void ResolveWhere_property_end_with_string_constant()
        {
            string sql = _resolver.ResolveWhere<User>(x => x.Name.EndsWith("uham"));
            Assert.Equal("([Name] LIKE '%uham')", sql);
        }

        [Fact]
        public void ResolveWhere_property_ends_with_string_local_value()
        {
            string value = "uham";

            string sql = _resolver.ResolveWhere<User>(x => x.Name.EndsWith(value));
            Assert.Equal("([Name] LIKE '%uham')", sql);
        }

        [Fact]
        public void ResolveWhere_when_equals_to_parameter_property_value()
        {
            ResolveWhere_sub_when_equals_to_parameter_property_value(new User() { Name="foo" });
        }

        public void ResolveWhere_sub_when_equals_to_parameter_property_value(User user)
        {
            string sql = _resolver.ResolveWhere<User>(x => x.Name == user.Name);

            Assert.Equal("([Name] = 'foo')", sql);
        }
    }
}

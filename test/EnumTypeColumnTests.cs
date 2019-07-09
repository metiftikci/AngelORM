using AngelORM.Tests.Core;
using AngelORM.Tests.Models;
using System.Collections.Generic;
using Xunit;

namespace AngelORM.Tests
{
    [Collection("Transaction")]
    public class EnumTypeColumnTests
    {
        [Fact]
        public void Insert()
        {
            TestContext.Run(engine => {
                TableWithEnumColumn model = new TableWithEnumColumn()
                {
                    Name = "Foo1",
                    Mode = TableWithEnumColumnMode.Baz,
                    Status = TableWithEnumColumnStatus.Bar
                };

                engine.Insert(model);

                Assert.NotEqual(0, model.Id);

                List<TableWithEnumColumn> list = engine.Select<TableWithEnumColumn>().ToList();

                Assert.Equal(1, list.Count);
                Assert.Equal("Foo1", list[0].Name);
                Assert.Equal(TableWithEnumColumnMode.Baz, list[0].Mode);
                Assert.Equal(TableWithEnumColumnStatus.Bar, list[0].Status);
            });
        }

        [Fact]
        public void Update()
        {
            TestContext.Run(engine => {
                TableWithEnumColumn model = new TableWithEnumColumn()
                {
                    Name = "Foo1",
                    Mode = TableWithEnumColumnMode.Baz,
                    Status = TableWithEnumColumnStatus.Bar
                };

                engine.Insert(model);

                Assert.NotEqual(0, model.Id);

                model.Mode = TableWithEnumColumnMode.Foo;
                model.Status = TableWithEnumColumnStatus.Baz;

                int affectedRows = engine.Update(model);

                List<TableWithEnumColumn> list = engine.Select<TableWithEnumColumn>().ToList();

                Assert.Equal(1, list.Count);
                Assert.Equal("Foo1", list[0].Name);
                Assert.Equal(TableWithEnumColumnMode.Foo, list[0].Mode);
                Assert.Equal(TableWithEnumColumnStatus.Baz, list[0].Status);
            });
        }
    }
}

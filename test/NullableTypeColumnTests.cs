using AngelORM.Tests.Core;
using AngelORM.Tests.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace AngelORM.Tests
{
    [Collection("Transaction")]
    public class NullableTypeColumnTests
    {
        private readonly DateTime NOW = new DateTime(2019, 7, 9, 21, 58, 12);

        [Fact]
        public void Insert_with_null_value()
        {
            TestContext.Run(engine => {
                TableWithNullableColumn foo = new TableWithNullableColumn()
                {
                    Name = "TableWithNullableColumn"
                };

                engine.Insert(foo);

                Assert.NotEqual(0, foo.Id);

                List<TableWithNullableColumn> list = engine.Select<TableWithNullableColumn>().ToList();

                Assert.Equal(1, list.Count);
                Assert.Equal("TableWithNullableColumn", list[0].Name);
                Assert.False(list[0].CreatedDate.HasValue);
            });
            
        }

        [Fact]
        public void Insert_with_not_null_value()
        {
            TestContext.Run(engine => {
                TableWithNullableColumn foo = new TableWithNullableColumn()
                {
                    Name = "TableWithNullableColumn",
                    CreatedDate = NOW
                };

                engine.Insert(foo);

                Assert.NotEqual(0, foo.Id);

                List<TableWithNullableColumn> list = engine.Select<TableWithNullableColumn>().ToList();
                
                Assert.Equal(1, list.Count);
                Assert.True(list[0].CreatedDate.HasValue);
                Assert.Equal(foo.CreatedDate.Value, list[0].CreatedDate.Value);
            });
        }

        [Fact]
        public void Select_when_null_value_exists()
        {
            TestContext.Run(engine => {
                engine.Insert(new TableWithNullableColumn() { Name = "TableWithNullableColumn1" });
                engine.Insert(new TableWithNullableColumn() { Name = "TableWithNullableColumn2", CreatedDate = NOW });
                engine.Insert(new TableWithNullableColumn() { Name = "TableWithNullableColumn3" });
                engine.Insert(new TableWithNullableColumn() { Name = "TableWithNullableColumn4", CreatedDate = NOW });

                List<TableWithNullableColumn> list = engine.Select<TableWithNullableColumn>().ToList();

                Assert.Equal(4, list.Count);
                Assert.Equal("TableWithNullableColumn1", list[0].Name);
                Assert.False(list[0].CreatedDate.HasValue);

                Assert.Equal("TableWithNullableColumn2", list[1].Name);
                Assert.True(list[1].CreatedDate.HasValue);
                
                Assert.Equal("TableWithNullableColumn3", list[2].Name);
                Assert.False(list[2].CreatedDate.HasValue);
                
                Assert.Equal("TableWithNullableColumn4", list[3].Name);
                Assert.True(list[3].CreatedDate.HasValue);
            });
        }

        [Fact]
        public void Update_new_value_is_null()
        {
            TestContext.Run(engine => {
                TableWithNullableColumn foo = new TableWithNullableColumn() { Name = "TableWithNullableColumn1", CreatedDate = NOW };

                engine.Insert(foo);

                foo.CreatedDate = null;

                int affectedRows = engine.Update(foo);

                Assert.Equal(1, affectedRows);

                List<TableWithNullableColumn> list = engine.Select<TableWithNullableColumn>().ToList();

                Assert.Equal(1, list.Count);
                Assert.False(list[0].CreatedDate.HasValue);
            });
        }

        [Fact]
        public void Update_new_value_is_not_null()
        {
            TestContext.Run(engine => {
                TableWithNullableColumn foo = new TableWithNullableColumn() { Name = "TableWithNullableColumn1" };

                engine.Insert(foo);

                foo.CreatedDate = NOW;

                int affectedRows = engine.Update(foo);

                Assert.Equal(1, affectedRows);

                List<TableWithNullableColumn> list = engine.Select<TableWithNullableColumn>().ToList();

                Assert.Equal(1, list.Count);
                Assert.True(list[0].CreatedDate.HasValue);
            });
        }
    }
}

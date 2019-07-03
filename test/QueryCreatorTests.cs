using AngelORM.Tests.Models;
using System;
using Xunit;

namespace AngelORM.Tests
{
    public class QueryCreatorTests
    {
        private QueryCreator _queryCreator = new QueryCreator();

        [Fact]
        public void CreateSelectQueryTest()
        {
            string query = @"SELECT [Id] AS [Id]
      ,[Name] AS [Name]
      ,[Surname] AS [Surname]
      ,[Username] AS [Username]
      ,[Password] AS [Password]
      ,[Email] AS [Email]
      ,[CreatedDate] AS [CreatedDate]
      ,[Active] AS [Active]
FROM [User]";

            string createdQuery = _queryCreator.CreateSelectQuery<User>();

            Assert.Equal(query, createdQuery);
        }

        [Fact]
        public void CreateInsertQueryTest()
        {
            string query = @"INSERT INTO [User] (
    [Name]
   ,[Surname]
   ,[Username]
   ,[Password]
   ,[Email]
   ,[CreatedDate]
   ,[Active]
)
OUTPUT inserted.[Id]
VALUES
(
    @Name
   ,@Surname
   ,@Username
   ,@Password
   ,@Email
   ,@CreatedDate
   ,@Active
)";

            string createdQuery = _queryCreator.CreateInsertQuery<User>();

            Assert.Equal(query, createdQuery);
        }

        [Fact]
        public void CreateUpdateQueryTest()
        {
            string query = @"UPDATE [User]
SET [Name] = @Name
   ,[Surname] = @Surname
   ,[Username] = @Username
   ,[Password] = @Password
   ,[Email] = @Email
   ,[CreatedDate] = @CreatedDate
   ,[Active] = @Active
WHERE [Id] = @Id";

            string createdQuery = _queryCreator.CreateUpdateQuery<User>();

            Assert.Equal(query, createdQuery);
        }

        [Fact]
        public void CreateDeleteQueryTest()
        {
            string query = "DELETE FROM [User] WHERE [Id] = @Id";
            string createdQuery = _queryCreator.CreateDeleteQuery<User>();

            Assert.Equal(query, createdQuery);
        }
    }
}

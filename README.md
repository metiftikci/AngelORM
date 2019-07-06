[![Build status](https://ci.appveyor.com/api/projects/status/4jmhhcdkrrk9qieh/branch/master?svg=true)](https://ci.appveyor.com/project/jaqra/angelorm/branch/master)
![AppVeyor tests (compact)](https://img.shields.io/appveyor/tests/jaqra/AngelORM.svg?compact_message)
![Nuget](https://img.shields.io/nuget/v/AngelORM.svg)
![GitHub](https://img.shields.io/github/license/jaqra/AngelORM.svg)

![LOGO](https://raw.githubusercontent.com/jaqra/AngelORM/master/assets/logo.jpg)

# Angel ORM

Basic and lightweight mssql operations framework.

## Why Angel ORM instead of Entity Framework?

- Lightweight
- Customizable
- Working with **all** model classes (does not require migration or anything)

## Fetaures

- **Select**: Get data as model list.
- **Where**: Get data with expression conditions.
- **Insert**: Insert new data to database.
- **Key**: Inserted key feedback to model.
- **Update**: Update row with auto detected key.
- **Delete**: Delete row with auto detected key.
- **Transaction**: Begin, Commit and Rollback transaction.
- **Raw Query**: Execute custom sql query.

## Roadmap

- Add Nullable<T> column type feature.
- Add enum column type feature.
- Allow Enumerable.Contains method in where expression to generate query like '[Column] IN (1,2,3,4)'.
- Implement data annotations to define table and column names.
- Add OnQueryCreated method to SelectOperation.
- Add OrderBy and OrderByDescending feature to SelectOperation.
- Add CreateTable<T>, CreateTableIfNotExists<T> and MigrateTable<T> methods to Engine.
- Validate model fetaure from data annotations on Insert and Update.

## Work On

Currently working on implement where feature with expression

```csharp
// ========== GOAL: Where Feature ==========

/* OK */ List<User> list = engine.Select<User>().ToList();
/* OK */ List<User> list = engine.Select<User>().Where(x => x.Id > 5).ToList();
/* OK */ List<User> list = engine.Select<User>().Where(x => x.Id > minId && x.Role == "admin").ToList();
/* OK */ List<User> list = engine.Select<User>().Where(x => x.Id > 5 && x.Username.Contains("qweqwe")).ToList();
/* OK */ List<User> list = engine.Select<User>().Where(x => x.Id > 5 && x.Username.Contains("qweqwe")).ToList();
/* OK */ List<User> list = engine.Select<User>().Where(x => x.Id > 5 && (x.Username.StartsWith("A") || x.Username.EndsWith("B"))).ToList();
List<User> list = engine.Select<User>().Where(x => selectedIds.Contains(x.Id)).ToList()
```

## Usage

Easy to use. You can do anything with one line :blush:

```csharp
Engine engine = new Engine(connectionString);

// SELECT
List<User> users = engine.Select<User>().ToList();

List<User> users = engine.Select<User>().Where(x => x.Id > 5 && x.Role == "admin" && x.CreatedDate < dateTime && x.Active == true).ToList();
List<User> users = engine.Select<User>().Where(x => x.Name.Contains("foo")).ToList();
List<User> users = engine.Select<User>().Where(x => x.Name.StartsWith("foo")).ToList();
List<User> users = engine.Select<User>().Where(x => x.Name.EndsWith("foo")).ToList();

// INSERT
engine.Insert(model);
int insertedId = model.Id; // You can get insrted id after insert

// UPDATE
int affectedRows = engine.Update(model);

// DELETE
int affectedRows = engine.Delete(model);

// ** TRANSACTIONS **
using (Transaction transaction = engine.BeginTransaction())
{
    try
    {
        engine.Insert(foo);
        engine.Update(bar);
        engine.Delete(baz);

        transaction.Commit();
    }
    catch (Exception ex)
    {
        transaction.Rollback();

        Log(ex);
    }
}
```

You can look at [tests](test/EngineTests.cs) to see real examples.

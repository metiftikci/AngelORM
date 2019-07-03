[![Build status](https://ci.appveyor.com/api/projects/status/4jmhhcdkrrk9qieh/branch/master?svg=true)](https://ci.appveyor.com/project/jaqra/angelorm/branch/master)

# Angel ORM

Basic and lightweight mssql operations framework.

## Roadmap

- [x] Add query creators for select, insert, update and delete operations.
- [x] Add parameter creator.
- [x] Run command for select, insert, update and delete queries.
- [x] Make DataTable to List<T> adapter.
- [x] Implement transaction.
- [ ] Add where feature to select query creator method.
- [ ] Add order by feature to select query creator method.
- [ ] Add Nullable<T> column type feature.
- [ ] Implement data annotations.

## Work On

Currently working on implement where feature with expression

```csharp
// ========== GOAL ==========

/** OK **/ List<User> list = engine.Select<User>().ToList();
/** OK **/ List<User> list = engine.Select<User>().Where(x => x.Id > 5).ToList();
/** OK **/ List<User> list = engine.Select<User>().Where(x => x.Id > minId && x.Role == "admin").ToList();
/** OK **/ List<User> list = engine.Select<User>().Where(x => x.Id > 5 && x.Username.Contains("qweqwe")).ToList();
/** OK **/ List<User> list = engine.Select<User>().Where(x => x.Id > 5 && x.Username.Contains("qweqwe")).ToList();
/** OK **/ List<User> list = engine.Select<User>().Where(x => x.Id > 5 && (x.Username.StartsWith("A") || x.Username.EndsWith("B"))).ToList();
List<User> list = engine.Select<User>().Where(x => x.Id > minId && x.Role == "admin").OrderBy(x => x.Id).OrderByDescendents(x => x.Name).ToList();
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
using (Transaction transaction = _engine.BeginTransaction())
{
    try
    {
        _engine.Insert(user);
        _engine.Insert(user2);
        _engine.Insert(user3);

        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
    }
}
```

You can look at [tests](test/EngineTests.cs) to see real examples.

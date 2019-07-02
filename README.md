# Angel ORM

Basic and lightweight mssql operations framework.

## Roadmap

- [x] Add query creators for select, insert, update and delete operations.
- [x] Add parameter creator.
- [x] Run command for select, insert, update and delete queries.
- [x] Make DataTable to List<T> adapter.
- [ ] Add where feature to select query creator method.
- [ ] Implement transaction.

## Work On

Currently working on implement where feature with expression

```csharp
// ========== GOAL ==========

/* OK */ List<User> list = engine.Select<User>().ToList();
/* OK */ List<User> list = engine.Select<User>().Where(x => x.Id > 5)
/* OK */ List<User> list = engine.Select<User>().Where(x => x.Id > minId && x.Role == "admin")
List<User> list = engine.Select<User>().Where(x => x.Id > 5 && x.Username.Contains("qweqwe")).ToList();
List<User> list = engine.Select<User>().Where(x => x.Id > 5 && x.Username.Contains("qweqwe")).ToList();
List<User> list = engine.Select<User>().Where(x => x.Id > 5 && (x.Username.StartsWith("A") || x.Username.EndsWith("B"))).ToList();
```

## Usage

Easy to use. You can do anything with one line :blush:

```csharp
Engine engine = new Engine(connectionString);

// SELECT
List<User> users = engine.ToList<User>();

// SELECT WITH CONDITIONS **[NEW]**
List<User> users = engine.ToList<User>(x => x.Id > 5 && x.Role == "admin" && x.CreatedDate < dateTime && x.Active == true);

// INSERT
engine.Insert(model);
int insertedId = model.Id; // You can get insrted id after insert

// UPDATE
int affectedRows = engine.Update(model);

// DELETE
int affectedRows = engine.Delete(model);
```

You can look at [tests](test/EngineTests.cs) to see real examples.

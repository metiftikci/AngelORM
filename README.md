[![Build status](https://ci.appveyor.com/api/projects/status/4jmhhcdkrrk9qieh/branch/master?svg=true)](https://ci.appveyor.com/project/jaqra/angelorm/branch/master)
![AppVeyor tests (compact)](https://img.shields.io/appveyor/tests/jaqra/AngelORM.svg?compact_message)
![Nuget](https://img.shields.io/nuget/v/AngelORM.svg)
![GitHub](https://img.shields.io/github/license/jaqra/AngelORM.svg)

![LOGO](https://raw.githubusercontent.com/jaqra/AngelORM/master/assets/logo.jpg)

# Angel ORM

Basic and lightweight mssql operations framework.

## Why Angel ORM instead of Entity Framework?

- Lightweight
- Less exception
- Customizable
- Working with **all** model classes (does not require migration or anything)

## Fetaures

- **Select**: Get data as model list.
- **Where**: Get data with expression conditions.
- **OrderBy**: Get ordered data (also multiple and descending combinations avaible).
- **Insert**: Insert new data to database.
- **Key**: Inserted key feedback to model.
- **Update**: Update row with auto detected key.
- **Delete**: Delete row with auto detected key.
- **Transaction**: Begin, Commit and Rollback transaction.
- **Raw Query**: Get raw query as string or execute raw query on database.
- **Nullable Column**: Supports for `Nullable<T>` type column (ex: `public DateTime? ModifiedDate { get; set; }`)
- **Enum Column**: Supports for `Enum` type column (ex: `public MyTypeStatus Status { get; set; }`)
- **Created Query Feedback**: You can use or manupilate created query with `OnQueryExecuting` method before execute.

## Roadmap

- Implement data annotations to define table and column names.
- Add CreateTable<T>, CreateTableIfNotExists<T> and MigrateTable<T> methods to Engine.
- Validate model fetaure from data annotations on Insert and Update.

## Usage

Easy to use. You can do anything with one line :blush:

### SELECT

```csharp
Engine engine = new Engine(connectionString);

List<User> users = engine.Select<User>().ToList();
```

### WHERE

```csharp
Engine engine = new Engine(connectionString);

List<User> users = engine.Select<User>().Where(x => x.Id > 5 && x.Role == "admin" && x.CreatedDate < dateTime && x.Active == true).ToList();
List<User> users = engine.Select<User>().Where(x => x.Name.Contains("foo")).ToList();
List<User> users = engine.Select<User>().Where(x => x.Name.StartsWith("foo")).ToList();
List<User> users = engine.Select<User>().Where(x => x.Name.EndsWith("foo")).ToList();

int[] refs = new int[] { 1, 2, 3, 4, 5 };

List<User> users = engine.Select<User>().Where(x => refs.Contains(x.Id)).ToList();
```

### OrderBy and OrderByDescending

```csharp
Engine engine = new Engine(connectionString);

List<User> users = engine.Select<User>().OrderBy(x => x.Name).ToList();
List<User> users = engine.Select<User>().OrderBy(x => x.Name).OrderByDescending(x => x.Surname).ToList();
```

### INSERT

You can get inserted id after call insert method.

```csharp
Engine engine = new Engine(connectionString);

engine.Insert(model);
int insertedId = model.Id;
```

### UPDATE

```csharp
Engine engine = new Engine(connectionString);

int affectedRows = engine.Update(model);
```

### DELETE

```csharp
Engine engine = new Engine(connectionString);

int affectedRows = engine.Delete(model);
```

### TRANSACTIONS

```csharp
Engine engine = new Engine(connectionString);

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

### Raw Query

```csharp
Engine engine = new Engine(connectionString);

string query = engine.Selet<User>().Where(x => x.Name == "foo").OrderBy(x => x.CreatedDate).ToSQL();

engine.ExecuteNonQuery(query);
```

### Created Query Feedback

```csharp
Engine engine = new Engine(connectionString);

List<User> users = engine
    .Select<User>()
    .OnQueryExecutin(x => x + " WHERE Id << 5 <> 0")
    .ToList();
```

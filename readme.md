# EntityFrameworkCore.Repository.Abstract

## What is it?

This library is meant for use with EntityFrameworkCore, on projects that support references to .NET Standard 1.5 or higher.

It's a wrapper for the [The Repository Pattern](https://msdn.microsoft.com/en-us/library/ff649690.aspx), in combination with EntityFramework(Core).

## What does it do?

Instead of having to write full repository functionality for each and every repository you create, using this library, and inheriting the exposed abstract class `AbstractRepository<TEntity, TKey>`, you will have automatically implemented the following methods into your repository:
* `Task<TEntity> GetById(TKey id)`
* `Task<TEntity> GetByKey(object[] keyValues)`
* `Task<ICollection<TEntity>> GetAll()`
* `Task<PagedResult<TEntity>> GetPaged(int page, int itemsPerPage)`
* `Task Add(TEntity entity)`
* `Task Remove(TEntity entity)`
* `Task Remove(params TEntity[] entity)`
* `Task Remove(ICollection<TEntity> entity)`
* `Task<int> TotalCount()`
* `Task SaveChanges()`

## Lazy-loading/includes

Because EntityFrameworkCore (at this time) does not support lazy-loading, the querying methods (with the exception of `GetByKey`) have an extra argument; `Func<IQueryable<TEntity>, IQueryable<TEntity>> queryExtra = null`.

This argument allows for adding extra filters, and includes in your queries, for example:

```
_usersRepository.GetById(
    10,
    users => users.Include(user => user.Friends).Include(user => user.Enemies)
);
```

This will add the `.Include` call into the query performed in the abstract repository.

It may also be used for further filtering, but it is still recommended to follow the repository pattern, and create a new method that more specifically returns the data you require.

## Dependency injection

This library was written with dependency injection in mind. The `AbstractRepository` type, which you'll be inheriting on your repositories, expects a `Microsoft.EntityFrameworkCore.DbContext`.  
If you've registered an object as `DbContext` in your DI, you should be able to pass it to the parent constructor as-is. The `AbstractRepository` will then attempt to find the set for `TEntity` by calling `DbContext.Set<TEntity>()`.

The `AbstractRepository` type also implements a generic interface `IRepository`, which can be used for [assembly-scanning registration](http://docs.autofac.org/en/latest/register/scanning.html) of all repositories in your project, based on if they implement `IRepository`.
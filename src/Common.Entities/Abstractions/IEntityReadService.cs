﻿using Regira.DAL.Paging;
using Regira.Entities.Models.Abstractions;

namespace Regira.Entities.Abstractions;

public interface IEntityReadService<TEntity> : IEntityReadService<TEntity, int>
{
}
public interface IEntityReadService<TEntity, in TKey>
{
    Task<TEntity?> Details(TKey id);
    Task<IList<TEntity>> List(object? so = default, PagingInfo? pagingInfo = null);
    Task<int> Count(object? so);
}
public interface IEntityReadService<TEntity, TSearchObject, TSortBy, TIncludes> : IEntityReadService<TEntity, int, TSearchObject, TSortBy, TIncludes>, IEntityReadService<TEntity>
    where TEntity : class, IEntity<int>
    where TSearchObject : ISearchObject<int>, new()
    where TSortBy : struct, Enum
    where TIncludes : struct, Enum
{
}
public interface IEntityReadService<TEntity, in TKey, TSearchObject, TSortBy, TIncludes> : IEntityReadService<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TSearchObject : ISearchObject<TKey>, new()
    where TSortBy : struct, Enum
    where TIncludes : struct, Enum
{
    Task<IList<TEntity>> List(IList<TSearchObject?> so, IList<TSortBy> sortBy, TIncludes? includes = null, PagingInfo? pagingInfo = null);
    Task<int> Count(IList<TSearchObject?> so);
}
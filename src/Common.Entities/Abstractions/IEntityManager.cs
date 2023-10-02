﻿using Regira.Entities.Models.Abstractions;

namespace Regira.Entities.Abstractions;

public interface IEntityManager<TEntity> : IEntityManager<TEntity, int>, IEntityService<TEntity>
    where TEntity : class, IEntity<int>
{
}
public interface IEntityManager<TEntity, in TKey> : IEntityService<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
}


public interface IEntityManager<TEntity, TSearchObject, TSortBy, TIncludes> : IEntityService<TEntity, TSearchObject, TSortBy, TIncludes>,
    IEntityManager<TEntity, int, TSearchObject, TSortBy, TIncludes>, IEntityManager<TEntity>
    where TEntity : class, IEntity<int>
    where TSearchObject : ISearchObject<int>, new()
    where TSortBy : struct, Enum
    where TIncludes : struct, Enum
{
}
public interface IEntityManager<TEntity, in TKey, TSearchObject, TSortBy, TIncludes> : IEntityService<TEntity, TKey, TSearchObject, TSortBy, TIncludes>,
    IEntityManager<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TSearchObject : ISearchObject<TKey>, new()
    where TSortBy : struct, Enum
    where TIncludes : struct, Enum
{
}
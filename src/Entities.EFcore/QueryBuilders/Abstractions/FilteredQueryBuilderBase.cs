﻿using Regira.Entities.Models.Abstractions;

namespace Regira.Entities.EFcore.QueryBuilders.Abstractions;

public abstract class FilteredQueryBuilderBase<TEntity, TSearchObject> : FilteredQueryBuilderBase<TEntity, int, TSearchObject>
    where TEntity : IEntity<int>
    where TSearchObject : ISearchObject<int>;
public abstract class FilteredQueryBuilderBase<TEntity, TKey, TSearchObject> : IFilteredQueryBuilder<TEntity, TKey, TSearchObject>
    where TEntity : IEntity<TKey>
    where TSearchObject : ISearchObject<TKey>
{
    public abstract IQueryable<TEntity> Build(IQueryable<TEntity> query, TSearchObject? so);
}
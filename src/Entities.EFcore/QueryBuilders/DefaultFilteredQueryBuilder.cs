﻿using Regira.Entities.EFcore.Extensions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models.Abstractions;

namespace Regira.Entities.EFcore.QueryBuilders;

public class DefaultFilteredQueryBuilder : DefaultFilteredQueryBuilder<int>;
public class DefaultFilteredQueryBuilder<TKey> : GlobalFilteredQueryBuilderBase<IEntity<TKey>, TKey>
{
    public override IQueryable<IEntity<TKey>> Build(IQueryable<IEntity<TKey>> query, ISearchObject<TKey>? so)
    {
        if (so != null)
        {
            query = query.FilterId(so.Id);
            query = query.FilterIds(so.Ids);
            query = query.FilterExclude(so.Exclude);
        }

        return query;
    }
}
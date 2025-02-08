﻿using Microsoft.EntityFrameworkCore;
using Regira.DAL.Paging;
using Regira.Entities.Abstractions;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Extensions;
using Regira.Entities.Models;
using Regira.Entities.Models.Abstractions;
using Regira.Utilities;

namespace Regira.Entities.EFcore.Services;

public class EntityRepository<TContext, TEntity>
    (TContext dbContext, IQueryBuilder<TEntity> queryBuilder)
    : EntityRepository<TContext, TEntity, int>(dbContext, queryBuilder), IEntityRepository<TEntity>
    where TContext : DbContext
    where TEntity : class, IEntity<int>;
public class EntityRepository<TContext, TEntity, TKey>
    (TContext dbContext, IQueryBuilder<TEntity, TKey> queryBuilder)
    : EntityRepository<TContext, TEntity, TKey, SearchObject<TKey>>(dbContext, queryBuilder)
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>;
public class EntityRepository<TContext, TEntity, TKey, TSearchObject>
    (TContext dbContext, IQueryBuilder<TEntity, TKey, TSearchObject> queryBuilder)
    : IEntityRepository<TEntity, TKey, TSearchObject>
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TSearchObject : class, ISearchObject<TKey>, new()
{
    public virtual DbSet<TEntity> DbSet => dbContext.Set<TEntity>();


    public virtual async Task<TEntity?> Details(TKey id)
        => id != null && id.Equals(default(TKey)) == false // make sure an id is passed or return null
            ? (await List(new TSearchObject { Id = id }, new PagingInfo { PageSize = 1 })).SingleOrDefault()
            : null;
    public virtual async Task<IList<TEntity>> List(TSearchObject? so = null, PagingInfo? pagingInfo = null)
    {
        var query = Query(DbSet, so, pagingInfo);
        return await query
#if NETSTANDARD2_0
            .AsNoTracking()
#else
            .AsNoTrackingWithIdentityResolution()
#endif
            .ToListAsync();
    }

    public virtual Task<int> Count(TSearchObject? so)
        => queryBuilder.Filter(DbSet, so).CountAsync();

    Task<IList<TEntity>> IEntityReadService<TEntity, TKey>.List(object? so, PagingInfo? pagingInfo)
        => List(Convert(so), pagingInfo);
    Task<int> IEntityReadService<TEntity, TKey>.Count(object? so)
        => Count(Convert(so));

    public virtual IQueryable<TEntity> Query(IQueryable<TEntity> query, TSearchObject? so, PagingInfo? pagingInfo = null)
        => queryBuilder.Query(query, so != null ? [so] : [], pagingInfo);

    public virtual Task Add(TEntity item)
    {
        PrepareItem(item);

        DbSet.Add(item);
        return Task.CompletedTask;
    }
    public virtual async Task Modify(TEntity item)
    {
        PrepareItem(item);

        var original = await Details(item.Id);
        if (original == null)
        {
            return;
        }

        dbContext.Attach(original);
        dbContext.Entry(original).CurrentValues.SetValues(item);
        dbContext.Entry(original).State = EntityState.Modified;

        Modify(item, original);
    }
    public virtual Task Save(TEntity item)
        => IsNew(item) ? Add(item) : Modify(item);
    public virtual Task Remove(TEntity item)
    {
        DbSet.Remove(item);
        return Task.CompletedTask;
    }

    public virtual Task<int> SaveChanges(CancellationToken token = default)
        => dbContext.SaveChangesAsync(token);

    public virtual void PrepareItem(TEntity item)
    {
    }
    public virtual void Modify(TEntity item, TEntity original)
    {
    }

    public virtual TSearchObject? Convert(object? so)
        => so != null
            ? so as TSearchObject ?? ObjectUtility.Create<TSearchObject>(so)
            : null;
    public bool IsNew(TEntity item)
        => item.IsNew();
}


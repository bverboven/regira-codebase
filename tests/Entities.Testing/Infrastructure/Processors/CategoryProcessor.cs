﻿using Entities.Testing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.Abstractions;
using Regira.Utilities;

namespace Entities.Testing.Infrastructure.Processors;

public class CategoryProcessor(ProductContext dbContext) : EntityProcessorBase<Category>
{
    public override async IAsyncEnumerable<Category> ProcessManyAsync(IEnumerable<Category> items)
    {
        var list = items.AsList();
        var categoryIds = list
            .Select(x => x.Id)
            .Distinct()
            .ToArray();
        var countPerCategory = await dbContext.Products
            .Where(p => categoryIds.Contains(p.CategoryId!.Value))
            .GroupBy(p => p.CategoryId!.Value)
            .ToDictionaryAsync(k => k.Key, v => v.Count());
        foreach (var item in list)
        {
            item.NumberOfProducts = countPerCategory[item.Id];
            yield return item;
        }
    }
}
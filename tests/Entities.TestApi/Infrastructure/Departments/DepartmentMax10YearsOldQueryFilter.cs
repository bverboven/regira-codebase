﻿using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Models;
using Testing.Library.Contoso;

namespace Entities.TestApi.Infrastructure.Departments;

public class DepartmentMax10YearsOldQueryFilter : FilteredQueryBuilderBase<Department, SearchObject>
{
    public override IQueryable<Department> Build(IQueryable<Department> query, SearchObject? so)
    {
        if (so != null)
        {
            query = query.Where(x => x.Created > DateTime.Today.AddYears(-10));
        }

        return query;
    }
}
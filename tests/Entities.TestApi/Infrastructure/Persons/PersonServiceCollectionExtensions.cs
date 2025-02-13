﻿using Microsoft.EntityFrameworkCore;
using Regira.Entities.DependencyInjection.Abstractions;
using Regira.Entities.DependencyInjection.Extensions;
using Testing.Library.Contoso;

namespace Entities.TestApi.Infrastructure.Persons;

public static class PersonServiceCollectionExtensions
{
    public static IEntityServiceCollection<TContext> AddPersons<TContext>(this IEntityServiceCollection<TContext> services)
        where TContext : DbContext
    {
        services
            .For<Person, PersonSearchObject, PersonSortBy, PersonIncludes>(e =>
            {
                e.UseEntityService<PersonManager>();
                e.HasRepository<PersonRepository>();
                e.HasManager<PersonManager>();
                e.AddQueryFilter<PersonQueryFilter>();
                e.UseQueryBuilder<PersonQueryBuilder>();
                e.AddMapping<PersonDto, PersonInputDto>();
                e.HasAttachments<TContext, Person, PersonAttachment>();
                e.AddNormalizer<PersonNormalizer>();
            });

        return services;
    }
}
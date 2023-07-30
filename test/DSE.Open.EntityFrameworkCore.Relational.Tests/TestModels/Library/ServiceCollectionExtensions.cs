// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.EntityFrameworkCore.Relational.Tests.TestModels.Library;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLibraryDbContext(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
    {
        _ = services.AddSingleton(TimeProvider.System);

        _ = services.AddScoped<UpdateTimesTrackedSaveChangesInterceptor>();

        var config = (IServiceProvider s, DbContextOptionsBuilder o) =>
        {
            optionsAction(s, o);

            _ = o.AddInterceptors(s.GetRequiredService<UpdateTimesTrackedSaveChangesInterceptor>());
        };

        _ = services.AddDbContext<LibraryDbContext>(config);

        return services;
    }
}

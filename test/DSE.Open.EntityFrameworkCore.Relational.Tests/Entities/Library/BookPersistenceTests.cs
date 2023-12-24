// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Relational.Tests.TestModels.Library;
using DSE.Open.Testing.Xunit.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.EntityFrameworkCore.Relational.Tests.Entities.Library;

public class BookPersistenceTests : SqliteEntityPersistenceTestsBase<LibraryDbContext, Book, int>
{
    public BookPersistenceTests(ITestOutputHelper output) : base(output)
    {
    }

    protected override void AddDbContext(IServiceCollection services)
    {
        services.AddLibraryDbContext(ConfigureDbContext);
    }

    protected override Task<Book> CreateEntityAsync(LibraryDbContext dataContext)
    {
        return Task.FromResult(new Book
        {
            Title = "The Hitchhiker's Guide to the Galaxy",
            Description = "It's an ordinary Thursday lunchtime for Arthur Dent until his house gets demolished."
        });
    }
}

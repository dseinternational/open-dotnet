// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Relational.Tests.TestModels.Library;
using DSE.Open.Testing.Xunit.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.EntityFrameworkCore.Relational.Tests.Entities.Library;

public class AuthorPersistenceTests : SqliteEntityPersistenceTestsBase<LibraryDbContext, Author, int>
{
    public AuthorPersistenceTests(ITestOutputHelper output) : base(output)
    {
    }

    protected override void AddDbContext(IServiceCollection services)
        => services.AddLibraryDbContext(ConfigureDbContext);

    protected override Task<Author> CreateEntityAsync(LibraryDbContext dataContext)
        => Task.FromResult(new Author { GivenName = "Fred", FamilyName = "Flintstone" });
}

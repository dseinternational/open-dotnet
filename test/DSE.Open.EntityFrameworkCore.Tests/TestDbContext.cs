// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using Microsoft.EntityFrameworkCore;

namespace DSE.Open.EntityFrameworkCore.Tests;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        _ = modelBuilder.Entity<Country>().ToTable("countries");

        _ = modelBuilder.Entity<Country>().HasKey(c => c.Code);

        _ = modelBuilder.Entity<Country>()
            .Property(c => c.Code)
            .HasConversion(new EntityFrameworkCore.Storage.ValueConversion.ValueTypeValueConverter<CountryCode, AsciiChar2, string>());
    }

    public DbSet<Country> Countries { get; set; } = null!;
}

public class Country
{
    public CountryCode Code { get; set; }
}

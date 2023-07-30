// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DSE.Open.EntityFrameworkCore.Relational.Tests.TestModels.Library;

public class AuthorEntityTypeConfiguration : UpdateTimesTrackedEventRaisingEntityTypeConfiguration<Author, int>
{
    public override void Configure(EntityTypeBuilder<Author> builder)
    {
        base.Configure(builder);

        _ = builder.ToTable("author", "library");
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DSE.Open.EntityFrameworkCore.Relational.Tests.TestModels.Library;

public class BookEntityTypeConfiguration : UpdateTimesTrackedEventRaisingEntityTypeConfiguration<Book, int>
{
    public override void Configure(EntityTypeBuilder<Book> builder)
    {
        base.Configure(builder);

        _ = builder.ToTable("book", "library");
    }
}

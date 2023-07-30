// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Abstractions;
using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> HasUpdateTimes<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity>(
        this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IUpdateTimesTracked
    {
        Guard.IsNotNull(builder);

        _ = builder.Property(e => e.Created);
        _ = builder.Property(e => e.Updated);

        return builder;
    }

    public static EntityTypeBuilder<TEntity> HasTimestamp<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        ValueConverter? valueConverter = null)
        where TEntity : class, ITimestamped
    {
        Guard.IsNotNull(builder);

        valueConverter ??= TimestampToByteArrayConverter.Default;

        _ = builder.Property(e => e.Timestamp).HasConversion(valueConverter);

        return builder;
    }
}

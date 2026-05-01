// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage;

/// <summary>
/// Provides extension methods for <see cref="EntityTypeBuilder{TEntity}"/>.
/// </summary>
public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Configures the <see cref="IUpdateTimesTracked.Created"/> and
    /// <see cref="IUpdateTimesTracked.Updated"/> properties on the entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="builder">The entity type builder.</param>
    /// <returns>The same <paramref name="builder"/> instance for chaining.</returns>
    public static EntityTypeBuilder<TEntity> HasUpdateTimes<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity>(
        this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IUpdateTimesTracked
    {
        ArgumentNullException.ThrowIfNull(builder);

        _ = builder.Property(e => e.Created);
        _ = builder.Property(e => e.Updated);

        return builder;
    }

    /// <summary>
    /// Configures the <see cref="IUpdateUsersTracked.CreatedUser"/> and
    /// <see cref="IUpdateUsersTracked.UpdatedUser"/> properties on the entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="builder">The entity type builder.</param>
    /// <returns>The same <paramref name="builder"/> instance for chaining.</returns>
    public static EntityTypeBuilder<TEntity> HasUpdateUsers<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity>(
        this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IUpdateUsersTracked
    {
        ArgumentNullException.ThrowIfNull(builder);

        _ = builder.Property(e => e.CreatedUser);
        _ = builder.Property(e => e.UpdatedUser);

        return builder;
    }

    /// <summary>
    /// Configures the <see cref="ITimestamped.Timestamp"/> property on the entity,
    /// applying the supplied <paramref name="valueConverter"/> or, if not provided,
    /// <see cref="TimestampToByteArrayConverter.Default"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="builder">The entity type builder.</param>
    /// <param name="valueConverter">An optional value converter for the timestamp property.</param>
    /// <returns>The same <paramref name="builder"/> instance for chaining.</returns>
    public static EntityTypeBuilder<TEntity> HasTimestamp<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        ValueConverter? valueConverter = null)
        where TEntity : class, ITimestamped
    {
        ArgumentNullException.ThrowIfNull(builder);

        valueConverter ??= TimestampToByteArrayConverter.Default;

        _ = builder.Property(e => e.Timestamp).HasConversion(valueConverter);

        return builder;
    }
}

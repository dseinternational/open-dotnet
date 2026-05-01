// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DSE.Open.EntityFrameworkCore.Storage;

/// <summary>
/// A base entity type configuration for entities that derive from
/// <see cref="UpdateUsersTrackedEntity{TId}"/>, configuring the id key, the
/// <c>Created</c>/<c>Updated</c> timestamp properties, the <c>CreatedUser</c>/
/// <c>UpdatedUser</c> audit identifiers, and a concurrency timestamp.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TId">The id type.</typeparam>
public abstract class UpdateUsersTrackedEntityTypeConfiguration<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity, TId>
    : EntityTypeConfiguration<TEntity, TId>
    where TEntity : UpdateUsersTrackedEntity<TId>
    where TId : struct, IEquatable<TId>
{
    /// <inheritdoc/>
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.Configure(builder);

        _ = builder.HasUpdateTimes();
        _ = builder.HasUpdateUsers();
        _ = builder.HasTimestamp();
    }
}

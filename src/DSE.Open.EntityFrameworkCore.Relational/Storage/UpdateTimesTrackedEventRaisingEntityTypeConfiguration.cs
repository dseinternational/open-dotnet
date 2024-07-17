// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DSE.Open.EntityFrameworkCore.Storage;

public abstract class UpdateTimesTrackedEventRaisingEntityTypeConfiguration<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity, TId>
    : EntityTypeConfiguration<TEntity, TId>
    where TEntity : UpdateTimesTrackedEventRaisingEntity<TId>
    where TId : struct, IEquatable<TId>
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.Configure(builder);

        _ = builder.HasUpdateTimes();
    }
}

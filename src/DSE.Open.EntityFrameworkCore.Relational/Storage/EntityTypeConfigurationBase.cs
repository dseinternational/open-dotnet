// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage;

public abstract class EntityTypeConfiguration<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity, TId>
    : EntityTypeConfigurationBase<TEntity>
    where TEntity : Entity<TId>
    where TId : struct, IEquatable<TId>
{
    public virtual bool HasManuallyAssignedId => false;

    public virtual string? IdColumnName => null;

    public virtual ValueConverter? IdValueConverter => null;

    public virtual bool ConfigureIdKey => true;

    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.Configure(builder);

        if (builder.Metadata.BaseType == null)
        {
            if (ConfigureIdKey)
            {
                _ = builder.HasKey(e => e.Id);

                var idPropertyBuilder = builder.Property(e => e.Id);

                _ = !HasManuallyAssignedId ? idPropertyBuilder.ValueGeneratedOnAdd() : idPropertyBuilder.ValueGeneratedNever();

                if (IdColumnName is not null)
                {
                    _ = idPropertyBuilder.HasColumnName(IdColumnName);
                }

                if (IdValueConverter is not null)
                {
                    _ = idPropertyBuilder.HasConversion(IdValueConverter);
                }
            }
            else
            {
                _ = builder.Ignore(e => e.Id);
            }
        }
    }
}

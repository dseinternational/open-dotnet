// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage;

/// <summary>
/// A base entity type configuration for entities that derive from <see cref="Entity{TId}"/>,
/// providing primary-key configuration with optional column name, value conversion, and
/// manual or generated id assignment.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TId">The id type.</typeparam>
public abstract class EntityTypeConfiguration<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity, TId>
    : EntityTypeConfigurationBase<TEntity>
    where TEntity : Entity<TId>
    where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// Gets a value indicating whether the id is assigned manually rather than generated
    /// by the store on add. Defaults to <see langword="false"/>.
    /// </summary>
    public virtual bool HasManuallyAssignedId => false;

    /// <summary>
    /// Gets an optional column name to use for the id property. When <see langword="null"/>,
    /// the default name is used.
    /// </summary>
    public virtual string? IdColumnName => null;

    /// <summary>
    /// Gets an optional <see cref="ValueConverter"/> applied to the id property.
    /// </summary>
    public virtual ValueConverter? IdValueConverter => null;

    /// <summary>
    /// Gets a value indicating whether the id property should be configured as the primary
    /// key. When <see langword="false"/>, the id is ignored. Defaults to <see langword="true"/>.
    /// </summary>
    public virtual bool ConfigureIdKey => true;

    /// <inheritdoc/>
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

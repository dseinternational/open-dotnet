// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DSE.Open.EntityFrameworkCore.Storage;

/// <summary>
/// Base class for <see cref="IEntityTypeConfiguration{TEntity}"/> implementations
/// that applies common defaults such as <see cref="PropertyAccessMode"/> for properties
/// and navigations.
/// </summary>
/// <typeparam name="TEntity">The entity type being configured.</typeparam>
public abstract class EntityTypeConfigurationBase<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Gets the <see cref="Microsoft.EntityFrameworkCore.PropertyAccessMode"/> applied to
    /// navigations of <typeparamref name="TEntity"/>. Defaults to
    /// <see cref="Microsoft.EntityFrameworkCore.PropertyAccessMode.Field"/>.
    /// </summary>
    public virtual PropertyAccessMode NavigationAccessMode => PropertyAccessMode.Field;

    /// <summary>
    /// Gets the <see cref="Microsoft.EntityFrameworkCore.PropertyAccessMode"/> applied to
    /// properties of <typeparamref name="TEntity"/>. Defaults to
    /// <see cref="Microsoft.EntityFrameworkCore.PropertyAccessMode.Field"/>.
    /// </summary>
    public virtual PropertyAccessMode PropertyAccessMode => PropertyAccessMode.Field;

    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity"/>, applying the
    /// configured <see cref="PropertyAccessMode"/> and <see cref="NavigationAccessMode"/>
    /// when this is the root of the inheritance hierarchy.
    /// </summary>
    /// <param name="builder">The builder for the entity type.</param>
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (builder.Metadata.BaseType == null)
        {
            builder.Metadata.SetPropertyAccessMode(PropertyAccessMode);
            builder.Metadata.SetNavigationAccessMode(NavigationAccessMode);
        }
    }
}

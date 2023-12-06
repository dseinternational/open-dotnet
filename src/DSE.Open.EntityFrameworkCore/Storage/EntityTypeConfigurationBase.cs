// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DSE.Open.EntityFrameworkCore.Storage;

public abstract class EntityTypeConfigurationBase<[DynamicallyAccessedMembers(TrimmingHelper.EntityDynamicallyAccessedMemberTypes)] TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public virtual PropertyAccessMode NavigationAccessMode => PropertyAccessMode.Field;

    public virtual PropertyAccessMode PropertyAccessMode => PropertyAccessMode.Field;

    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (builder.Metadata.BaseType == null)
        {
            builder.Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.SetNavigationAccessMode(PropertyAccessMode.Field);
        }
    }
}

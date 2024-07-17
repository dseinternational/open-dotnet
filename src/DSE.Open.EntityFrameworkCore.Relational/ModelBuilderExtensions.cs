// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DSE.Open.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static void ApplySnakeCaseConvention(this ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        foreach (var entity in builder.Model.GetEntityTypes())
        {
            // https://github.com/dotnet/efcore/issues/18006
            // https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.0/breaking-changes#totable-on-a-derived-type-throws-an-exception

            if (entity.BaseType == null)
            {
                entity.SetTableName(StringHelper.ToSnakeCase(entity.GetTableName()));
            }

            if (entity.IsMappedToJson())
            {
                entity.SetContainerColumnName(StringHelper.ToSnakeCase(entity.GetContainerColumnName()));
            }

            var tableId = StoreObjectIdentifier.Table(entity.GetTableName() ?? string.Empty, entity.GetSchema());

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(StringHelper.ToSnakeCase(property.GetColumnName(tableId)));
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(StringHelper.ToSnakeCase(key.GetName()));
            }

            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(StringHelper.ToSnakeCase(key.GetConstraintName()));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(StringHelper.ToSnakeCase(index.GetDatabaseName()));
            }
        }
    }
}

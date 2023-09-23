// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// 
/// </summary>
/// <remarks>When serializing collections, consider an appropriate value comparer -
/// for example, <see cref="CollectionSequenceEqualValueComparer{T}"/></remarks>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public class CollectionToJsonStringValueConverter<TModel> : ObjectToJsonStringValueConverter<ICollection<TModel>, TModel[]>
{
    public static new readonly CollectionToJsonStringValueConverter<TModel> Default = new();

    public CollectionToJsonStringValueConverter() : this(null)
    {
    }

    public CollectionToJsonStringValueConverter(JsonSerializerOptions? jsonSerializerOptions)
        : base(jsonSerializerOptions)
    {
    }
}

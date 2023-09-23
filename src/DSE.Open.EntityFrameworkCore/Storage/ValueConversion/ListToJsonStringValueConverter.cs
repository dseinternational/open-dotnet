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
/// for example, <see cref="ListSequenceEqualValueComparer{T}"/></remarks>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public class ListToJsonStringValueConverter<TModel> : ObjectToJsonStringValueConverter<IList<TModel>, TModel[]>
{
    public static new readonly ListToJsonStringValueConverter<TModel> Default = new();

    public ListToJsonStringValueConverter() : this(null)
    {
    }

    public ListToJsonStringValueConverter(JsonSerializerOptions? jsonSerializerOptions)
        : base(jsonSerializerOptions)
    {
    }
}

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
/// for example, <see cref="SetEqualsValueComparer{T}"/></remarks>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public class SetToJsonStringValueConverter<TModel> : ObjectToJsonStringValueConverter<ISet<TModel>, HashSet<TModel>>
{
    public static new readonly SetToJsonStringValueConverter<TModel> Default = new();

    public SetToJsonStringValueConverter() : this(null)
    {
    }

    public SetToJsonStringValueConverter(JsonSerializerOptions? jsonSerializerOptions)
        : base(jsonSerializerOptions)
    {
    }
}

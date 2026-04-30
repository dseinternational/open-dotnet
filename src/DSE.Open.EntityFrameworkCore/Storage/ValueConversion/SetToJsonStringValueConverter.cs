// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="ISet{T}"/> to and from a JSON <see cref="string"/> for storage,
/// using <see cref="HashSet{T}"/> as the materialized backing type.
/// </summary>
/// <typeparam name="TModel">The element type of the set.</typeparam>
/// <remarks>When serializing collections, consider an appropriate value comparer -
/// for example, <see cref="SetEqualsValueComparer{T}"/></remarks>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public class SetToJsonStringValueConverter<TModel> : ObjectToJsonStringValueConverter<ISet<TModel>, HashSet<TModel>>
{
    /// <summary>
    /// A shared default instance using the default JSON serializer options.
    /// </summary>
    public static new readonly SetToJsonStringValueConverter<TModel> Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="SetToJsonStringValueConverter{TModel}"/>
    /// using the default serializer options.
    /// </summary>
    public SetToJsonStringValueConverter() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SetToJsonStringValueConverter{TModel}"/>
    /// using the specified serializer options.
    /// </summary>
    /// <param name="jsonSerializerOptions">The serializer options to use, or <see langword="null"/> for the defaults.</param>
    public SetToJsonStringValueConverter(JsonSerializerOptions? jsonSerializerOptions)
        : base(jsonSerializerOptions)
    {
    }
}

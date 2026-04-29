// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="IList{T}"/> to and from a JSON <see cref="string"/> for storage,
/// using <see cref="List{T}"/> as the materialized backing type.
/// </summary>
/// <typeparam name="TModel">The element type of the list.</typeparam>
/// <remarks>When serializing collections, consider an appropriate value comparer -
/// for example, <see cref="ListSequenceEqualValueComparer{T}"/></remarks>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public class ListToJsonStringValueConverter<TModel> : ObjectToJsonStringValueConverter<IList<TModel>, List<TModel>>
{
    /// <summary>
    /// A shared default instance using the default JSON serializer options.
    /// </summary>
    public static new readonly ListToJsonStringValueConverter<TModel> Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="ListToJsonStringValueConverter{TModel}"/>
    /// using the default serializer options.
    /// </summary>
    public ListToJsonStringValueConverter() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ListToJsonStringValueConverter{TModel}"/>
    /// using the specified serializer options.
    /// </summary>
    /// <param name="jsonSerializerOptions">The serializer options to use, or <see langword="null"/> for the defaults.</param>
    public ListToJsonStringValueConverter(JsonSerializerOptions? jsonSerializerOptions)
        : base(jsonSerializerOptions)
    {
    }
}

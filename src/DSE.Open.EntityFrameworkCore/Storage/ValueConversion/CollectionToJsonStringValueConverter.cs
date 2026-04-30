// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.EntityFrameworkCore.ChangeTracking;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="ICollection{T}"/> to and from a JSON <see cref="string"/> for storage,
/// using <see cref="List{T}"/> as the materialized backing type.
/// </summary>
/// <typeparam name="TModel">The element type of the collection.</typeparam>
/// <remarks>When serializing collections, consider an appropriate value comparer -
/// for example, <see cref="CollectionSequenceEqualValueComparer{T}"/></remarks>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public class CollectionToJsonStringValueConverter<TModel> : ObjectToJsonStringValueConverter<ICollection<TModel>, List<TModel>>
{
    /// <summary>
    /// A shared default instance using the default JSON serializer options.
    /// </summary>
    public static new readonly CollectionToJsonStringValueConverter<TModel> Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="CollectionToJsonStringValueConverter{TModel}"/>
    /// using the default serializer options.
    /// </summary>
    public CollectionToJsonStringValueConverter() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CollectionToJsonStringValueConverter{TModel}"/>
    /// using the specified serializer options.
    /// </summary>
    /// <param name="jsonSerializerOptions">The serializer options to use, or <see langword="null"/> for the defaults.</param>
    public CollectionToJsonStringValueConverter(JsonSerializerOptions? jsonSerializerOptions)
        : base(jsonSerializerOptions)
    {
    }
}

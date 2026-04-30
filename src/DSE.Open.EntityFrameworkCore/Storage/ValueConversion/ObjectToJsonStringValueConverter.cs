// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an object of type <typeparamref name="TModel"/> to and from a JSON-encoded
/// <see cref="string"/> for storage, deserializing into a concrete <typeparamref name="TSerialized"/>.
/// </summary>
/// <typeparam name="TModel">The model type exposed to consumers.</typeparam>
/// <typeparam name="TSerialized">The concrete type used when deserializing JSON. Must be assignable to <typeparamref name="TModel"/>.</typeparam>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public class ObjectToJsonStringValueConverter<TModel, TSerialized> : ValueConverter<TModel, string>
    where TSerialized : TModel
{
    /// <summary>
    /// A shared default instance using the default JSON serializer options.
    /// </summary>
    public static readonly ObjectToJsonStringValueConverter<TModel, TSerialized> Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="ObjectToJsonStringValueConverter{TModel, TSerialized}"/>
    /// using the default serializer options.
    /// </summary>
    public ObjectToJsonStringValueConverter() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ObjectToJsonStringValueConverter{TModel, TSerialized}"/>
    /// using the specified serializer options.
    /// </summary>
    /// <param name="jsonSerializerOptions">The serializer options to use, or <see langword="null"/> for the defaults.</param>
    public ObjectToJsonStringValueConverter(JsonSerializerOptions? jsonSerializerOptions)
        : base(v => ConvertTo(v, jsonSerializerOptions), v => ConvertFrom(v, jsonSerializerOptions))
    {
    }

    /// <summary>
    /// Serializes <paramref name="model"/> to a JSON string.
    /// </summary>
    /// <param name="model">The model to serialize. Must not be <see langword="null"/>.</param>
    /// <param name="jsonSerializerOptions">The serializer options to use, or <see langword="null"/> for the defaults.</param>
    /// <returns>The JSON-encoded string.</returns>
    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static string ConvertTo(TModel model, JsonSerializerOptions? jsonSerializerOptions)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        ArgumentNullException.ThrowIfNull(model);

        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;

        return JsonSerializer.Serialize(model, jsonSerializerOptions);
    }

    /// <summary>
    /// Deserializes a JSON-encoded storage string back to a <typeparamref name="TModel"/>
    /// via the concrete <typeparamref name="TSerialized"/>.
    /// </summary>
    /// <param name="providerValue">The JSON-encoded string to deserialize. Must not be <see langword="null"/>.</param>
    /// <param name="jsonSerializerOptions">The serializer options to use, or <see langword="null"/> for the defaults.</param>
    /// <returns>The deserialized model.</returns>
    // keep public for EF Core compiled models
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static TModel ConvertFrom(string providerValue,
#pragma warning restore CA1000 // Do not declare static members on generic types
                                     JsonSerializerOptions? jsonSerializerOptions)
    {
        ArgumentNullException.ThrowIfNull(providerValue);

        jsonSerializerOptions ??= JsonSharedOptions.RelaxedJsonEscaping;

        return JsonSerializer.Deserialize<TSerialized>(providerValue, jsonSerializerOptions)!;
    }
}

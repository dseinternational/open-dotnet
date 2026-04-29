// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

#pragma warning disable CA1000 // Do not declare static members on generic types

/// <summary>
/// Converts a <see cref="ReadOnlyMemory{T}"/> to and from a JSON-encoded <see cref="string"/> for storage.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public sealed class ReadOnlyMemoryToJsonArrayConverter<T> : ValueConverter<ReadOnlyMemory<T>, string>
{
    /// <summary>
    /// Gets a shared default instance.
    /// </summary>
    public static ReadOnlyMemoryToJsonArrayConverter<T> Default { get; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="ReadOnlyMemoryToJsonArrayConverter{T}"/>.
    /// </summary>
    public ReadOnlyMemoryToJsonArrayConverter()
        : base(v => ConvertToArray(v), v => ConvertToReadOnlyMemory(v), null)
    {
    }

    /// <summary>
    /// Serializes the supplied <see cref="ReadOnlyMemory{T}"/> to its JSON array storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The JSON-encoded string.</returns>
    public static string ConvertToArray(ReadOnlyMemory<T> value)
    {
        return JsonSerializer.Serialize(value, JsonSharedOptions.RelaxedJsonEscaping);
    }

    /// <summary>
    /// Deserializes a JSON array storage value back to a <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    /// <param name="value">The JSON-encoded string.</param>
    /// <returns>The reconstructed <see cref="ReadOnlyMemory{T}"/>.</returns>
    public static ReadOnlyMemory<T> ConvertToReadOnlyMemory(string value)
    {
        return JsonSerializer.Deserialize<T[]>(value, JsonSharedOptions.RelaxedJsonEscaping);
    }
}

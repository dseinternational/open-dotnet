// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

#pragma warning disable CA1000 // Do not declare static members on generic types

/// <summary>
/// Converts a <see cref="Memory{T}"/> to and from a JSON-encoded <see cref="string"/> for storage.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public sealed class MemoryToJsonArrayConverter<T> : ValueConverter<Memory<T>, string>
{
    /// <summary>
    /// Gets a shared default instance.
    /// </summary>
    public static MemoryToJsonArrayConverter<T> Default { get; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="MemoryToJsonArrayConverter{T}"/>.
    /// </summary>
    public MemoryToJsonArrayConverter()
        : base(v => ConvertToArray(v), v => ConvertToMemory(v), null)
    {
    }

    /// <summary>
    /// Serializes the supplied <see cref="Memory{T}"/> to its JSON array storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The JSON-encoded string.</returns>
    public static string ConvertToArray(Memory<T> value)
    {
        return JsonSerializer.Serialize(value, JsonSharedOptions.RelaxedJsonEscaping);
    }

    /// <summary>
    /// Deserializes a JSON array storage value back to a <see cref="Memory{T}"/>.
    /// </summary>
    /// <param name="value">The JSON-encoded string.</param>
    /// <returns>The reconstructed <see cref="Memory{T}"/>.</returns>
    public static Memory<T> ConvertToMemory(string value)
    {
        return JsonSerializer.Deserialize<T[]>(value, JsonSharedOptions.RelaxedJsonEscaping);
    }
}

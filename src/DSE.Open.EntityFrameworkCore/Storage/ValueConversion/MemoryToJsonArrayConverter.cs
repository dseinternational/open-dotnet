// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using DSE.Open.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

#pragma warning disable CA1000 // Do not declare static members on generic types

[RequiresDynamicCode(WarningMessages.RequiresDynamicCode)]
[RequiresUnreferencedCode(WarningMessages.RequiresUnreferencedCode)]
public sealed class MemoryToJsonArrayConverter<T> : ValueConverter<Memory<T>, string>
{
    public static MemoryToJsonArrayConverter<T> Default { get; } = new();

    public MemoryToJsonArrayConverter()
        : base(v => ConvertToArray(v), v => ConvertToMemory(v), null)
    {
    }

    public static string ConvertToArray(Memory<T> value)
    {
        return JsonSerializer.Serialize(value, JsonSharedOptions.RelaxedJsonEscaping);
    }

    public static Memory<T> ConvertToMemory(string value)
    {
        return JsonSerializer.Deserialize<T[]>(value, JsonSharedOptions.RelaxedJsonEscaping);
    }
}

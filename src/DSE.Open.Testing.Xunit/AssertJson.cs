// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit;

namespace DSE.Open.Testing.Xunit;

public static class AssertJson
{
    [RequiresDynamicCode(
        "JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System. Text. Json source generation for native AOT applications.")]
    [RequiresUnreferencedCode(
        "JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static void Roundtrip<T>(T value)
    {
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<T>(json);
        Assert.Equivalent(value, deserialized);
    }
}

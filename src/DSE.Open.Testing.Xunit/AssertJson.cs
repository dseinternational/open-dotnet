// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Xunit;

namespace DSE.Open.Testing.Xunit;

public static class AssertJson
{
    [RequiresDynamicCode(
        "JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
    [RequiresUnreferencedCode(
        "JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static void Roundtrip<T>(T value, JsonSerializerOptions? options = null)
    {
        var json = JsonSerializer.Serialize(value, options);
        var deserialized = JsonSerializer.Deserialize<T>(json, options);
        Assert.Equivalent(value, deserialized);
    }

    /// <summary>
    /// Roundtrip a value of type <typeparamref name="T"/> using the provided <see cref="JsonTypeInfo{T}"/> instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="typeInfo"></param>
    public static void Roundtrip<T>(T value, JsonTypeInfo<T> typeInfo)
    {
        var json = JsonSerializer.Serialize(value, typeInfo);
        var deserialized = JsonSerializer.Deserialize(json, typeInfo);
        Assert.Equivalent(value, deserialized);
    }

    public static void Roundtrip<T>(T value, JsonSerializerContext context)
    {
        var json = JsonSerializer.Serialize(value, typeof(T), context);
        var deserialized = JsonSerializer.Deserialize(json, typeof(T), context);
        Assert.Equivalent(value, deserialized);
    }
}

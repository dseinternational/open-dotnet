// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Xunit;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// Helpers that verify a type round-trips faithfully through
/// <see cref="JsonSerializer"/>.
/// </summary>
public static class AssertJson
{
    /// <summary>
    /// Serialises <paramref name="value"/> with <paramref name="options"/>, deserialises
    /// the result, and asserts equivalence with the original.
    /// </summary>
    /// <remarks>
    /// Uses reflection-based serialisation. For AOT-compatible code, use the overload
    /// that accepts a <see cref="JsonTypeInfo{T}"/> or <see cref="JsonSerializerContext"/>.
    /// </remarks>
    [RequiresDynamicCode(
        "JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
    [RequiresUnreferencedCode(
        "JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static void Roundtrip<T>(T value, JsonSerializerOptions? options = null)
    {
        var json = JsonSerializer.Serialize(value, options);
        var deserialized = JsonSerializer.Deserialize<T>(json, options);
        Assert.Equivalent(value, deserialized, true);
    }

    /// <summary>
    /// Serialises <paramref name="value"/> using the supplied <see cref="JsonTypeInfo{T}"/>,
    /// deserialises the result, and asserts equivalence with the original. Safe for AOT.
    /// </summary>
    public static void Roundtrip<T>(T value, JsonTypeInfo<T> typeInfo)
    {
        var json = JsonSerializer.Serialize(value, typeInfo);
        var deserialized = JsonSerializer.Deserialize(json, typeInfo);
        Assert.Equivalent(value, deserialized, true);
    }

    /// <summary>
    /// Serialises <paramref name="value"/> using the type metadata supplied by
    /// <paramref name="context"/>, deserialises the result, and asserts equivalence with
    /// the original. Safe for AOT.
    /// </summary>
    public static void Roundtrip<T>(T value, JsonSerializerContext context)
    {
        var json = JsonSerializer.Serialize(value, typeof(T), context);
        var deserialized = JsonSerializer.Deserialize(json, typeof(T), context);
        Assert.Equivalent(value, deserialized, true);
    }
}

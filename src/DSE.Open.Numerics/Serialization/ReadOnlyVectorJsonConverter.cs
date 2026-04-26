// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// JSON converter for <see cref="ReadOnlyVector"/> and any concrete
/// <see cref="ReadOnlyVector{T}"/>. Delegates to <see cref="VectorJsonConverter"/>
/// for the actual read/write and snapshots the result as read-only.
/// </summary>
public sealed class ReadOnlyVectorJsonConverter : JsonConverter<ReadOnlyVector>
{
    /// <summary>The default instance, configured with <see cref="VectorJsonFormat"/>'s default value.</summary>
    public static ReadOnlyVectorJsonConverter Default { get; } = new();

    private readonly VectorJsonConverter _inner;

    /// <summary>Creates a converter using the default <see cref="VectorJsonFormat"/>.</summary>
    public ReadOnlyVectorJsonConverter() : this(default)
    {
    }

    /// <summary>Creates a converter using the supplied <paramref name="format"/>.</summary>
    public ReadOnlyVectorJsonConverter(VectorJsonFormat format = default)
    {
        _inner = new VectorJsonConverter(format);
    }

    /// <summary>Gets the format used to read/write vectors.</summary>
    public VectorJsonFormat Format => _inner.Format;

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(ReadOnlyVector));
    }

    /// <inheritdoc />
    public override ReadOnlyVector Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert is not null);

        var vector = _inner.Read(ref reader, typeToConvert, options);

        if (vector is null)
        {
            throw new JsonException("Failed to deserialize vector");
        }

        return vector.AsReadOnly();
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ReadOnlyVector value, JsonSerializerOptions options)
    {
        VectorJsonWriter.WriteVector(writer, value, options);
    }
}

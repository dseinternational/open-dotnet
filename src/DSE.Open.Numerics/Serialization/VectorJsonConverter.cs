// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// JSON converter for <see cref="Vector"/> and any concrete <see cref="Vector{T}"/>.
/// Delegates the read/write to <see cref="VectorJsonReader"/> and
/// <see cref="VectorJsonWriter"/>.
/// </summary>
public class VectorJsonConverter : JsonConverter<Vector>
{
    /// <summary>The default instance, configured with <see cref="VectorJsonFormat"/>'s default value.</summary>
    public static VectorJsonConverter Default { get; } = new();

    /// <summary>Creates a converter using the default <see cref="VectorJsonFormat"/>.</summary>
    public VectorJsonConverter() : this(default)
    {
    }

    /// <summary>Creates a converter using the supplied <paramref name="format"/>.</summary>
    public VectorJsonConverter(VectorJsonFormat format = default)
    {
        Format = format;
    }

    /// <summary>Gets the format used to read/write vectors.</summary>
    public VectorJsonFormat Format { get; }

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(Vector));
    }

    /// <inheritdoc />
    public override Vector? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return VectorJsonReader.ReadVector(ref reader, Format);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Vector value, JsonSerializerOptions options)
    {
        VectorJsonWriter.WriteVector(writer, value, options);
    }
}

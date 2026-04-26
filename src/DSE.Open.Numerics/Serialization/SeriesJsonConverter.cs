// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// JSON converter for <see cref="Series"/> and any concrete <see cref="Series{T}"/>.
/// Delegates to <see cref="SeriesJsonReader"/> and <see cref="SeriesJsonWriter"/>.
/// </summary>
public class SeriesJsonConverter : JsonConverter<Series>
{
    /// <summary>The default instance, configured with <see cref="VectorJsonFormat"/>'s default value.</summary>
    public static SeriesJsonConverter Default { get; } = new();

    /// <summary>Creates a converter using the default <see cref="VectorJsonFormat"/>.</summary>
    public SeriesJsonConverter() : this(default)
    {
    }

    /// <summary>Creates a converter using the supplied <paramref name="format"/>.</summary>
    public SeriesJsonConverter(VectorJsonFormat format = default)
    {
        Format = format;
    }

    /// <summary>Gets the format used to read/write series.</summary>
    public VectorJsonFormat Format { get; }

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(Series));
    }

    /// <inheritdoc />
    public override Series? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return SeriesJsonReader.Read(ref reader, options);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Series value, JsonSerializerOptions options)
    {
        SeriesJsonWriter.Write(writer, value, options);
    }
}

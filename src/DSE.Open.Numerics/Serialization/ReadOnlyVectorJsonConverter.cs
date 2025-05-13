// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Serialization;

public sealed class ReadOnlyVectorJsonConverter : JsonConverter<IReadOnlyVector>
{
    public static ReadOnlyVectorJsonConverter Default { get; } = new();

    private readonly VectorJsonConverter _inner;

    public ReadOnlyVectorJsonConverter() : this(default)
    {
    }

    public ReadOnlyVectorJsonConverter(VectorJsonFormat format = default)
    {
        _inner = new VectorJsonConverter(format);
    }

    public VectorJsonFormat Format => _inner.Format;

    public override bool CanConvert(Type typeToConvert)
    {
        Debug.Assert(typeToConvert is not null);
        return typeToConvert.IsAssignableTo(typeof(IReadOnlyVector));
    }

    public override IReadOnlyVector Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert is not null);

        var vector = _inner.Read(ref reader, typeToConvert, options);

        if (vector is null)
        {
            throw new JsonException("Failed to deserialize vector");
        }

        return vector.AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyVector value, JsonSerializerOptions options)
    {
        _inner.Write(writer, value, options);
    }
}

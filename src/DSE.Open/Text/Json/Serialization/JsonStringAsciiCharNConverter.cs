// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

public sealed class JsonStringAsciiCharNConverter<T> : JsonConverter<T> where T : struct, IUtf8SpanSerializable<T>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(T.MaxSerializedByteLength <= 8);

        var span = reader.HasValueSequence
            ? reader.ValueSequence.ToArray()
            : reader.ValueSpan;

        if (T.TryParse(span, null, out var value))
        {
            return value;
        }

        ThrowHelper.ThrowFormatException($"The JSON value is not a valid {typeof(T).Name}.");
        return default; // unreachable
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        Debug.Assert(T.MaxSerializedByteLength <= 8);

        Span<byte> buffer = stackalloc byte[T.MaxSerializedByteLength];

        if (!value.TryFormat(buffer, out var bytesWritten, null, null))
        {
            ThrowHelper.ThrowFormatException($"The value is not a valid {typeof(T).Name}.");
        }

        Debug.Assert(bytesWritten == T.MaxSerializedByteLength);

        writer.WriteStringValue(buffer);
    }
}

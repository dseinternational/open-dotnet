// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Values.Text.Json.Serialization;

public sealed class JsonUtf8SpanSerializableValueConverter<TValue, T> : JsonConverter<TValue>
    where T : IEquatable<T>, IUtf8SpanParsable<T>, IUtf8SpanFormattable
    where TValue : struct, IValue<TValue, T>, IUtf8SpanSerializable<TValue>
{
    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var bytes = reader.HasValueSequence
            ? reader.ValueSequence.ToArray()
            : reader.ValueSpan;

        if (TValue.TryParse(bytes, default, out var value))
        {
            return value;
        }

        ThrowHelper.ThrowFormatException(
            $"Could not convert {typeof(TValue).Name} value: {Encoding.UTF8.GetString(bytes)}");
        return default; // unreachable
    }

    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        Guard.IsNotNull(writer);

        var rented = SpanOwner<char>.Empty;

        var length = TValue.MaxSerializedCharLength;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(length)
            ? stackalloc char[length]
            : (rented = SpanOwner<char>.Allocate(length)).Span;

        using (rented)
        {
            if (value.TryFormat(buffer, out var bytesWritten, default, default))
            {
                writer.WriteStringValue(buffer[..bytesWritten]);
            }
            else
            {
                ThrowHelper.ThrowFormatException(
                    $"Could not convert {typeof(TValue).Name} value: {value}");
            }
        }
    }
}

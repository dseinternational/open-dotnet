// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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
        var valueLength = reader.HasValueSequence
            ? checked((int)reader.ValueSequence.Length)
            : reader.ValueSpan.Length;

        if (valueLength > TValue.MaxSerializedByteLength)
        {
            throw new FormatException($"Could not convert {typeof(TValue).Name} value: value was too long.");
        }

        var length = TValue.MaxSerializedByteLength;
        var rented = SpanOwner<byte>.Empty;

        // Stack allocate the constant `TValue.MaxSerializedByteLength` if we can, otherwise get an array of size >=
        // `valueLength` from the `ArrayPool` (via `SpanOwner`).
        Span<byte> buffer = MemoryThresholds.CanStackalloc<byte>(length)
            ? stackalloc byte[length]
            : (rented = SpanOwner<byte>.Allocate(length)).Span;

        using (rented)
        {
            var chars = reader.CopyString(buffer);

            var success = TValue.TryParse(buffer[..chars], default, out var value);

            return success switch
            {
                true => value,
                _ => throw new FormatException($"Could not convert {typeof(TValue).Name} value")
            };
        }
    }

    public override TValue ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Read(ref reader, typeToConvert, options);
    }

    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        Write(writer, value, options, false);
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] TValue value, JsonSerializerOptions options)
    {
        Write(writer, value, options, true);
    }

    [SkipLocalsInit]
    private void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options, bool asPropertyName = false)
    {
        ArgumentNullException.ThrowIfNull(writer);

        var rented = SpanOwner<char>.Empty;

        var length = TValue.MaxSerializedCharLength;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(length)
            ? stackalloc char[length]
            : (rented = SpanOwner<char>.Allocate(length)).Span;

        using (rented)
        {
            if (value.TryFormat(buffer, out var bytesWritten, default, default))
            {
                if (asPropertyName)
                {
                    writer.WritePropertyName(buffer[..bytesWritten]);
                }
                else
                {
                    writer.WriteStringValue(buffer[..bytesWritten]);
                }
            }
            else
            {
                ThrowHelper.ThrowFormatException(
                    $"Could not convert {typeof(TValue).Name} value: {value}");
            }
        }
    }
}

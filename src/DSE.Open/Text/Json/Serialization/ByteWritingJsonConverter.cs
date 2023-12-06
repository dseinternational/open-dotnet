// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json.Serialization;

/// <summary>
/// Base implementation for a <see cref="JsonConverter"/> that reads and writes values from <see cref="byte"/> buffers.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public abstract class ByteWritingJsonConverter<TValue> : JsonConverter<TValue>
    where TValue : IUtf8SpanFormattable, IUtf8SpanParsable<TValue>
{
    protected abstract int GetMaxByteCountToWrite(TValue value);

    protected abstract bool TryParse(ReadOnlySpan<byte> data, [MaybeNullWhen(false)] out TValue value);

    protected abstract bool TryFormat(TValue value, Span<byte> data, out int bytesWritten);

    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var bytes = reader.HasValueSequence
            ? reader.ValueSequence.ToArray()
            : reader.ValueSpan;

        if (TryParse(bytes, out var value))
        {
            return value;
        }

        ThrowHelper.ThrowFormatException($"Could not convert {typeof(TValue).Name} value: {bytes.ToArray()}");
        return default;
    }

    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        var byteCount = GetMaxByteCountToWrite(value);

        if (byteCount < 1)
        {
            writer.WriteStringValue(string.Empty);
            return;
        }

        byte[]? rented = null;

        Span<byte> output = byteCount <= StackallocThresholds.MaxByteLength
            ? stackalloc byte[byteCount]
            : (rented = ArrayPool<byte>.Shared.Rent(byteCount));

        try
        {
            if (TryFormat(value, output, out var bytesWritten))
            {
                writer.WriteStringValue(output[..bytesWritten]);
            }
            else
            {
                ThrowHelper.ThrowFormatException($"Could not convert {typeof(TValue).Name} value: {value}");
            }
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<byte>.Shared.Return(rented, clearArray: true);
            }
        }
    }
}

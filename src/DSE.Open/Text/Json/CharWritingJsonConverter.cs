// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Text.Json;

/// <summary>
/// Base implementation for a <see cref="JsonConverter"/> that reads and writes values
/// from <see cref="char"/> buffers.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public abstract class CharWritingJsonConverter<TValue> : JsonConverter<TValue>
{
    protected abstract int GetMaxCharCountToWrite(TValue value);

    protected abstract bool TryParse(ReadOnlySpan<char> data, [MaybeNullWhen(false)] out TValue value);

    protected abstract bool TryFormat(TValue value, Span<char> data, out int charsWritten);

    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valueLength = reader.HasValueSequence
            ? checked((int)reader.ValueSequence.Length)
            : reader.ValueSpan.Length;

        char[]? rented = null;

        Span<char> buffer = valueLength <= StackallocThresholds.MaxCharLength
            ? stackalloc char[valueLength]
            : (rented = ArrayPool<char>.Shared.Rent(valueLength));

        try
        {
            var chars = reader.CopyString(buffer); // use CopyString to unescape

            if (TryParse(buffer[..chars], out var value))
            {
                return value;
            }

            ThrowHelper.ThrowFormatException($"Could not convert {typeof(TValue).Name} value: {buffer}");
            return default;
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented, clearArray: true);
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        Guard.IsNotNull(writer);

        var charCount = GetMaxCharCountToWrite(value);

        if (charCount < 1)
        {
            writer.WriteStringValue(string.Empty);
            return;
        }

        char[]? rented = null;

        Span<char> output = charCount <= StackallocThresholds.MaxCharLength
            ? stackalloc char[charCount]
            : (rented = ArrayPool<char>.Shared.Rent(charCount));

        try
        {
            if (TryFormat(value, output, out var charsWritten))
            {
                writer.WriteStringValue(output[..charsWritten]);
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
                ArrayPool<char>.Shared.Return(rented, clearArray: true);
            }
        }
    }
}

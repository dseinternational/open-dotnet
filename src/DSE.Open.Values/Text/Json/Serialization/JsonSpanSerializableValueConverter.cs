// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DSE.Open.Values.Text.Json.Serialization;

public sealed class JsonSpanSerializableValueConverter<TValue, T> : JsonConverter<TValue>
    where T : IEquatable<T>, ISpanParsable<T>, ISpanFormattable
    where TValue : struct, IValue<TValue, T>, ISpanSerializable<TValue>
{
    public static readonly JsonSpanSerializableValueConverter<TValue, T> Default = new();

    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valueLength = reader.HasValueSequence
           ? checked((int)reader.ValueSequence.Length)
           : reader.ValueSpan.Length;

        char[]? rented = null;

        Span<char> buffer = valueLength <= JsonConstants.StackallocCharThreshold
            ? stackalloc char[valueLength]
            : (rented = ArrayPool<char>.Shared.Rent(valueLength));

        try
        {
            var chars = reader.CopyString(buffer);

            var success = TValue.TryParse(buffer[..chars], default, out var value);

            return success ? value : throw new FormatException($"Could not convert {typeof(TValue).Name} value: {buffer}");
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

        char[]? rented = null;

        Span<char> buffer = TValue.MaxSerializedCharLength <= JsonConstants.StackallocCharThreshold
            ? stackalloc char[JsonConstants.StackallocCharThreshold]
            : (rented = ArrayPool<char>.Shared.Rent(TValue.MaxSerializedCharLength));

        try
        {
            if (value.TryFormat(buffer, out var charsWritten, default, default))
            {
                writer.WriteStringValue(buffer[..charsWritten]);
            }
            else
            {
                throw new FormatException($"Could not convert {typeof(TValue).Name} value: {value}");
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

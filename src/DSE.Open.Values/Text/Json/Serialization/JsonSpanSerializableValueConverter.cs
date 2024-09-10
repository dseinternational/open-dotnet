// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance.Buffers;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Values.Text.Json.Serialization;

public sealed class JsonSpanSerializableValueConverter<TValue, T> : JsonConverter<TValue>
    where T : IEquatable<T>, ISpanParsable<T>, ISpanFormattable
    where TValue : struct, IValue<TValue, T>, ISpanSerializable<TValue>
{
    public static readonly JsonSpanSerializableValueConverter<TValue, T> Default = new();

    [SkipLocalsInit]
    public override TValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valueLength = reader.HasValueSequence
           ? checked((int)reader.ValueSequence.Length)
           : reader.ValueSpan.Length;

        var rented = SpanOwner<char>.Empty;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(valueLength)
            ? stackalloc char[valueLength]
            : (rented = SpanOwner<char>.Allocate(valueLength)).Span;

        using (rented)
        {
            var chars = reader.CopyString(buffer);

            var success = TValue.TryParse(buffer[..chars], default, out var value);

            return success ? value : throw new FormatException($"Could not convert {typeof(TValue).Name} value: {buffer}");
        }
    }

    [SkipLocalsInit]
    public override void Write(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        var rented = SpanOwner<char>.Empty;

        var length = TValue.MaxSerializedCharLength;

        Span<char> buffer = MemoryThresholds.CanStackalloc<char>(length)
            ? stackalloc char[length]
            : (rented = SpanOwner<char>.Allocate(length)).Span;

        using (rented)
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
    }
}

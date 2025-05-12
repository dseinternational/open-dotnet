// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// Writes a vector types to JSON.
/// </summary>
public static class SeriesJsonWriter
{
    public static void Write<T>(Utf8JsonWriter writer, IReadOnlySeries<T> vector, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(vector);
        JsonExceptionHelper.ThrowIfLengthExceedsSerializationLimit(vector.Length);

        writer.WriteStartObject();
        writer.WriteString(SeriesJsonPropertyNames.DataType, VectorDataTypeHelper.GetLabel(vector.DataType));
        writer.WriteNumber(SeriesJsonPropertyNames.Length, vector.Length);
        writer.WritePropertyName(SeriesJsonPropertyNames.Values);

        if (vector.Categories.Count > 0)
        {
            writer.WritePropertyName(SeriesJsonPropertyNames.Categories);

            writer.WriteStartObject();

            foreach (var kvp in vector.Categories)
            {
                writer.WritePropertyName(kvp.Key);
                // TODO: Figure out type and write value
                // writer.WriteNumberValue(kvp.Value);
            }

            writer.WriteEndObject();
        }

        if (vector.Length == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
            writer.WriteEndObject();
            return;
        }

        if (typeof(T).IsAssignableFrom(typeof(INumber<>).MakeGenericType(typeof(T))))
        {
            WriteNumberArray(writer, vector);
        }

        if (vector is IReadOnlySeries<string> stringVector)
        {
            WriteStringArray(writer, stringVector.AsReadOnlySpan());
        }
        else if (vector is IReadOnlySeries<char> charVector)
        {
            WriteCharArray(writer, charVector.AsReadOnlySpan());
        }
        else if (vector is IReadOnlySeries<Guid> guidVector)
        {
            WriteGuidArray(writer, guidVector.AsReadOnlySpan());
        }
        else if (vector is IReadOnlySeries<DateTime> dateTimeVector)
        {
            WriteDateTimeArray(writer, dateTimeVector.AsReadOnlySpan());
        }
        else if (vector is IReadOnlySeries<DateTimeOffset> dateTimeOffsetVector)
        {
            WriteDateTimeOffsetArray(writer, dateTimeOffsetVector.AsReadOnlySpan());
        }

        // todo: add support for other types

        writer.WriteEndObject();
    }

    private static void WriteNumberArray<T>(Utf8JsonWriter writer, IReadOnlySeries<T> vector)
    {
        if (typeof(T) == typeof(byte))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<byte>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(sbyte))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<sbyte>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(short))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<short>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(ushort))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<ushort>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(int))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<int>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(uint))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<uint>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(long))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<long>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(ulong))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<ulong>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(float))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<float>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(double))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<double>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(decimal))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<decimal>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(Half))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<Half>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(BigInteger))
        {
            WriteNumberSpan(writer, ((IReadOnlySeries<BigInteger>)vector).AsReadOnlySpan());
        }
        else
        {
            throw new NotSupportedException($"The type `{typeof(T)}` is a not supported numeric type.");
        }


        static void WriteNumberSpan<TNumber>(Utf8JsonWriter writer, ReadOnlySpan<TNumber> vector)
            where TNumber : struct, INumber<TNumber>
        {
            Debug.Assert(vector.Length > 0);
            WriteArray(writer, vector, static (writer, value) => writer.WriteNumberValue(value));
        }
    }

    private static void WriteCharArray(Utf8JsonWriter writer, ReadOnlySpan<char> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value.ToString()));
    }

    private static void WriteStringArray(Utf8JsonWriter writer, ReadOnlySpan<string> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value));
    }

    private static void WriteGuidArray(Utf8JsonWriter writer, ReadOnlySpan<Guid> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value));
    }

    private static void WriteDateTimeArray(Utf8JsonWriter writer, ReadOnlySpan<DateTime> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value));
    }

    private static void WriteDateTimeOffsetArray(Utf8JsonWriter writer, ReadOnlySpan<DateTimeOffset> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteArray<T>(
        Utf8JsonWriter writer,
        ReadOnlySpan<T> vector,
        Action<Utf8JsonWriter, T> writeValue)
    {
        writer.WriteStartArray();

        foreach (var value in vector)
        {
            writeValue(writer, value);
        }

        writer.WriteEndArray();
    }
}

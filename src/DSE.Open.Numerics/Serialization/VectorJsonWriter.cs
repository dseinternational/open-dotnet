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
public static class VectorJsonWriter
{
    public static void Write<T>(Utf8JsonWriter writer, IReadOnlyVector<T> vector, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(vector);
        JsonExceptionHelper.ThrowIfLengthExceedsSerializationLimit(vector.Length);

        writer.WriteStartObject();
        writer.WriteString(VectorJsonPropertyNames.DataType, VectorDataTypeHelper.GetLabel(vector.DataType));
        writer.WriteNumber(VectorJsonPropertyNames.Length, vector.Length);

        if (vector.HasCategories)
        {
            WriteCategories(writer, vector);
        }

        writer.WritePropertyName(VectorJsonPropertyNames.Values);

        if (vector.Length == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
            writer.WriteEndObject();
            return;
        }

        var numberType = typeof(INumber<>).MakeGenericType(typeof(T));

        if (typeof(T).IsAssignableTo(numberType))
        {
            WriteNumberArray(writer, vector);
        }
        else if (vector is IReadOnlyVector<string> stringVector)
        {
            WriteStringArray(writer, stringVector.AsReadOnlySpan());
        }
        else if (vector is IReadOnlyVector<char> charVector)
        {
            WriteCharArray(writer, charVector.AsReadOnlySpan());
        }
        else if (vector is IReadOnlyVector<Guid> guidVector)
        {
            WriteGuidArray(writer, guidVector.AsReadOnlySpan());
        }
        else if (vector is IReadOnlyVector<DateTime> dateTimeVector)
        {
            WriteDateTimeArray(writer, dateTimeVector.AsReadOnlySpan());
        }
        else if (vector is IReadOnlyVector<DateTimeOffset> dateTimeOffsetVector)
        {
            WriteDateTimeOffsetArray(writer, dateTimeOffsetVector.AsReadOnlySpan());
        }

        // todo: add support for other types

        writer.WriteEndObject();
    }

    private static void WriteCategories<T>(Utf8JsonWriter writer, IReadOnlyVector<T> vector)
    {
        writer.WritePropertyName(VectorJsonPropertyNames.Categories);

        writer.WriteStartObject();

        foreach (var kvp in vector.Categories)
        {
            writer.WritePropertyName(kvp.Key);
            // TODO: Figure out type and write value
            // writer.WriteNumberValue(kvp.Value);

            if (kvp.Value is byte byteValue)
            {
                writer.WriteNumberValue(byteValue);
            }
            else if (kvp.Value is sbyte sbyteValue)
            {
                writer.WriteNumberValue(sbyteValue);
            }
            else if (kvp.Value is short shortValue)
            {
                writer.WriteNumberValue(shortValue);
            }
            else if (kvp.Value is ushort ushortValue)
            {
                writer.WriteNumberValue(ushortValue);
            }
            else if (kvp.Value is int intValue)
            {
                writer.WriteNumberValue(intValue);
            }
            else if (kvp.Value is uint uintValue)
            {
                writer.WriteNumberValue(uintValue);
            }
            else if (kvp.Value is long longValue)
            {
                writer.WriteNumberValue(longValue);
            }
            else if (kvp.Value is ulong ulongValue)
            {
                writer.WriteNumberValue(ulongValue);
            }
            else if (kvp.Value is float floatValue)
            {
                writer.WriteNumberValue(floatValue);
            }
            else if (kvp.Value is double doubleValue)
            {
                writer.WriteNumberValue(doubleValue);
            }
            else if (kvp.Value is decimal decimalValue)
            {
                writer.WriteNumberValue(decimalValue);
            }
            else if (kvp.Value is DateTime64 dateTime64Value)
            {
                writer.WriteNumberValue(dateTime64Value);
            }
            else if (kvp.Value is Half halfValue)
            {
                writer.WriteNumberValue(halfValue);
            }
            else if (kvp.Value is string stringValue)
            {
                writer.WriteStringValue(stringValue);
            }
            else if (kvp.Value is char charValue)
            {
                writer.WriteStringValue(charValue.ToString());
            }
            else if (kvp.Value is Guid guidValue)
            {
                writer.WriteStringValue(guidValue);
            }
            else if (kvp.Value is DateTime dateTimeValue)
            {
                writer.WriteStringValue(dateTimeValue);
            }
            else if (kvp.Value is DateTimeOffset dateTimeOffsetValue)
            {
                writer.WriteStringValue(dateTimeOffsetValue);
            }
            else
            {
                throw new JsonException($"The type `{typeof(T)}` is a not supported category value type.");
            }
        }

        writer.WriteEndObject();
    }

    private static void WriteNumberArray<T>(Utf8JsonWriter writer, IReadOnlyVector<T> vector)
    {
        if (typeof(T) == typeof(byte))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<byte>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(sbyte))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<sbyte>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(short))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<short>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(ushort))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<ushort>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(int))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<int>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(uint))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<uint>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(long))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<long>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(ulong))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<ulong>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(float))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<float>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(double))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<double>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(decimal))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<decimal>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(DateTime64))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<DateTime64>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(Half))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<Half>)vector).AsReadOnlySpan());
        }
        else if (typeof(T) == typeof(BigInteger))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<BigInteger>)vector).AsReadOnlySpan());
        }
        else
        {
            throw new JsonException($"The type `{typeof(T)}` is a not supported numeric type.");
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

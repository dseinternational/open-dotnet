// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics.Serialization;

/// <summary>
/// Writes a vector types to JSON.
/// </summary>
public static class VectorJsonWriter
{
    public static void WriteVector(Utf8JsonWriter writer, IReadOnlyVector vector, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(vector);

        switch (vector)
        {
            case IReadOnlyVector<int> intVector:
                WriteVector(writer, intVector, options);
                return;
            case IReadOnlyVector<long> longVector:
                WriteVector(writer, longVector, options);
                return;
            case IReadOnlyVector<float> floatVector:
                WriteVector(writer, floatVector, options);
                return;
            case IReadOnlyVector<double> doubleVector:
                WriteVector(writer, doubleVector, options);
                return;
            case IReadOnlyVector<uint> uintVector:
                WriteVector(writer, uintVector, options);
                return;
            case IReadOnlyVector<ulong> uuidVector:
                WriteVector(writer, uuidVector, options);
                return;
            case IReadOnlyVector<DateTime64> dateTime64Vector:
                WriteVector(writer, dateTime64Vector, options);
                return;
            case IReadOnlyVector<short> shortVector:
                WriteVector(writer, shortVector, options);
                return;
            case IReadOnlyVector<ushort> ushortVector:
                WriteVector(writer, ushortVector, options);
                return;
            case IReadOnlyVector<sbyte> sbyteVector:
                WriteVector(writer, sbyteVector, options);
                return;
            case IReadOnlyVector<byte> byteVector:
                WriteVector(writer, byteVector, options);
                return;
            case IReadOnlyVector<Int128> int128Vector:
                WriteVector(writer, int128Vector, options);
                return;
            case IReadOnlyVector<UInt128> uint128Vector:
                WriteVector(writer, uint128Vector, options);
                return;
            case IReadOnlyVector<string> stringVector:
                WriteVector(writer, stringVector, options);
                return;
            case IReadOnlyVector<char> charVector:
                WriteVector(writer, charVector, options);
                return;
            case IReadOnlyVector<bool> boolVector:
                WriteVector(writer, boolVector, options);
                return;
            case IReadOnlyVector<DateTime> dateTimeVector:
                WriteVector(writer, dateTimeVector, options);
                return;
            default:
                throw new JsonException("Unsupported series type");
        }
    }

    public static void WriteVector<T>(Utf8JsonWriter writer, IReadOnlyVector<T> vector, JsonSerializerOptions options)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(vector);
        JsonExceptionHelper.ThrowIfLengthExceedsSerializationLimit(vector.Length);

        writer.WriteStartObject();

        writer.WriteString(NumericsPropertyNames.DataType, VectorDataTypeHelper.GetLabel(vector.DataType));

        writer.WriteNumber(NumericsPropertyNames.Length, vector.Length);

        writer.WritePropertyName(NumericsPropertyNames.Values);

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
            WriteStringArray(writer, stringVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<char> charVector)
        {
            WriteCharArray(writer, charVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<Guid> guidVector)
        {
            WriteGuidArray(writer, guidVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<DateTime> dateTimeVector)
        {
            WriteDateTimeArray(writer, dateTimeVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<DateTimeOffset> dateTimeOffsetVector)
        {
            WriteDateTimeOffsetArray(writer, dateTimeOffsetVector.AsSpan());
        }

        // todo: add support for other types

        writer.WriteEndObject();
    }

    public static void WriteValue<T>(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        switch (value)
        {
            case byte byteValue:
                writer.WriteNumberValue(byteValue);
                return;
            case sbyte sbyteValue:
                writer.WriteNumberValue(sbyteValue);
                return;
            case short shortValue:
                writer.WriteNumberValue(shortValue);
                return;
            case ushort ushortValue:
                writer.WriteNumberValue(ushortValue);
                return;
            case int intValue:
                writer.WriteNumberValue(intValue);
                return;
            case uint uintValue:
                writer.WriteNumberValue(uintValue);
                return;
            case long longValue:
                writer.WriteNumberValue(longValue);
                return;
            case ulong ulongValue:
                writer.WriteNumberValue(ulongValue);
                return;
            case Int128 int128Value:
                writer.WriteNumberValue(int128Value);
                return;
            case float floatValue:
                writer.WriteNumberValue(floatValue);
                return;
            case double doubleValue:
                writer.WriteNumberValue(doubleValue);
                return;
            case decimal decimalValue:
                writer.WriteNumberValue(decimalValue);
                return;
            case DateTime64 dateTime64Value:
                writer.WriteNumberValue(dateTime64Value);
                return;
            case Half halfValue:
                writer.WriteNumberValue(halfValue);
                return;
            case BigInteger bigIntegerValue:
                writer.WriteNumberValue(bigIntegerValue);
                return;
            case DateTime dateTimeValue:
                writer.WriteStringValue(dateTimeValue);
                return;
            case DateTimeOffset dateTimeOffsetValue:
                writer.WriteStringValue(dateTimeOffsetValue);
                return;
            case Guid guidValue:
                writer.WriteStringValue(guidValue);
                return;
            case string stringValue:
                writer.WriteStringValue(stringValue);
                return;
            default:
                break;
        }

        throw new JsonException($"Unsupported value type: {typeof(T)}");
    }

    private static void WriteNumberArray<T>(Utf8JsonWriter writer, IReadOnlyVector<T> vector)
        where T : IEquatable<T>
    {
        switch (vector)
        {
            case IReadOnlyVector<byte> byteVector:
                WriteNumberSpan(writer, byteVector.AsSpan());
                break;
            case IReadOnlyVector<sbyte> sbyteVector:
                WriteNumberSpan(writer, sbyteVector.AsSpan());
                break;
            case IReadOnlyVector<short> shortVector:
                WriteNumberSpan(writer, shortVector.AsSpan());
                break;
            case IReadOnlyVector<ushort> ushortVector:
                WriteNumberSpan(writer, ushortVector.AsSpan());
                break;
            case IReadOnlyVector<int> intVector:
                WriteNumberSpan(writer, intVector.AsSpan());
                break;
            case IReadOnlyVector<uint> uintVector:
                WriteNumberSpan(writer, uintVector.AsSpan());
                break;
            case IReadOnlyVector<long> longVector:
                WriteNumberSpan(writer, longVector.AsSpan());
                break;
            case IReadOnlyVector<ulong> ulongVector:
                WriteNumberSpan(writer, ulongVector.AsSpan());
                break;
            case IReadOnlyVector<float> floatVector:
                WriteNumberSpan(writer, floatVector.AsSpan());
                break;
            case IReadOnlyVector<double> doubleVector:
                WriteNumberSpan(writer, doubleVector.AsSpan());
                break;
            case IReadOnlyVector<decimal> decimalVector:
                WriteNumberSpan(writer, decimalVector.AsSpan());
                break;
            case IReadOnlyVector<Half> halfVector:
                WriteNumberSpan(writer, halfVector.AsSpan());
                break;
            case IReadOnlyVector<DateTime64> dateTime64Vector:
                WriteNumberSpan(writer, dateTime64Vector.AsSpan());
                break;
            case IReadOnlyVector<Int128> int128Vector:
                WriteNumberSpan(writer, int128Vector.AsSpan());
                break;
            case IReadOnlyVector<UInt128> uint128Vector:
                WriteNumberSpan(writer, uint128Vector.AsSpan());
                break;
            case IReadOnlyVector<BigInteger> bigIntegerVector:
                WriteNumberSpan(writer, bigIntegerVector.AsSpan());
                break;
            default:
                throw new JsonException($"Unsupported numeric vector type: {typeof(T)}");
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
        JsonValueWriter<T> writeValue)
    {
        writer.WriteStartArray();

        foreach (var value in vector)
        {
            writeValue(writer, value);
        }

        writer.WriteEndArray();
    }
}

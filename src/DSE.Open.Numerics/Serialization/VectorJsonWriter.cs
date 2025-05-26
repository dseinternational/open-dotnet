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
    /// <summary>
    /// Writes a vector to JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used to write the JSON.</param>
    /// <param name="vector">The vector to serialize.</param>
    /// <param name="options"></param>
    /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>,
    /// <paramref name="vector"/> is <see langword="null"/>, or
    /// <paramref name="options"/> is <see langword="null"/>.</exception>
    public static void WriteVector(Utf8JsonWriter writer, IReadOnlyVector vector, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(vector);
        ArgumentNullException.ThrowIfNull(options);

        if (vector.IsNullable)
        {
            WriteNaVector(writer, vector, options);
            return;
        }

        // switches over non-na vector types

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
            case IReadOnlyVector<Half> halfVector:
                WriteVector(writer, halfVector, options);
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
            case IReadOnlyVector<string> stringVector:
                WriteVector(writer, stringVector, options);
                return;
            case IReadOnlyVector<char> charVector:
                WriteVector(writer, charVector, options);
                return;
            case IReadOnlyVector<bool> boolVector:
                WriteVector(writer, boolVector, options);
                return;
            case IReadOnlyVector<DateTimeOffset> dateTimeOffsetVector:
                WriteVector(writer, dateTimeOffsetVector, options);
                return;
            case IReadOnlyVector<DateTime> dateTimeVector:
                WriteVector(writer, dateTimeVector, options);
                return;
            default:
                throw new JsonException("Unsupported series type");
        }
    }

    // switches over na vector types
    private static void WriteNaVector(Utf8JsonWriter writer, IReadOnlyVector vector, JsonSerializerOptions options)
    {
        Debug.Assert(vector.IsNullable);

        switch (vector)
        {
            case IReadOnlyVector<NaInt<int>> intVector:
                WriteVector(writer, intVector, options);
                return;
            case IReadOnlyVector<NaInt<long>> longVector:
                WriteVector(writer, longVector, options);
                return;
            case IReadOnlyVector<NaFloat<float>> floatVector:
                WriteVector(writer, floatVector, options);
                return;
            case IReadOnlyVector<NaFloat<double>> doubleVector:
                WriteVector(writer, doubleVector, options);
                return;
            case IReadOnlyVector<NaFloat<Half>> halfVector:
                WriteVector(writer, halfVector, options);
                return;
            case IReadOnlyVector<NaInt<uint>> uintVector:
                WriteVector(writer, uintVector, options);
                return;
            case IReadOnlyVector<NaInt<ulong>> uuidVector:
                WriteVector(writer, uuidVector, options);
                return;
            case IReadOnlyVector<NaInt<DateTime64>> dateTime64Vector:
                WriteVector(writer, dateTime64Vector, options);
                return;
            case IReadOnlyVector<NaInt<short>> shortVector:
                WriteVector(writer, shortVector, options);
                return;
            case IReadOnlyVector<NaInt<ushort>> ushortVector:
                WriteVector(writer, ushortVector, options);
                return;
            case IReadOnlyVector<NaInt<sbyte>> sbyteVector:
                WriteVector(writer, sbyteVector, options);
                return;
            case IReadOnlyVector<NaInt<byte>> byteVector:
                WriteVector(writer, byteVector, options);
                return;
            case IReadOnlyVector<NaValue<string>> stringVector:
                WriteVector(writer, stringVector, options);
                return;
            case IReadOnlyVector<NaValue<char>> charVector:
                WriteVector(writer, charVector, options);
                return;
            case IReadOnlyVector<NaValue<bool>> boolVector:
                WriteVector(writer, boolVector, options);
                return;
            case IReadOnlyVector<NaValue<DateTimeOffset>> dateTimeOffsetVector:
                WriteVector(writer, dateTimeOffsetVector, options);
                return;
            case IReadOnlyVector<NaValue<DateTime>> dateTimeVector:
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

        if (vector.IsNullable)
        {
            WriteNaVector(writer, vector);
            return;
        }

        writer.WriteStartObject();

        writer.WriteString(NumericsPropertyNames.DataType, Vector.GetDataTypeLabel(vector.DataType));

        writer.WriteNumber(NumericsPropertyNames.Length, vector.Length);

        writer.WritePropertyName(NumericsPropertyNames.Values);

        if (vector.Length == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
            writer.WriteEndObject();
            return;
        }

        if (vector.IsNumeric)
        {
            var numberType = typeof(INumber<>).MakeGenericType(typeof(T));

            if (typeof(T).IsAssignableTo(numberType))
            {
                WriteNumberArray(writer, vector);
            }
            else
            {
                throw new JsonException($"Unsupported numeric vector type: {typeof(T)}");
            }
        }
        else if (vector is IReadOnlyVector<string> stringVector)
        {
            WriteStringArray(writer, stringVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<char> charVector)
        {
            WriteCharArray(writer, charVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<DateTime> dateTimeVector)
        {
            WriteDateTimeArray(writer, dateTimeVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<DateTimeOffset> dateTimeOffsetVector)
        {
            WriteDateTimeOffsetArray(writer, dateTimeOffsetVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<bool> boolVector)
        {
            WriteBooleanArray(writer, boolVector.AsSpan());
        }
        else
        {
            throw new JsonException($"Unsupported vector type: {typeof(T)}");
        }

        writer.WriteEndObject();
    }

    private static void WriteNaVector<T>(
        Utf8JsonWriter writer,
        IReadOnlyVector<T> vector)
        where T : IEquatable<T>
    {
        Debug.Assert(writer is not null);
        Debug.Assert(vector is not null);
        Debug.Assert(vector.Length <= VectorJsonConstants.MaximumSerializedLength);

        writer.WriteStartObject();

        writer.WriteString(NumericsPropertyNames.DataType, Vector.GetDataTypeLabel(vector.DataType));

        writer.WriteNumber(NumericsPropertyNames.Length, vector.Length);

        writer.WritePropertyName(NumericsPropertyNames.Values);

        if (vector.Length == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
            writer.WriteEndObject();
            return;
        }

        if (vector.IsNumeric)
        {
            var numberType = typeof(INumber<>).MakeGenericType(typeof(T));

            if (typeof(T).IsAssignableTo(numberType))
            {
                WriteNaNumberArray(writer, vector);
            }
            else
            {
                throw new JsonException($"Unsupported numeric vector type: {typeof(T)}");
            }
        }
        else if (vector is IReadOnlyVector<NaValue<string>> stringVector)
        {
            WriteNaStringArray(writer, stringVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<NaValue<char>> charVector)
        {
            WriteNaCharArray(writer, charVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<NaValue<DateTime>> dateTimeVector)
        {
            WriteNaDateTimeArray(writer, dateTimeVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<NaValue<DateTimeOffset>> dateTimeOffsetVector)
        {
            WriteNaDateTimeOffsetArray(writer, dateTimeOffsetVector.AsSpan());
        }
        else if (vector is IReadOnlyVector<NaValue<bool>> boolVector)
        {
            WriteNaBooleanArray(writer, boolVector.AsSpan());
        }
        else
        {
            throw new JsonException($"Unsupported vector type: {typeof(T)}");
        }

        writer.WriteEndObject();
    }

    /// <summary>
    /// Writes a vector data value to JSON.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <exception cref="JsonException"></exception>
    public static void WriteValue<T>(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        where T : IEquatable<T>
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
                writer.WriteNumberValue(floatValue, true);
                return;
            case double doubleValue:
                writer.WriteNumberValue(doubleValue, true);
                return;
            case decimal decimalValue:
                writer.WriteNumberValue(decimalValue);
                return;
            case DateTime64 dateTime64Value:
                writer.WriteNumberValue(dateTime64Value);
                return;
            case Half halfValue:
                writer.WriteNumberValue(halfValue, true);
                return;
            case DateTime dateTimeValue:
                writer.WriteStringValue(dateTimeValue);
                return;
            case DateTimeOffset dateTimeOffsetValue:
                writer.WriteStringValue(dateTimeOffsetValue);
                return;
            case string stringValue:
                writer.WriteStringValue(stringValue);
                return;
            case NaInt<byte> naByteValue:
                writer.WriteNullableNumberValue<NaInt<byte>, byte>(naByteValue);
                return;
            case NaInt<sbyte> naSbyteValue:
                writer.WriteNullableNumberValue<NaInt<sbyte>, sbyte>(naSbyteValue);
                return;
            case NaInt<short> naShortValue:
                writer.WriteNullableNumberValue<NaInt<short>, short>(naShortValue);
                return;
            case NaInt<ushort> ushortValue:
                writer.WriteNullableNumberValue<NaInt<ushort>, ushort>(ushortValue);
                return;
            case NaInt<int> intValue:
                writer.WriteNullableNumberValue<NaInt<int>, int>(intValue);
                return;
            case NaInt<uint> uintValue:
                writer.WriteNullableNumberValue<NaInt<uint>, uint>(uintValue);
                return;
            case NaInt<long> longValue:
                writer.WriteNullableNumberValue<NaInt<long>, long>(longValue);
                return;
            case NaInt<ulong> ulongValue:
                writer.WriteNullableNumberValue<NaInt<ulong>, ulong>(ulongValue);
                return;
            case NaInt<Int128> int128Value:
                writer.WriteNullableNumberValue<NaInt<Int128>, Int128>(int128Value);
                return;
            case NaFloat<float> floatValue:
                writer.WriteNullableNumberValue<NaFloat<float>, float>(floatValue, true);
                return;
            case NaFloat<double> doubleValue:
                writer.WriteNullableNumberValue<NaFloat<double>, double>(doubleValue, true);
                return;
            case NaInt<DateTime64> dateTime64Value:
                writer.WriteNullableNumberValue<NaInt<DateTime64>, DateTime64>(dateTime64Value);
                return;
            case NaFloat<Half> halfValue:
                writer.WriteNullableNumberValue<NaFloat<Half>, Half>(halfValue, true);
                return;
            case NaValue<DateTime> dateTimeValue:
                writer.WriteNullableValue<NaValue<DateTime>, DateTime>(dateTimeValue);
                return;
            case NaValue<DateTimeOffset> dateTimeOffsetValue:
                writer.WriteNullableValue<NaValue<DateTimeOffset>, DateTimeOffset>(dateTimeOffsetValue);
                return;
            case NaValue<string> stringValue:
                writer.WriteNullableValue<NaValue<string>, string>(stringValue);
                return;
            default:
                break;
        }

        throw new JsonException($"Unsupported value type: {typeof(T)}");
    }

    private static void WriteNumberArray<T>(Utf8JsonWriter writer, IReadOnlyVector<T> vector)
        where T : IEquatable<T>
    {
        Debug.Assert(writer is not null);
        Debug.Assert(vector is not null);
        Debug.Assert(vector.Length <= VectorJsonConstants.MaximumSerializedLength);
        Debug.Assert(!vector.IsNullable);

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
            default:
                throw new JsonException($"Unsupported numeric vector type: {typeof(T)}");
        }

        static void WriteNumberSpan<TNumber>(Utf8JsonWriter writer, ReadOnlySpan<TNumber> vector)
            where TNumber : struct, INumber<TNumber>
        {
            Debug.Assert(vector.Length > 0);
            WriteArray(writer, vector, static (writer, value) => writer.WriteNumberValue(value, true));
        }
    }

    private static void WriteNaNumberArray<T>(Utf8JsonWriter writer, IReadOnlyVector<T> vector)
        where T : IEquatable<T>
    {
        Debug.Assert(writer is not null);
        Debug.Assert(vector is not null);
        Debug.Assert(vector.Length <= VectorJsonConstants.MaximumSerializedLength);
        Debug.Assert(vector.IsNullable);

        switch (vector)
        {
            case IReadOnlyVector<NaInt<byte>> byteVector:
                WriteNaNumberSpan<NaInt<byte>, byte>(writer, byteVector.AsSpan());
                break;
            case IReadOnlyVector<NaInt<sbyte>> sbyteVector:
                WriteNaNumberSpan<NaInt<sbyte>, sbyte>(writer, sbyteVector.AsSpan());
                break;
            case IReadOnlyVector<NaInt<short>> shortVector:
                WriteNaNumberSpan<NaInt<short>, short>(writer, shortVector.AsSpan());
                break;
            case IReadOnlyVector<NaInt<ushort>> ushortVector:
                WriteNaNumberSpan<NaInt<ushort>, ushort>(writer, ushortVector.AsSpan());
                break;
            case IReadOnlyVector<NaInt<int>> intVector:
                WriteNaNumberSpan<NaInt<int>, int>(writer, intVector.AsSpan());
                break;
            case IReadOnlyVector<NaInt<uint>> uintVector:
                WriteNaNumberSpan<NaInt<uint>, uint>(writer, uintVector.AsSpan());
                break;
            case IReadOnlyVector<NaInt<long>> longVector:
                WriteNaNumberSpan<NaInt<long>, long>(writer, longVector.AsSpan());
                break;
            case IReadOnlyVector<NaInt<ulong>> ulongVector:
                WriteNaNumberSpan<NaInt<ulong>, ulong>(writer, ulongVector.AsSpan());
                break;
            case IReadOnlyVector<NaFloat<float>> floatVector:
                WriteNaNumberSpan<NaFloat<float>, float>(writer, floatVector.AsSpan());
                break;
            case IReadOnlyVector<NaFloat<double>> doubleVector:
                WriteNaNumberSpan<NaFloat<double>, double>(writer, doubleVector.AsSpan());
                break;
            case IReadOnlyVector<NaFloat<Half>> halfVector:
                WriteNaNumberSpan<NaFloat<Half>, Half>(writer, halfVector.AsSpan());
                break;
            case IReadOnlyVector<NaInt<DateTime64>> dateTime64Vector:
                WriteNaNumberSpan<NaInt<DateTime64>, DateTime64>(writer, dateTime64Vector.AsSpan());
                break;
            case IReadOnlyVector<NaInt<Int128>> int128Vector:
                WriteNaNumberSpan<NaInt<Int128>, Int128>(writer, int128Vector.AsSpan());
                break;
            case IReadOnlyVector<NaInt<UInt128>> uint128Vector:
                WriteNaNumberSpan<NaInt<UInt128>, UInt128>(writer, uint128Vector.AsSpan());
                break;
            default:
                throw new JsonException($"Unsupported numeric vector type: {typeof(T)}");
        }

        static void WriteNaNumberSpan<TSelf, TNum>(Utf8JsonWriter writer, ReadOnlySpan<TSelf> vector)
            where TNum : struct, INumber<TNum>
            where TSelf : struct, INaValue<TSelf, TNum>
        {
            Debug.Assert(vector.Length > 0);
            WriteArray(writer, vector, static (writer, value) => writer.WriteNullableNumberValue<TSelf, TNum>(value));
        }
    }

    private static void WriteCharArray(Utf8JsonWriter writer, ReadOnlySpan<char> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value.ToString()));
    }

    private static void WriteNaCharArray(Utf8JsonWriter writer, ReadOnlySpan<NaValue<char>> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value.HasValue ? value.ToString() : null));
    }

    private static void WriteStringArray(Utf8JsonWriter writer, ReadOnlySpan<string?> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value));
    }

    private static void WriteNaStringArray(Utf8JsonWriter writer, ReadOnlySpan<NaValue<string>> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value.HasValue ? value.Value : null));
    }

    private static void WriteBooleanArray(Utf8JsonWriter writer, ReadOnlySpan<bool> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteBooleanValue(value));
    }

    private static void WriteNaBooleanArray(Utf8JsonWriter writer, ReadOnlySpan<NaValue<bool>> vector)
    {
        WriteArray(writer, vector, static (writer, value) =>
        {
            if (value.IsNa)
            {
                writer.WriteBooleanValue((bool)value);
            }
            else
            {
                writer.WriteNullValue();
            }
        });
    }

    private static void WriteDateTimeArray(Utf8JsonWriter writer, ReadOnlySpan<DateTime> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value));
    }

    private static void WriteNaDateTimeArray(Utf8JsonWriter writer, ReadOnlySpan<NaValue<DateTime>> vector)
    {
        WriteArray(writer, vector, static (writer, value) =>
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Value);
            }
        });
    }

    private static void WriteDateTimeOffsetArray(Utf8JsonWriter writer, ReadOnlySpan<DateTimeOffset> vector)
    {
        WriteArray(writer, vector, static (writer, value) => writer.WriteStringValue(value));
    }

    private static void WriteNaDateTimeOffsetArray(Utf8JsonWriter writer, ReadOnlySpan<NaValue<DateTimeOffset>> vector)
    {
        WriteArray(writer, vector, static (writer, value) =>
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Value);
            }
        });
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

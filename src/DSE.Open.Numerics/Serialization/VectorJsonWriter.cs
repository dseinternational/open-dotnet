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
    public static void Write(Utf8JsonWriter writer, IReadOnlyVector vector, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(vector);

        switch (vector)
        {
            case IReadOnlyVector<int> intVector:
                Write(writer, intVector, options);
                return;
            case IReadOnlyVector<long> longVector:
                Write(writer, longVector, options);
                return;
            case IReadOnlyVector<float> floatVector:
                Write(writer, floatVector, options);
                return;
            case IReadOnlyVector<double> doubleVector:
                Write(writer, doubleVector, options);
                return;
            case IReadOnlyVector<uint> uintVector:
                Write(writer, uintVector, options);
                return;
            case IReadOnlyVector<ulong> uuidVector:
                Write(writer, uuidVector, options);
                return;
            case IReadOnlyVector<DateTime64> dateTime64Vector:
                Write(writer, dateTime64Vector, options);
                return;
            case IReadOnlyVector<short> shortVector:
                Write(writer, shortVector, options);
                return;
            case IReadOnlyVector<ushort> ushortVector:
                Write(writer, ushortVector, options);
                return;
            case IReadOnlyVector<sbyte> sbyteVector:
                Write(writer, sbyteVector, options);
                return;
            case IReadOnlyVector<byte> byteVector:
                Write(writer, byteVector, options);
                return;
            case IReadOnlyVector<Int128> int128Vector:
                Write(writer, int128Vector, options);
                return;
            case IReadOnlyVector<UInt128> uint128Vector:
                Write(writer, uint128Vector, options);
                return;
            case IReadOnlyVector<string> stringVector:
                Write(writer, stringVector, options);
                return;
            case IReadOnlyVector<char> charVector:
                Write(writer, charVector, options);
                return;
            case IReadOnlyVector<bool> boolVector:
                Write(writer, boolVector, options);
                return;
            case IReadOnlyVector<DateTime> dateTimeVector:
                Write(writer, dateTimeVector, options);
                return;
            default:
                throw new JsonException("Unsupported series type");
        }
    }

    public static void Write<T>(Utf8JsonWriter writer, IReadOnlyVector<T> vector, JsonSerializerOptions options)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(vector);
        JsonExceptionHelper.ThrowIfLengthExceedsSerializationLimit(vector.Length);

        writer.WriteStartObject();
        writer.WriteString(VectorJsonPropertyNames.DataType, VectorDataTypeHelper.GetLabel(vector.DataType));
        writer.WriteNumber(VectorJsonPropertyNames.Length, vector.Length);
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

    private static void WriteNumberArray<T>(Utf8JsonWriter writer, IReadOnlyVector<T> vector)
        where T : IEquatable<T>
    {
        if (typeof(T) == typeof(byte))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<byte>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(sbyte))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<sbyte>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(short))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<short>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(ushort))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<ushort>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(int))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<int>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(uint))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<uint>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(long))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<long>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(ulong))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<ulong>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(float))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<float>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(double))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<double>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(decimal))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<decimal>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(DateTime64))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<DateTime64>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(Half))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<Half>)vector).AsSpan());
        }
        else if (typeof(T) == typeof(BigInteger))
        {
            WriteNumberSpan(writer, ((IReadOnlyVector<BigInteger>)vector).AsSpan());
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

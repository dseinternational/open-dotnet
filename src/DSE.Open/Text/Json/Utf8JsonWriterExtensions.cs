// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;

namespace DSE.Open.Text.Json;

public static class Utf8JsonWriterExtensions
{
    public static void WriteNumberValue<T>(this Utf8JsonWriter writer, T value)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (value is ulong uint64)
        {
            writer.WriteNumberValue(uint64);
        }
        else if (value is long int64)
        {
            writer.WriteNumberValue(int64);
        }
        else if (value is uint uint32)
        {
            writer.WriteNumberValue(uint32);
        }
        else if (value is int int32)
        {
            writer.WriteNumberValue(int32);
        }
        else if (value is ushort uint16)
        {
            writer.WriteNumberValue(uint16);
        }
        else if (value is short int16)
        {
            writer.WriteNumberValue(int16);
        }
        else if (value is byte uint8)
        {
            writer.WriteNumberValue(uint8);
        }
        else if (value is sbyte int8)
        {
            writer.WriteNumberValue(int8);
        }
        else if (value is float single)
        {
            writer.WriteNumberValue(single);
        }
        else if (value is double doubleVal)
        {
            writer.WriteNumberValue(doubleVal);
        }
        else if (value is decimal decimalVal)
        {
            writer.WriteNumberValue(decimalVal);
        }
        else if (value is DateTime64 dateTime64)
        {
            writer.WriteNumberValue(dateTime64.TotalMilliseconds);
        }
        else if (T.IsInteger(value))
        {
            writer.WriteNumberValue(long.CreateChecked(value));
        }
        else
        {
            writer.WriteNumberValue(double.CreateChecked(value));
        }
    }

    public static void WriteNumberValue(this Utf8JsonWriter writer, decimal value, bool ensureFloatingPoint)
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (ensureFloatingPoint && !value.HasDecimalPlaces())
        {
            writer.WriteRawValue(value.ToStringInvariant("F1"));
        }
        else
        {
            writer.WriteNumberValue(value);
        }
    }

    public static void WriteNumberValue(this Utf8JsonWriter writer, double value, bool ensureFloatingPoint)
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (ensureFloatingPoint && !value.HasDecimalPlaces())
        {
            writer.WriteRawValue(value.ToStringInvariant("F1"));
        }
        else
        {
            writer.WriteNumberValue(value);
        }
    }

    public static void WriteNumberValue(this Utf8JsonWriter writer, float value, bool ensureFloatingPoint)
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (ensureFloatingPoint && !value.HasDecimalPlaces())
        {
            writer.WriteRawValue(value.ToStringInvariant("F1"));
        }
        else
        {
            writer.WriteNumberValue(value);
        }
    }
}

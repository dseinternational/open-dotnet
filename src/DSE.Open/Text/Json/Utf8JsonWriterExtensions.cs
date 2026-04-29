// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;

namespace DSE.Open.Text.Json;

/// <summary>
/// Extension methods for <see cref="Utf8JsonWriter"/>.
/// </summary>
public static class Utf8JsonWriterExtensions
{
    /// <summary>
    /// Writes the specified <typeparamref name="T"/> value as a JSON number, dispatching to the
    /// appropriate <see cref="Utf8JsonWriter"/> overload for the underlying numeric type.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="allowNamedFloatingPointLiterals">
    /// When <see langword="true"/>, NaN and infinity values for floating-point types are written as JSON
    /// strings using their named representations.
    /// </param>
    public static void WriteNumberValue<T>(
        this Utf8JsonWriter writer,
        T value,
        bool allowNamedFloatingPointLiterals = false)
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
            if (allowNamedFloatingPointLiterals
                && (float.IsNaN(single)
                    || float.PositiveInfinity.Equals(single)
                    || float.NegativeInfinity.Equals(single)))
            {
                writer.WriteStringValue(single.ToStringInvariant("G"));
            }
            else
            {
                writer.WriteNumberValue(single);
            }
        }
        else if (value is double doubleVal)
        {
            if (allowNamedFloatingPointLiterals
                && (double.IsNaN(doubleVal)
                    || double.PositiveInfinity.Equals(doubleVal)
                    || double.NegativeInfinity.Equals(doubleVal)))
            {
                writer.WriteStringValue(doubleVal.ToStringInvariant("G"));
            }
            else
            {
                writer.WriteNumberValue(doubleVal);
            }
        }
        else if (value is Half halfVal)
        {
            if (allowNamedFloatingPointLiterals
                && (Half.IsNaN(halfVal)
                    || Half.PositiveInfinity.Equals(halfVal)
                    || Half.NegativeInfinity.Equals(halfVal)))
            {
                writer.WriteStringValue(halfVal.ToStringInvariant("G"));
            }
            else
            {
                writer.WriteNumberValue(halfVal);
            }
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

    /// <summary>
    /// Writes the specified <see cref="INaValue{TSelf, T}"/> value, emitting a JSON null when the
    /// value represents N/A and otherwise dispatching to the appropriate JSON token for the
    /// underlying value type.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="allowNamedFloatingPointLiterals">
    /// When <see langword="true"/>, NaN and infinity values for floating-point types are written as JSON
    /// strings using their named representations.
    /// </param>
    public static void WriteNullableValue<TSelf, T>(
        this Utf8JsonWriter writer,
        TSelf value,
        bool allowNamedFloatingPointLiterals = false)
        where T : IEquatable<T>
        where TSelf : struct, INaValue<TSelf, T>
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (value.IsNa || value.Value is null)
        {
            writer.WriteNullValue();
        }
        else if (value.Value is string stringValue)
        {
            writer.WriteStringValue(stringValue);
        }
        else if (value.Value is char charValue)
        {
            writer.WriteStringValue(charValue.ToString());
        }
        else if (value.Value is Guid guidValue)
        {
            writer.WriteStringValue(guidValue);
        }
        else if (value.Value is DateTimeOffset dateTimeOffsetValue)
        {
            writer.WriteStringValue(dateTimeOffsetValue);
        }
        else if (value.Value is DateTime dateTimeValue)
        {
            writer.WriteStringValue(dateTimeValue);
        }
        else if (value.Value is bool boolValue)
        {
            writer.WriteBooleanValue(boolValue);
        }
        else if (value.Value is ulong uint64)
        {
            writer.WriteNumberValue(uint64);
        }
        else if (value.Value is long int64)
        {
            writer.WriteNumberValue(int64);
        }
        else if (value.Value is uint uint32)
        {
            writer.WriteNumberValue(uint32);
        }
        else if (value.Value is int int32)
        {
            writer.WriteNumberValue(int32);
        }
        else if (value.Value is ushort uint16)
        {
            writer.WriteNumberValue(uint16);
        }
        else if (value.Value is short int16)
        {
            writer.WriteNumberValue(int16);
        }
        else if (value.Value is byte uint8)
        {
            writer.WriteNumberValue(uint8);
        }
        else if (value.Value is sbyte int8)
        {
            writer.WriteNumberValue(int8);
        }
        else if (value.Value is float single)
        {
            writer.WriteNumberValue(single, allowNamedFloatingPointLiterals);
        }
        else if (value.Value is double doubleVal)
        {
            writer.WriteNumberValue(doubleVal, allowNamedFloatingPointLiterals);
        }
        else if (value.Value is Half halfVal)
        {
            writer.WriteNumberValue(halfVal, allowNamedFloatingPointLiterals);
        }
        else if (value.Value is decimal decimalVal)
        {
            writer.WriteNumberValue(decimalVal);
        }
        else if (value.Value is DateTime64 dateTime64)
        {
            writer.WriteNumberValue(dateTime64.TotalMilliseconds);
        }
    }

    /// <summary>
    /// Writes the specified <see cref="INaValue{TSelf, T}"/> numeric value, emitting a JSON null when
    /// the value represents N/A and otherwise dispatching to the appropriate numeric writer overload.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="allowNamedFloatingPointLiterals">
    /// When <see langword="true"/>, NaN and infinity values for floating-point types are written as JSON
    /// strings using their named representations.
    /// </param>
    public static void WriteNullableNumberValue<TSelf, T>(
        this Utf8JsonWriter writer,
        TSelf value,
        bool allowNamedFloatingPointLiterals = false)
        where T : struct, INumber<T>
        where TSelf : struct, INaValue<TSelf, T>
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (value.IsNa)
        {
            writer.WriteNullValue();
        }
        else if (value.Value is ulong uint64)
        {
            writer.WriteNumberValue(uint64);
        }
        else if (value.Value is long int64)
        {
            writer.WriteNumberValue(int64);
        }
        else if (value.Value is uint uint32)
        {
            writer.WriteNumberValue(uint32);
        }
        else if (value.Value is int int32)
        {
            writer.WriteNumberValue(int32);
        }
        else if (value.Value is ushort uint16)
        {
            writer.WriteNumberValue(uint16);
        }
        else if (value.Value is short int16)
        {
            writer.WriteNumberValue(int16);
        }
        else if (value.Value is byte uint8)
        {
            writer.WriteNumberValue(uint8);
        }
        else if (value.Value is sbyte int8)
        {
            writer.WriteNumberValue(int8);
        }
        else if (value.Value is float single)
        {
            writer.WriteNumberValue(single, allowNamedFloatingPointLiterals);
        }
        else if (value.Value is double doubleVal)
        {
            writer.WriteNumberValue(doubleVal, allowNamedFloatingPointLiterals);
        }
        else if (value.Value is Half halfVal)
        {
            writer.WriteNumberValue(halfVal, allowNamedFloatingPointLiterals);
        }
        else if (value.Value is decimal decimalVal)
        {
            writer.WriteNumberValue(decimalVal);
        }
        else if (value.Value is DateTime64 dateTime64)
        {
            writer.WriteNumberValue(dateTime64.TotalMilliseconds);
        }
        else if (T.IsInteger(value.Value))
        {
            writer.WriteNumberValue(long.CreateChecked(value.Value));
        }
        else
        {
            writer.WriteNumberValue(double.CreateChecked(value.Value));
        }
    }
}

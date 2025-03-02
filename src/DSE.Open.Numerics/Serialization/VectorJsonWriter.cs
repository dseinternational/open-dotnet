// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
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
        writer.WritePropertyName(VectorJsonPropertyNames.Values);

        if (vector.Length == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
        }
        else if (vector is IReadOnlyVector<string> stringVector)
        {
            WriteStringArray(writer, stringVector.Span);
        }
        else if (vector is IReadOnlyVector<char> charVector)
        {
            WriteCharArray(writer, charVector.Span);
        }
        else if (vector is IReadOnlyVector<Guid> guidVector)
        {
            WriteGuidArray(writer, guidVector.Span);
        }
        else if (vector is IReadOnlyVector<DateTime> dateTimeVector)
        {
            WriteDateTimeArray(writer, dateTimeVector.Span);
        }
        else if (vector is IReadOnlyVector<DateTimeOffset> dateTimeOffsetVector)
        {
            WriteDateTimeOffsetArray(writer, dateTimeOffsetVector.Span);
        }

        // todo: add support for other types

        writer.WriteEndObject();
    }

    public static void Write<T>(Utf8JsonWriter writer, IReadOnlyNumericVector<T> vector, JsonSerializerOptions options)
        where T : struct, INumber<T>
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
        }
        else
        {
            WriteNumberArray(writer, vector.Span);
        }

        writer.WriteEndObject();
    }

    public static void Write<T>(Utf8JsonWriter writer, IReadOnlyCategoricalVector<T> vector, JsonSerializerOptions options)
        where T : struct, IComparable<T>, IEquatable<T>, IBinaryInteger<T>, IMinMaxValue<T>
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(vector);
        JsonExceptionHelper.ThrowIfLengthExceedsSerializationLimit(vector.Length);

        writer.WriteStartObject();
        writer.WriteString(VectorJsonPropertyNames.DataType, VectorDataTypeHelper.GetLabel(vector.DataType));
        writer.WriteNumber(VectorJsonPropertyNames.Length, vector.Length);
        writer.WritePropertyName(VectorJsonPropertyNames.Values);

        // todo: error if no categories and > 0 data values ??

        if (vector.CategoryData.Length > 0)
        {
            writer.WritePropertyName(VectorJsonPropertyNames.Categories);

            writer.WriteStartObject();

            foreach (var kvp in vector.CategoryData)
            {
                writer.WritePropertyName(kvp.Key);
                writer.WriteNumberValue(kvp.Value);
            }

            writer.WriteEndObject();
        }

        if (vector.Length == 0)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
        }
        else
        {
            WriteNumberArray(writer, vector.Span);
        }

        writer.WriteEndObject();
    }

    private static void WriteNumberArray<T>(Utf8JsonWriter writer, ReadOnlySpan<T> vector)
        where T : struct, INumber<T>
    {
        Debug.Assert(vector.Length > 0);

        writer.WriteStartArray();

        foreach (var value in vector)
        {
            writer.WriteNumberValue(value);
        }

        writer.WriteEndArray();
    }

    private static void WriteCharArray(Utf8JsonWriter writer, ReadOnlySpan<char> vector)
    {
        writer.WriteStartArray();

        foreach (var value in vector)
        {
            writer.WriteStringValue(value.ToString());
        }

        writer.WriteEndArray();
    }

    private static void WriteStringArray(Utf8JsonWriter writer, ReadOnlySpan<string> vector)
    {
        writer.WriteStartArray();

        foreach (var value in vector)
        {
            writer.WriteStringValue(value);
        }

        writer.WriteEndArray();
    }

    private static void WriteGuidArray(Utf8JsonWriter writer, ReadOnlySpan<Guid> vector)
    {
        writer.WriteStartArray();

        foreach (var value in vector)
        {
            writer.WriteStringValue(value);
        }

        writer.WriteEndArray();
    }

    private static void WriteDateTimeArray(Utf8JsonWriter writer, ReadOnlySpan<DateTime> vector)
    {
        writer.WriteStartArray();
        foreach (var value in vector)
        {
            writer.WriteStringValue(value);
        }

        writer.WriteEndArray();
    }

    private static void WriteDateTimeOffsetArray(Utf8JsonWriter writer, ReadOnlySpan<DateTimeOffset> vector)
    {
        writer.WriteStartArray();
        foreach (var value in vector)
        {
            writer.WriteStringValue(value);
        }

        writer.WriteEndArray();
    }
}

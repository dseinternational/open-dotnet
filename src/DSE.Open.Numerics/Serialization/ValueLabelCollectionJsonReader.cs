// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;
using DSE.Open.Memory;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics.Serialization;

#pragma warning disable DSEOPEN001 // ArrayBuilder ref struct warning

internal static class ValueLabelCollectionJsonReader
{
    public static ValueLabelCollection? ReadLabels(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        string? dataType = null;
        var length = -1;
        ValueLabelCollection? labels = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var property = reader.GetString();

                if (property == NumericsPropertyNames.Length)
                {
                    _ = reader.Read();
                    length = reader.GetInt32();

                    if (length < 0)
                    {
                        throw new JsonException("Length must be greater than or equal to zero");
                    }

                    if (length > VectorJsonConstants.MaximumSerializedLength)
                    {
                        throw new JsonException("Length must be less than or equal to " + VectorJsonConstants.MaximumSerializedLength);
                    }
                }
                else if (property == NumericsPropertyNames.DataType)
                {
                    _ = reader.Read();
                    dataType = reader.GetString();

                    if (dataType is null)
                    {
                        throw new JsonException("Data type must be specified");
                    }
                }
                else if (property == "labels")
                {
                    if (length < 0)
                    {
                        throw new JsonException();
                    }

                    if (dataType is null)
                    {
                        throw new JsonException();
                    }

                    var type = VectorDataTypeHelper.GetVectorDataType(dataType);

                    switch (type)
                    {
                        case VectorDataType.Float64:
                            labels = ReadNumberLabels<double>(ref reader, length);
                            break;
                        case VectorDataType.Float32:
                            labels = ReadNumberLabels<float>(ref reader, length);
                            break;
                        case VectorDataType.Int64:
                            labels = ReadNumberLabels<long>(ref reader, length);
                            break;
                        case VectorDataType.UInt64:
                            labels = ReadNumberLabels<ulong>(ref reader, length);
                            break;
                        case VectorDataType.Int32:
                            labels = ReadNumberLabels<int>(ref reader, length);
                            break;
                        case VectorDataType.UInt32:
                            labels = ReadNumberLabels<uint>(ref reader, length);
                            break;
                        case VectorDataType.Int16:
                            labels = ReadNumberLabels<short>(ref reader, length);
                            break;
                        case VectorDataType.UInt16:
                            labels = ReadNumberLabels<ushort>(ref reader, length);
                            break;
                        case VectorDataType.Int8:
                            labels = ReadNumberLabels<sbyte>(ref reader, length);
                            break;
                        case VectorDataType.UInt8:
                            labels = ReadNumberLabels<byte>(ref reader, length);
                            break;
                        case VectorDataType.Int128:
                            labels = ReadNumberLabels<Int128>(ref reader, length);
                            break;
                        case VectorDataType.UInt128:
                            labels = ReadNumberLabels<UInt128>(ref reader, length);
                            break;
                        case VectorDataType.DateTime64:
                            labels = ReadNumberLabels<DateTime64>(ref reader, length);
                            break;
                        case VectorDataType.DateTime:
                            labels = ReadDateTimeLabels(ref reader, length);
                            break;
                        case VectorDataType.DateTimeOffset:
                            labels = ReadDateTimeOffsetLabels(ref reader, length);
                            break;
                        case VectorDataType.Uuid:
                            labels = ReadGuidLabels(ref reader, length);
                            break;
                        case VectorDataType.Bool:
                            labels = ReadBooleanLabels(ref reader, length);
                            break;
                        case VectorDataType.Char:
                            break;
                        case VectorDataType.String:
                            labels = ReadStringLabels(ref reader, length);
                            break;
                        default:
                            throw new JsonException($"Unsupported data type: {dataType}");
                    }
                }
                else
                {
                    throw new JsonException($"Unexpected property: {property}");
                }
            }
        }

        return labels;
    }

    private static ValueLabelCollection<T> ReadNumberLabels<T>(ref Utf8JsonReader reader, int length)
        where T : struct, INumber<T>
    {
        return ReadLabels(ref reader, length, (ref r) =>
        {
            if (r.TryGetNumber<T>(out var value))
            {
                return value;
            }

            throw new JsonException("Expected number value.");
        });
    }

    private static ValueLabelCollection<string> ReadStringLabels(ref Utf8JsonReader reader, int length)
    {
        return ReadLabels(ref reader, length, (ref r) => r.GetString() ?? throw new JsonException("Expected non-null string value."));
    }

    private static ValueLabelCollection<bool> ReadBooleanLabels(ref Utf8JsonReader reader, int length)
    {
        return ReadLabels(ref reader, length, (ref r) => r.GetBoolean());
    }

    private static ValueLabelCollection<Guid> ReadGuidLabels(ref Utf8JsonReader reader, int length)
    {
        return ReadLabels(ref reader, length, (ref r) => r.GetGuid());
    }

    private static ValueLabelCollection<DateTime> ReadDateTimeLabels(ref Utf8JsonReader reader, int length)
    {
        return ReadLabels(ref reader, length, (ref r) => r.GetDateTime());
    }

    private static ValueLabelCollection<DateTimeOffset> ReadDateTimeOffsetLabels(ref Utf8JsonReader reader, int length)
    {
        return ReadLabels(ref reader, length, (ref r) => r.GetDateTimeOffset());
    }

    private static ValueLabelCollection<T> ReadLabels<T>(ref Utf8JsonReader reader, int length, JsonValueReader<T> valueReader)
        where T : IEquatable<T>
    {
        _ = reader.Read();

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object for labels");
        }

        using var builder = length > -1
            ? new ArrayBuilder<ValueLabel<T>>(length, rentFromPool: false)
            : new ArrayBuilder<ValueLabel<T>>(rentFromPool: true);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                continue;
            }

            var label = reader.GetString();

            if (label is null)
            {
                throw new JsonException("Label must be specified.");
            }

            _ = reader.Read();

            var value = valueReader(ref reader);

            builder.Add((value, label));
        }

        return [.. builder.ToArray()];
    }
}

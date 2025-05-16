// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

internal static class ValueLabelCollectionJsonWriter
{
    public static void WriteCollection(
        Utf8JsonWriter writer,
        IReadOnlyValueLabelCollection labels,
        JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(labels);

        switch (labels)
        {
            case IReadOnlyValueLabelCollection<int> intValueLabel:
                WriteCollection(writer, intValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<long> longValueLabel:
                WriteCollection(writer, longValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<float> floatValueLabel:
                WriteCollection(writer, floatValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<double> doubleValueLabel:
                WriteCollection(writer, doubleValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<uint> uintValueLabel:
                WriteCollection(writer, uintValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<ulong> uuidValueLabel:
                WriteCollection(writer, uuidValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<DateTime64> dateTime64ValueLabel:
                WriteCollection(writer, dateTime64ValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<short> shortValueLabel:
                WriteCollection(writer, shortValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<ushort> ushortValueLabel:
                WriteCollection(writer, ushortValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<sbyte> sbyteValueLabel:
                WriteCollection(writer, sbyteValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<byte> byteValueLabel:
                WriteCollection(writer, byteValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<Int128> int128ValueLabel:
                WriteCollection(writer, int128ValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<UInt128> uint128ValueLabel:
                WriteCollection(writer, uint128ValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<string> stringValueLabel:
                WriteCollection(writer, stringValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<char> charValueLabel:
                WriteCollection(writer, charValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<bool> boolValueLabel:
                WriteCollection(writer, boolValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<DateTime> dateTimeValueLabel:
                WriteCollection(writer, dateTimeValueLabel, options);
                return;
            case IReadOnlyValueLabelCollection<DateTimeOffset> dateTimeOffsetValueLabel:
                WriteCollection(writer, dateTimeOffsetValueLabel, options);
                return;
            default:
                throw new JsonException("Unsupported series type");

        }
    }

    public static void WriteCollection<T>(
        Utf8JsonWriter writer,
        IReadOnlyValueLabelCollection<T> labels,
        JsonSerializerOptions options)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(labels);

        writer.WriteStartObject();

        writer.WriteString(
            NumericsPropertyNames.DataType,
            VectorDataTypeHelper.GetLabel(VectorDataTypeHelper.GetVectorDataType<T>()));

        writer.WriteNumber(
            NumericsPropertyNames.Length,
            labels.Count);

        if (labels.Count > 0)
        {
            writer.WritePropertyName(NumericsPropertyNames.Values);

            writer.WriteStartObject();

            foreach (var dataLabel in labels)
            {
                writer.WritePropertyName(dataLabel.Label);
                VectorJsonWriter.WriteValue(writer, dataLabel.Value, options);
            }

            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }
}

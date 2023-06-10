// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Text.Json;

public static class Utf8JsonWriterExtensions
{
    public static void WriteNumberValue(this Utf8JsonWriter writer, decimal value, bool ensureFloatingPoint)
    {
        Guard.IsNotNull(writer);

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
        Guard.IsNotNull(writer);

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
        Guard.IsNotNull(writer);

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

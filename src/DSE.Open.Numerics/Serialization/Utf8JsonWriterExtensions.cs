// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

public static class Utf8JsonWriterExtensions
{
    public static void WriteNumberValue<T>(this Utf8JsonWriter writer, T value)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (value.GetType() == typeof(ulong))
        {
            writer.WriteNumberValue(ulong.CreateChecked(value));
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
}

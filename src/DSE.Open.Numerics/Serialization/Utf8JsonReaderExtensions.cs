// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json;

namespace DSE.Open.Numerics.Serialization;

public static class Utf8JsonReaderExtensions
{
    public static bool TryGetNumber<T>(this Utf8JsonReader reader, out T value)
        where T : struct, INumber<T>
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt64(out var int64Value))
            {
                value = T.CreateChecked(int64Value);
                return true;
            }

            if (reader.TryGetUInt64(out var uint64Value))
            {
                value = T.CreateChecked(uint64Value);
                return true;
            }

            if (reader.TryGetDouble(out var doubleValue))
            {
                value = T.CreateChecked(doubleValue);
                return true;
            }
        }

        value = default;
        return false;
    }
}

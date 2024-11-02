// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimestampToByteArrayConverter : ValueConverter<Timestamp, byte[]>
{
    public static readonly TimestampToByteArrayConverter Default = new();

    public TimestampToByteArrayConverter()
        : base(v => ConvertToByteArray(v), v => ConvertFromByteArray(v))
    {
    }

    // keep public for EF Core compiled models
    public static byte[] ConvertToByteArray(Timestamp value)
    {
        return value.GetBytes();
    }

    // keep public for EF Core compiled models
    public static Timestamp ConvertFromByteArray(byte[] value)
    {
        if (Timestamp.TryCreate(value, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert byte array '{value}' to {nameof(Timestamp)}");
        return default; // unreachable
    }
}

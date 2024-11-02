// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimestampToBase64StringConverter : ValueConverter<Timestamp, string>
{
    public static readonly TimestampToBase64StringConverter Default = new();

    public TimestampToBase64StringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertToString(Timestamp value)
    {
        return value.ToBase64String();
    }

    // keep public for EF Core compiled models
    public static Timestamp ConvertFromString(string value)
    {
        if (Timestamp.TryParse(value, null, out var timestamp))
        {
            return timestamp;
        }

        ValueConversionException.Throw($"Could not convert string '{value}' to {nameof(Timestamp)}");
        return default; // unreachable
    }
}

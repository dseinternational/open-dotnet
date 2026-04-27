// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TelephoneNumberToStringConverter : ValueConverter<TelephoneNumber, string>
{
    public static readonly TelephoneNumberToStringConverter Default = new();

    public TelephoneNumberToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertToString(TelephoneNumber number)
    {
        return number.ToString();
    }

    // keep public for EF Core compiled models
    public static TelephoneNumber ConvertFromString(string value)
    {
        if (TelephoneNumber.TryParse(value, out var number))
        {
            return number;
        }

        ValueConversionException.Throw($"Could not convert string '{value}' to {nameof(TelephoneNumber)}", value, null);
        return default; // unreachable
    }
}

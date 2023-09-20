// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class AgeInMonthsToStringConverter : ValueConverter<AgeInMonths, string>
{
    public static readonly AgeInMonthsToStringConverter Default = new();

    public AgeInMonthsToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    private static string ConvertToString(AgeInMonths code) => code.ToString();

    private static AgeInMonths ConvertFromString(string code)
    {
        if (AgeInMonths.TryParse(code, out var age))
        {
            return age;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(AgeInMonths)}");
        return default; // unreachable
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class CountryCodeToStringConverter : ValueConverter<CountryCode, string>
{
    public static readonly CountryCodeToStringConverter Default = new();

    public CountryCodeToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    private static string ConvertToString(CountryCode code) => code.ToStringUpper();

    private static CountryCode ConvertFromString(string code)
    {
        if (CountryCode.TryParse(code, out var countryCode))
        {
            return countryCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(CountryCode)}");
        return default; // unreachable
    }
}

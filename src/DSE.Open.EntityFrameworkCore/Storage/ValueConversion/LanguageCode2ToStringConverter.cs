// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class LanguageCode2ToStringConverter : ValueConverter<LanguageCode2, string>
{
    public static readonly LanguageCode2ToStringConverter Default = new();

    public LanguageCode2ToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    private static string ConvertToString(LanguageCode2 code) => code.ToString();

    private static LanguageCode2 ConvertFromString(string code)
    {
        if (LanguageCode2.TryParse(code, null, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(LanguageCode2)}");
        return default; // unreachable
    }
}

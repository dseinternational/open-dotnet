// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class LanguageTagToStringConverter : ValueConverter<LanguageTag, string>
{
    public static readonly LanguageTagToStringConverter Default = new();

    public LanguageTagToStringConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    private static string ConvertTo(LanguageTag code)
    {
        return code.ToStringFormatted();
    }

    private static LanguageTag ConvertFrom(string code)
    {
        if (LanguageTag.TryParse(code, out var languageCode))
        {
            return languageCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(LanguageTag)}");
        return default; // unreachable
    }
}

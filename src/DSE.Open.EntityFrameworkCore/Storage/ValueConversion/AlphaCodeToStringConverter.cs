// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class AlphaCodeToStringConverter : ValueConverter<AlphaCode, string>
{
    public static readonly AlphaCodeToStringConverter Default = new();

    public AlphaCodeToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    private static string ConvertToString(AlphaCode code)
    {
        return code.ToString();
    }

    private static AlphaCode ConvertFromString(string code)
    {
        if (AlphaCode.TryParse(code, out var alphaCode))
        {
            return alphaCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(AlphaCode)}");
        return default; // unreachable
    }
}

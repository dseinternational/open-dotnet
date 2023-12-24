// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class AlphaNumericCodeToStringConverter : ValueConverter<AlphaNumericCode, string>
{
    public static readonly AlphaNumericCodeToStringConverter Default = new();

    public AlphaNumericCodeToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    private static string ConvertToString(AlphaNumericCode code)
    {
        return code.ToString();
    }

    private static AlphaNumericCode ConvertFromString(string code)
    {
        if (AlphaNumericCode.TryParse(code, out var value))
        {
            return value;
        }

        ValueConversionException.Throw($"Error converting string value '{code}' to AlphaNumericCode.", code, null);
        return default;
    }
}

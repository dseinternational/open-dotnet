// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class CodeToStringConverter : ValueConverter<Code, string>
{
    public static readonly CodeToStringConverter Default = new();

    public CodeToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertToString(Code code)
    {
        return code.ToString();
    }

    // keep public for EF Core compiled models
    public static Code ConvertFromString(string code)
    {
        if (Code.TryParse(code, null, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(Code)}");
        return default; // unreachable
    }
}

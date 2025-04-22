// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class SentenceStructureToStringConverter : ValueConverter<SentenceStructure, string>
{
    public static readonly SentenceStructureToStringConverter Default = new();

    public SentenceStructureToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    // public for EF Core model compilation
    public static string ConvertToString(SentenceStructure code)
    {
        return code.ToString();
    }

    // public for EF Core model compilation
    public static SentenceStructure ConvertFromString(string code)
    {
        if (SentenceStructure.TryParse(code, out var alphaCode))
        {
            return alphaCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(SentenceStructure)}");
        return default; // unreachable
    }
}

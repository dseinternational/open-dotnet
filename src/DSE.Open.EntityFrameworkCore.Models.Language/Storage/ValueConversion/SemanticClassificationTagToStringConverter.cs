// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class SemanticClassificationTagToStringConverter : ValueConverter<SemanticClassificationTag, string>
{
    public static readonly SemanticClassificationTagToStringConverter Default = new();

    public SemanticClassificationTagToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    // public for EF Core model compilation
    public static string ConvertToString(SemanticClassificationTag code)
    {
        return code.ToString();
    }

    // public for EF Core model compilation
    public static SemanticClassificationTag ConvertFromString(string code)
    {
        if (SemanticClassificationTag.TryParse(code, out var alphaCode))
        {
            return alphaCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(SemanticClassificationTag)}");
        return default; // unreachable
    }
}

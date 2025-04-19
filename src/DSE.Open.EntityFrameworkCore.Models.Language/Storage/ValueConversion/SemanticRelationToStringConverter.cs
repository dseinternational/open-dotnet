// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class SemanticRelationToStringConverter : ValueConverter<SemanticRelation, string>
{
    public static readonly SemanticRelationToStringConverter Default = new();

    public SemanticRelationToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    // public for EF Core model compilation
    public static string ConvertToString(SemanticRelation code)
    {
        return code.ToString();
    }

    // public for EF Core model compilation
    public static SemanticRelation ConvertFromString(string code)
    {
        if (SemanticRelation.TryParse(code, out var alphaCode))
        {
            return alphaCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(SemanticRelation)}");
        return default; // unreachable
    }
}

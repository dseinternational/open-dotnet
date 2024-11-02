// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class WordTextToStringConverter : ValueConverter<WordText, string>
{
    public static readonly WordTextToStringConverter Default = new();

    public WordTextToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    // public for EF Core model compilation
    public static string ConvertTo(WordText value)
    {
        return value.ToString();
    }

    // public for EF Core model compilation
    public static WordText ConvertFrom(string value)
    {
        return new(value);
    }
}

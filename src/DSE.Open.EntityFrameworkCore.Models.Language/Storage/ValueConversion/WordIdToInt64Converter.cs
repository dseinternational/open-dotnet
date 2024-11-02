// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class WordIdToInt64Converter : ValueConverter<WordId, long>
{
    public static readonly WordIdToInt64Converter Default = new();

    public WordIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    // public for EF Core model compilation
    public static long ConvertTo(WordId value)
    {
        return (long)value;
    }

    // public for EF Core model compilation
    public static WordId ConvertFrom(long value)
    {
        if (WordId.TryFromInt64(value, out var id))
        {
            return id;
        }

        ValueConversionException.Throw(
            $"Unable to convert {nameof(Int64)} value '{value}' to {nameof(WordId)}.", value, null);
        return default;
    }
}

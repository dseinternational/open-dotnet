// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class SentenceMeaningIdToInt64Converter : ValueConverter<SentenceMeaningId, long>
{
    public static readonly SentenceMeaningIdToInt64Converter Default = new();

    public SentenceMeaningIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    // public for EF Core model compilation
    public static long ConvertTo(SentenceMeaningId value)
    {
        return (long)value;
    }

    // public for EF Core model compilation
    public static SentenceMeaningId ConvertFrom(long value)
    {
        if (SentenceMeaningId.TryFromInt64(value, out var id))
        {
            return id;
        }

        ValueConversionException.Throw(
            $"Unable to convert {nameof(Int64)} value '{value}' to {nameof(SentenceMeaningId)}.", value, null);
        return default;
    }
}

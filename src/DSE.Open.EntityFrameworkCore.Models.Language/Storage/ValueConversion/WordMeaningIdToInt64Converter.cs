// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class WordMeaningIdToInt64Converter : ValueConverter<WordMeaningId, long>
{
    public static readonly WordMeaningIdToInt64Converter Default = new();

    public WordMeaningIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    private static long ConvertTo(WordMeaningId value)
    {
        return (long)value;
    }

    private static WordMeaningId ConvertFrom(long value)
    {
        if (WordMeaningId.TryFromInt64(value, out var id))
        {
            return id;
        }

        ValueConversionException.Throw(
            $"Unable to convert {typeof(long).Name} value '{value}' to {nameof(WordMeaningId)}.", value, null);
        return default;
    }
}

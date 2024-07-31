// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class SentenceIdToInt64Converter : ValueConverter<SentenceId, long>
{
    public static readonly SentenceIdToInt64Converter Default = new();

    public SentenceIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    private static long ConvertTo(SentenceId value)
    {
        return (long)value;
    }

    private static SentenceId ConvertFrom(long value)
    {
        if (SentenceId.TryFromInt64(value, out var id))
        {
            return id;
        }

        ValueConversionException.Throw(
            $"Unable to convert {typeof(long).Name} value '{value}' to {nameof(SentenceId)}.", value, null);
        return default;
    }
}

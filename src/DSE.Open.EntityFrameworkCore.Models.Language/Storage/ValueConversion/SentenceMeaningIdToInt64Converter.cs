// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="SentenceMeaningId"/> values to and from <see cref="long"/>.
/// </summary>
public sealed class SentenceMeaningIdToInt64Converter : ValueConverter<SentenceMeaningId, long>
{
    /// <summary>
    /// Gets the default <see cref="SentenceMeaningIdToInt64Converter"/> instance.
    /// </summary>
    public static readonly SentenceMeaningIdToInt64Converter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="SentenceMeaningIdToInt64Converter"/> class.
    /// </summary>
    public SentenceMeaningIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="SentenceMeaningId"/> to <see cref="long"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static long ConvertTo(SentenceMeaningId value)
    {
        return (long)value;
    }

    /// <summary>
    /// Converts a <see cref="long"/> to a <see cref="SentenceMeaningId"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
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

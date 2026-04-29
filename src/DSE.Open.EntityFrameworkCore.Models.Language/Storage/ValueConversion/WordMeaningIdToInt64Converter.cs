// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="WordMeaningId"/> values to and from <see cref="long"/>.
/// </summary>
public sealed class WordMeaningIdToInt64Converter : ValueConverter<WordMeaningId, long>
{
    /// <summary>
    /// Gets the default <see cref="WordMeaningIdToInt64Converter"/> instance.
    /// </summary>
    public static readonly WordMeaningIdToInt64Converter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="WordMeaningIdToInt64Converter"/> class.
    /// </summary>
    public WordMeaningIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="WordMeaningId"/> to <see cref="long"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static long ConvertTo(WordMeaningId value)
    {
        return (long)value;
    }

    /// <summary>
    /// Converts a <see cref="long"/> to a <see cref="WordMeaningId"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static WordMeaningId ConvertFrom(long value)
    {
        if (WordMeaningId.TryFromInt64(value, out var id))
        {
            return id;
        }

        ValueConversionException.Throw(
            $"Unable to convert {nameof(Int64)} value '{value}' to {nameof(WordMeaningId)}.", value, null);
        return default;
    }
}

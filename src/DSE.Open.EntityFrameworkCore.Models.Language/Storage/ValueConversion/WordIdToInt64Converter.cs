// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="WordId"/> values to and from <see cref="long"/>.
/// </summary>
public sealed class WordIdToInt64Converter : ValueConverter<WordId, long>
{
    /// <summary>
    /// Gets the default <see cref="WordIdToInt64Converter"/> instance.
    /// </summary>
    public static readonly WordIdToInt64Converter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="WordIdToInt64Converter"/> class.
    /// </summary>
    public WordIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="WordId"/> to <see cref="long"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static long ConvertTo(WordId value)
    {
        return (long)value;
    }

    /// <summary>
    /// Converts a <see cref="long"/> to a <see cref="WordId"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
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

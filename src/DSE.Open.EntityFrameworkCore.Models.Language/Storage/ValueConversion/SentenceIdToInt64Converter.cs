// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="SentenceId"/> values to and from <see cref="long"/>.
/// </summary>
public sealed class SentenceIdToInt64Converter : ValueConverter<SentenceId, long>
{
    /// <summary>
    /// Gets the default <see cref="SentenceIdToInt64Converter"/> instance.
    /// </summary>
    public static readonly SentenceIdToInt64Converter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="SentenceIdToInt64Converter"/> class.
    /// </summary>
    public SentenceIdToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="SentenceId"/> to <see cref="long"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static long ConvertTo(SentenceId value)
    {
        return (long)value;
    }

    /// <summary>
    /// Converts a <see cref="long"/> to a <see cref="SentenceId"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static SentenceId ConvertFrom(long value)
    {
        if (SentenceId.TryFromInt64(value, out var id))
        {
            return id;
        }

        ValueConversionException.Throw(
            $"Unable to convert {nameof(Int64)} value '{value}' to {nameof(SentenceId)}.", value, null);
        return default;
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="WordText"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class WordTextToStringConverter : ValueConverter<WordText, string>
{
    /// <summary>
    /// Gets the default <see cref="WordTextToStringConverter"/> instance.
    /// </summary>
    public static readonly WordTextToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="WordTextToStringConverter"/> class.
    /// </summary>
    public WordTextToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="WordText"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertTo(WordText value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="WordText"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static WordText ConvertFrom(string value)
    {
        return new(value);
    }
}

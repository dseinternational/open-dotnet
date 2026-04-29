// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="TokenText"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class TokenTextToStringConverter : ValueConverter<TokenText, string>
{
    /// <summary>
    /// Gets the default <see cref="TokenTextToStringConverter"/> instance.
    /// </summary>
    public static readonly TokenTextToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="TokenTextToStringConverter"/> class.
    /// </summary>
    public TokenTextToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="TokenText"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertTo(TokenText value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="TokenText"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static TokenText ConvertFrom(string value)
    {
        return new(value);
    }
}

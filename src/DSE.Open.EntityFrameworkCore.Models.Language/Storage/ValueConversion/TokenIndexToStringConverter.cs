// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="TokenIndex"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class TokenIndexToStringConverter : ValueConverter<TokenIndex, string>
{
    /// <summary>
    /// Gets the default <see cref="TokenIndexToStringConverter"/> instance.
    /// </summary>
    public static readonly TokenIndexToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="TokenIndexToStringConverter"/> class.
    /// </summary>
    public TokenIndexToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="TokenIndex"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertTo(TokenIndex value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="TokenIndex"/> using invariant culture.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static TokenIndex ConvertFrom(string value)
    {
        return TokenIndex.ParseInvariant(value);
    }
}

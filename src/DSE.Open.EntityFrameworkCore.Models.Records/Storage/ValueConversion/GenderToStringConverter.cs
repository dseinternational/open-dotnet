// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.EntityFrameworkCore.Storage.ValueConversion;
using DSE.Open.Records;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Records.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="Gender"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class GenderToStringConverter : ValueConverter<Gender, string>
{
    /// <summary>
    /// The default shared instance of the converter.
    /// </summary>
    public static readonly GenderToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="GenderToStringConverter"/> class.
    /// </summary>
    public GenderToStringConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="Gender"/> value to its invariant <see cref="string"/> representation.
    /// </summary>
    /// <param name="code">The <see cref="Gender"/> value to convert.</param>
    /// <returns>The invariant <see cref="string"/> representation of <paramref name="code"/>.</returns>
    // keep public for EF Core compiled models
    public static string ConvertTo(Gender code)
    {
        return code.ToStringInvariant();
    }

    /// <summary>
    /// Converts a <see cref="string"/> value to its <see cref="Gender"/> representation.
    /// </summary>
    /// <param name="code">The <see cref="string"/> value to convert, parsed using the invariant culture.</param>
    /// <returns>The <see cref="Gender"/> value represented by <paramref name="code"/>.</returns>
    // keep public for EF Core compiled models
    public static Gender ConvertFrom(string code)
    {
        if (Gender.TryParseInvariant(code, out var gender))
        {
            return gender;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(Gender)}", code, null);
        return default; // unreachable
    }
}

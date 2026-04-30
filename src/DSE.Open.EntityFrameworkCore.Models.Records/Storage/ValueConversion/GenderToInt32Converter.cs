// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Records;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Records.Storage.ValueConversion;

// Note: https://github.com/dotnet/efcore/issues/13850

/// <summary>
/// EF Core value converter that maps <see cref="Gender"/> values to and from <see cref="int"/>.
/// </summary>
public sealed class GenderToInt32Converter : ValueConverter<Gender, int>
{
    /// <summary>
    /// The default shared instance of the converter.
    /// </summary>
    public static readonly GenderToInt32Converter Default = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="GenderToInt32Converter"/> class.
    /// </summary>
    public GenderToInt32Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="Gender"/> value to its <see cref="int"/> representation.
    /// </summary>
    /// <param name="code">The <see cref="Gender"/> value to convert.</param>
    /// <returns>The <see cref="int"/> value representing <paramref name="code"/>.</returns>
    // keep public for EF Core compiled models
    public static int ConvertTo(Gender code)
    {
        return GenderToByteConverter.ConvertTo(code);
    }

    /// <summary>
    /// Converts an <see cref="int"/> value to its <see cref="Gender"/> representation.
    /// </summary>
    /// <param name="code">The <see cref="int"/> value to convert.</param>
    /// <returns>The <see cref="Gender"/> value represented by <paramref name="code"/>.</returns>
    // keep public for EF Core compiled models
    public static Gender ConvertFrom(int code)
    {
        return GenderToByteConverter.ConvertFrom((byte)code);
    }
}

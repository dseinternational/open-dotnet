// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="UniversalPosTag"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class UniversalPosTagToStringConverter : ValueConverter<UniversalPosTag, string>
{
    /// <summary>
    /// Gets the default <see cref="UniversalPosTagToStringConverter"/> instance.
    /// </summary>
    public static readonly UniversalPosTagToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="UniversalPosTagToStringConverter"/> class.
    /// </summary>
    public UniversalPosTagToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="UniversalPosTag"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertTo(UniversalPosTag value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="UniversalPosTag"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static UniversalPosTag ConvertFrom(string value)
    {
        return new(value);
    }
}

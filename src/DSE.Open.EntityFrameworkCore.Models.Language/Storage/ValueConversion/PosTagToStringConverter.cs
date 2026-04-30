// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="PosTag"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class PosTagToStringConverter : ValueConverter<PosTag, string>
{
    /// <summary>
    /// Gets the default <see cref="PosTagToStringConverter"/> instance.
    /// </summary>
    public static readonly PosTagToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="PosTagToStringConverter"/> class.
    /// </summary>
    public PosTagToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="PosTag"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertTo(PosTag value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="PosTag"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static PosTag ConvertFrom(string value)
    {
        return new(value);
    }
}

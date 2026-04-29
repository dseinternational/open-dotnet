// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="TreebankPosTag"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class TreebankPosTagToStringConverter : ValueConverter<TreebankPosTag, string>
{
    /// <summary>
    /// Gets the default <see cref="TreebankPosTagToStringConverter"/> instance.
    /// </summary>
    public static readonly TreebankPosTagToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="TreebankPosTagToStringConverter"/> class.
    /// </summary>
    public TreebankPosTagToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="TreebankPosTag"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertTo(TreebankPosTag value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="TreebankPosTag"/>.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static TreebankPosTag ConvertFrom(string value)
    {
        return new(value);
    }
}

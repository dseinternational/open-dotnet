// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="UniversalRelationTag"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class UniversalRelationTagToStringConverter : ValueConverter<UniversalRelationTag, string>
{
    /// <summary>
    /// Gets the default <see cref="UniversalRelationTagToStringConverter"/> instance.
    /// </summary>
    public static readonly UniversalRelationTagToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="UniversalRelationTagToStringConverter"/> class.
    /// </summary>
    public UniversalRelationTagToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="UniversalRelationTag"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertTo(UniversalRelationTag value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="UniversalRelationTag"/> using invariant culture.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static UniversalRelationTag ConvertFrom(string value)
    {
        return UniversalRelationTag.ParseInvariant(value);
    }
}

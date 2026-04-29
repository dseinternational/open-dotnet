// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="ReadOnlyAttributeValueCollection"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class ReadOnlyAttributeValueCollectionToStringConverter : ValueConverter<ReadOnlyAttributeValueCollection, string>
{
    /// <summary>
    /// Gets the default <see cref="ReadOnlyAttributeValueCollectionToStringConverter"/> instance.
    /// </summary>
    public static readonly ReadOnlyAttributeValueCollectionToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="ReadOnlyAttributeValueCollectionToStringConverter"/> class.
    /// </summary>
    public ReadOnlyAttributeValueCollectionToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="ReadOnlyAttributeValueCollection"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertTo(ReadOnlyAttributeValueCollection value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="ReadOnlyAttributeValueCollection"/> using invariant culture.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static ReadOnlyAttributeValueCollection ConvertFrom(string value)
    {
        return ReadOnlyAttributeValueCollection.ParseInvariant(value);
    }
}

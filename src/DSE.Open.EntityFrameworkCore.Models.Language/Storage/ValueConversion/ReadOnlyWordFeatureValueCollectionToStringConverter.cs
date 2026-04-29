// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

/// <summary>
/// EF Core value converter that maps <see cref="ReadOnlyWordFeatureValueCollection"/> values to and from <see cref="string"/>.
/// </summary>
public sealed class ReadOnlyWordFeatureValueCollectionToStringConverter : ValueConverter<ReadOnlyWordFeatureValueCollection, string>
{
    /// <summary>
    /// Gets the default <see cref="ReadOnlyWordFeatureValueCollectionToStringConverter"/> instance.
    /// </summary>
    public static readonly ReadOnlyWordFeatureValueCollectionToStringConverter Default = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="ReadOnlyWordFeatureValueCollectionToStringConverter"/> class.
    /// </summary>
    public ReadOnlyWordFeatureValueCollectionToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="ReadOnlyWordFeatureValueCollection"/> to its string representation.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static string ConvertTo(ReadOnlyWordFeatureValueCollection value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.ToString();
    }

    /// <summary>
    /// Converts a string to a <see cref="ReadOnlyWordFeatureValueCollection"/> using invariant culture.
    /// </summary>
    /// <remarks>Public for EF Core model compilation.</remarks>
    public static ReadOnlyWordFeatureValueCollection ConvertFrom(string value)
    {
        return ReadOnlyWordFeatureValueCollection.ParseInvariant(value);
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class ReadOnlyWordFeatureValueCollectionToStringConverter : ValueConverter<ReadOnlyWordFeatureValueCollection, string>
{
    public static readonly ReadOnlyWordFeatureValueCollectionToStringConverter Default = new();

    public ReadOnlyWordFeatureValueCollectionToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    // public for EF Core model compilation
    public static string ConvertTo(ReadOnlyWordFeatureValueCollection value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.ToString();
    }

    // public for EF Core model compilation
    public static ReadOnlyWordFeatureValueCollection ConvertFrom(string value)
    {
        return ReadOnlyWordFeatureValueCollection.ParseInvariant(value);
    }
}

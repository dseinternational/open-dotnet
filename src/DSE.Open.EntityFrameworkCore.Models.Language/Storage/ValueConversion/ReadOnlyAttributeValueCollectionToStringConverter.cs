// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class ReadOnlyAttributeValueCollectionToStringConverter : ValueConverter<ReadOnlyAttributeValueCollection, string>
{
    public static readonly ReadOnlyAttributeValueCollectionToStringConverter Default = new();

    public ReadOnlyAttributeValueCollectionToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    private static string ConvertTo(ReadOnlyAttributeValueCollection value)
    {
        return value.ToString();
    }

    private static ReadOnlyAttributeValueCollection ConvertFrom(string value)
    {
        return ReadOnlyAttributeValueCollection.ParseInvariant(value);
    }
}

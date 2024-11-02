// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class UniversalRelationTagToStringConverter : ValueConverter<UniversalRelationTag, string>
{
    public static readonly UniversalRelationTagToStringConverter Default = new();

    public UniversalRelationTagToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    // public for EF Core model compilation
    public static string ConvertTo(UniversalRelationTag value)
    {
        return value.ToString();
    }

    // public for EF Core model compilation
    public static UniversalRelationTag ConvertFrom(string value)
    {
        return UniversalRelationTag.ParseInvariant(value);
    }
}

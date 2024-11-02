// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Language.Storage.ValueConversion;

public sealed class UniversalPosTagToStringConverter : ValueConverter<UniversalPosTag, string>
{
    public static readonly UniversalPosTagToStringConverter Default = new();

    public UniversalPosTagToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    // public for EF Core model compilation
    public static string ConvertTo(UniversalPosTag value)
    {
        return value.ToString();
    }

    // public for EF Core model compilation
    public static UniversalPosTag ConvertFrom(string value)
    {
        return new(value);
    }
}

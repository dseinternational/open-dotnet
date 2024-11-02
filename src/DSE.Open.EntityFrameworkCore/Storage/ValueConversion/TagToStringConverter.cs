// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TagToStringConverter : ValueConverter<Tag, string>
{
    public static readonly TagToStringConverter Default = new();

    public TagToStringConverter()
        : base(v => ConvertToString(v), v => ConvertToUniqueId(v))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertToString(Tag value)
    {
        return value.ToString();
    }

    // keep public for EF Core compiled models
    public static Tag ConvertToUniqueId(string value)
    {
        return new(value);
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class LikePatternToStringConverter : ValueConverter<LikePattern, string>
{
    public static readonly LikePatternToStringConverter Default = new();

    public LikePatternToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertTo(LikePattern value)
    {
        return value.ToString();
    }

    // keep public for EF Core compiled models
    public static LikePattern ConvertFrom(string value)
    {
        return new(value);
    }
}

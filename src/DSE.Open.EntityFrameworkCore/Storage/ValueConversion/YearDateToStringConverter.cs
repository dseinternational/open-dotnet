// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class YearDateToStringConverter : ValueConverter<YearDate, string>
{
    public static readonly YearDateToStringConverter Default = new();

    public YearDateToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertTo(YearDate value)
    {
        return value.ToString();
    }

    // keep public for EF Core compiled models
    public static YearDate ConvertFrom(string value)
    {
        return YearDate.Parse(value, CultureInfo.InvariantCulture);
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Time;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimePeriodToStringConverter : ValueConverter<TimePeriod, string>
{
    public static readonly TimePeriodToStringConverter Default = new();

    public TimePeriodToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    // keep public for EF Core compiled models
    public static string ConvertToString(TimePeriod value)
    {
        return value.ToString();
    }

    // keep public for EF Core compiled models
    public static TimePeriod ConvertFromString(string value)
    {
        return TimePeriod.Parse(value);
    }
}

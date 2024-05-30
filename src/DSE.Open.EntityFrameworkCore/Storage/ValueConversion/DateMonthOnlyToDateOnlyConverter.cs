// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class DateMonthOnlyToDateOnlyConverter : ValueConverter<DateMonthOnly, DateOnly>
{
    public static readonly DateMonthOnlyToDateOnlyConverter Default = new();

    public DateMonthOnlyToDateOnlyConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    private static DateOnly ConvertTo(DateMonthOnly value)
    {
        return value.StartOfMonth;
    }

    private static DateMonthOnly ConvertFrom(DateOnly value)
    {
        return new(value);
    }
}

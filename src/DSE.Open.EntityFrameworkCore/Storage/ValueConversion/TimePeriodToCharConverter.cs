// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Time;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimePeriodToCharConverter : ValueConverter<TimePeriod, char>
{
    public static readonly TimePeriodToCharConverter Default = new();

    public TimePeriodToCharConverter()
        : base(c => ConvertToChar(c), s => ConvertFromChar(s))
    {
    }

    // keep public for EF Core compiled models
    public static char ConvertToChar(TimePeriod value)
    {
        if (value == TimePeriod.None)
        {
            return 'N';
        }

        if (value == TimePeriod.Month)
        {
            return 'M';
        }

        if (value == TimePeriod.Year)
        {
            return 'Y';
        }

        if (value == TimePeriod.Week)
        {
            return 'W';
        }

        if (value == TimePeriod.Day)
        {
            return 'D';
        }

        if (value == TimePeriod.Hour)
        {
            return 'H';
        }

        return value == TimePeriod.Minute
            ? 'I'
            : throw new InvalidOperationException("Invalid TimePeriod value: " + value);
    }

    // keep public for EF Core compiled models
    public static TimePeriod ConvertFromChar(char value)
    {
        return value switch
        {
            'N' => TimePeriod.None,
            'M' => TimePeriod.Month,
            'Y' => TimePeriod.Year,
            'W' => TimePeriod.Week,
            'D' => TimePeriod.Day,
            'H' => TimePeriod.Hour,
            'I' => TimePeriod.Minute,
            _ => throw new InvalidOperationException("Invalid TimePeriod char value: " + value)
        };
    }
}

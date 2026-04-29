// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Time;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="TimePeriod"/> to a <see cref="char"/> for storage.
/// </summary>
public sealed class TimePeriodToCharConverter : ValueConverter<TimePeriod, char>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimePeriodToCharConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimePeriodToCharConverter"/>.
    /// </summary>
    public TimePeriodToCharConverter()
        : base(c => ConvertToChar(c), s => ConvertFromChar(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="TimePeriod"/> to its single-character storage form
    /// (<c>'N'</c> none, <c>'M'</c> month, <c>'Y'</c> year, <c>'W'</c> week,
    /// <c>'D'</c> day, <c>'H'</c> hour, <c>'I'</c> minute).
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The corresponding character.</returns>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="value"/> is not a recognized period.</exception>
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

    /// <summary>
    /// Converts a single-character storage value back to a <see cref="TimePeriod"/>.
    /// </summary>
    /// <param name="value">The stored character.</param>
    /// <returns>The corresponding <see cref="TimePeriod"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="value"/> is not a recognized period character.</exception>
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

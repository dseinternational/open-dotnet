// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Time;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="TimePeriod"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class TimePeriodToStringConverter : ValueConverter<TimePeriod, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimePeriodToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimePeriodToStringConverter"/>.
    /// </summary>
    public TimePeriodToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="TimePeriod"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(TimePeriod value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="TimePeriod"/>.
    /// </summary>
    /// <param name="value">The stored string value.</param>
    /// <returns>The parsed <see cref="TimePeriod"/>.</returns>
    // keep public for EF Core compiled models
    public static TimePeriod ConvertFromString(string value)
    {
        return TimePeriod.Parse(value);
    }
}

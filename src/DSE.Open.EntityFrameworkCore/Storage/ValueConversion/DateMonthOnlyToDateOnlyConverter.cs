// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="DateMonthOnly"/> to a <see cref="DateOnly"/> for storage,
/// using the first day of the month as the canonical representation.
/// </summary>
public sealed class DateMonthOnlyToDateOnlyConverter : ValueConverter<DateMonthOnly, DateOnly>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly DateMonthOnlyToDateOnlyConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="DateMonthOnlyToDateOnlyConverter"/>.
    /// </summary>
    public DateMonthOnlyToDateOnlyConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="DateMonthOnly"/> to its <see cref="DateOnly"/> storage form (start of month).
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The first day of the month as a <see cref="DateOnly"/>.</returns>
    // keep public for EF Core compiled models
    public static DateOnly ConvertTo(DateMonthOnly value)
    {
        return value.StartOfMonth;
    }

    /// <summary>
    /// Converts a <see cref="DateOnly"/> storage value back to a <see cref="DateMonthOnly"/>.
    /// </summary>
    /// <param name="value">The stored date.</param>
    /// <returns>The reconstructed <see cref="DateMonthOnly"/>.</returns>
    // keep public for EF Core compiled models
    public static DateMonthOnly ConvertFrom(DateOnly value)
    {
        return new(value);
    }
}

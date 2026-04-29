// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="YearDate"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class YearDateToStringConverter : ValueConverter<YearDate, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly YearDateToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="YearDateToStringConverter"/>.
    /// </summary>
    public YearDateToStringConverter() : base(v => ConvertTo(v), v => ConvertFrom(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="YearDate"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertTo(YearDate value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="YearDate"/>,
    /// parsing using the invariant culture.
    /// </summary>
    /// <param name="value">The stored string value.</param>
    /// <returns>The parsed <see cref="YearDate"/>.</returns>
    // keep public for EF Core compiled models
    public static YearDate ConvertFrom(string value)
    {
        return YearDate.Parse(value, CultureInfo.InvariantCulture);
    }
}

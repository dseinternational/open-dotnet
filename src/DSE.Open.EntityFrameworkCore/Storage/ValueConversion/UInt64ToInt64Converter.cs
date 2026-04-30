// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="ulong"/> to a <see cref="long"/> for storage using checked arithmetic.
/// </summary>
public sealed class UInt64ToInt64Converter : ValueConverter<ulong, long>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly UInt64ToInt64Converter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="UInt64ToInt64Converter"/>.
    /// </summary>
    public UInt64ToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="ulong"/> to its <see cref="long"/> storage form using checked arithmetic.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The narrowed <see cref="long"/>.</returns>
    /// <exception cref="OverflowException">Thrown when <paramref name="value"/> is outside the range of <see cref="long"/>.</exception>
    // keep public for EF Core compiled models
    public static long ConvertTo(ulong value)
    {
        checked
        {
            return (long)value;
        }
    }

    /// <summary>
    /// Converts a <see cref="long"/> storage value back to a <see cref="ulong"/> by reinterpreting its bits.
    /// </summary>
    /// <param name="value">The stored <see cref="long"/> value.</param>
    /// <returns>The reinterpreted <see cref="ulong"/>.</returns>
    // keep public for EF Core compiled models
    public static ulong ConvertFrom(long value)
    {
        return (ulong)value;
    }
}

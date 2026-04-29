// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="uint"/> to a <see cref="long"/> for storage. Conversion back to
/// <see cref="uint"/> is checked and will throw <see cref="OverflowException"/> on out-of-range values.
/// </summary>
public sealed class UInt32ToInt64Converter : ValueConverter<uint, long>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly UInt32ToInt64Converter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="UInt32ToInt64Converter"/>.
    /// </summary>
    public UInt32ToInt64Converter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="uint"/> to its <see cref="long"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The widened <see cref="long"/>.</returns>
    // keep public for EF Core compiled models
    public static long ConvertTo(uint value)
    {
        return value;
    }

    /// <summary>
    /// Converts a <see cref="long"/> storage value back to a <see cref="uint"/> using checked arithmetic.
    /// </summary>
    /// <param name="value">The stored <see cref="long"/> value.</param>
    /// <returns>The narrowed <see cref="uint"/>.</returns>
    /// <exception cref="OverflowException">Thrown when <paramref name="value"/> is outside the range of <see cref="uint"/>.</exception>
    // keep public for EF Core compiled models
    public static uint ConvertFrom(long value)
    {
        checked
        {
            return (uint)value;
        }
    }
}

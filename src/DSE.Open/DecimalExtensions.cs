// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open;

/// <summary>
/// Extension methods for <see cref="decimal"/>.
/// </summary>
public static class DecimalExtensions
{
    /// <summary>
    /// Gets the number of non-zero decimal places in the value. For example,
    /// <c>1.250m</c> returns <c>2</c> and <c>42m</c> returns <c>0</c>.
    /// </summary>
    /// <param name="val">The value.</param>
    /// <returns>The number of decimal places, from 0 up to the decimal precision (28).</returns>
    public static int GetDecimalPlacesCount(this decimal val)
    {
        var i = 0;

        while (i < 30 && Math.Round(val, i) != val)
        {
            i++;
        }

        return i;
    }

    /// <summary>
    /// Indicates whether the value has a non-zero fractional component.
    /// </summary>
    /// <param name="val">The value.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="val"/> has any non-zero digits to the right
    /// of the decimal point; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasDecimalPlaces(this decimal val)
    {
        return Math.Round(val, 0) != val;
    }

    /// <summary>
    /// Computes a 64-bit hash code that is stable across application runs and process
    /// architectures.
    /// </summary>
    /// <param name="number">The value.</param>
    /// <returns>A repeatable 64-bit hash code for <paramref name="number"/>.</returns>
    public static ulong GetRepeatableHashCode(this decimal number)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(number);
    }
}

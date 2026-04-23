// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open;

/// <summary>
/// Extension methods for <see cref="float"/>.
/// </summary>
public static class SingleExtensions
{
    /// <summary>
    /// Gets the number of non-zero decimal places in the value, up to a maximum of 10.
    /// Due to the limits of binary floating-point representation, results for values
    /// with more than a few decimal digits may not match the originating literal.
    /// </summary>
    /// <param name="val">The value.</param>
    /// <returns>The number of decimal places, from 0 to 10.</returns>
    public static int GetDecimalPlacesCount(this float val)
    {
        var i = 0;

        while (i < 10 && Math.Round(val, i) != val)
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
    /// <see langword="true"/> if <paramref name="val"/> is not an exact integer;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasDecimalPlaces(this float val)
    {
        return Math.Round(val, 0) != val;
    }

    /// <summary>
    /// Computes a 64-bit hash code that is stable across application runs and process
    /// architectures.
    /// </summary>
    /// <param name="number">The value.</param>
    /// <returns>A repeatable 64-bit hash code for <paramref name="number"/>.</returns>
    public static ulong GetRepeatableHashCode(this float number)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(number);
    }
}

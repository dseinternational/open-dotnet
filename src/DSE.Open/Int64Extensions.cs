// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open;

/// <summary>
/// Extension methods for <see cref="long"/>.
/// </summary>
public static class Int64Extensions
{
    /// <summary>
    /// Reinterprets the value's bit pattern as a <see cref="ulong"/>. Negative values wrap
    /// into the upper half of the <see cref="ulong"/> range.
    /// </summary>
    public static ulong ToUInt64(this long value)
    {
        return (ulong)value;
    }

    /// <summary>
    /// Converts the value to a <see cref="ulong"/>, throwing <see cref="OverflowException"/>
    /// when <paramref name="value"/> is negative.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is less than zero.</exception>
    public static ulong ToUInt64Checked(this long value)
    {
        checked
        {
            return (ulong)value;
        }
    }

    /// <summary>
    /// Gets the number of base-10 digits required to represent the value, ignoring its sign.
    /// Returns <c>1</c> for <c>0</c>.
    /// </summary>
    /// <param name="number">The value.</param>
    public static int GetDigitCount(this long number)
    {
        // Compute absolute value as ulong to handle long.MinValue, which would
        // overflow Math.Abs. For long.MinValue, unchecked -number wraps to the
        // same bit pattern, which reinterpreted as ulong is the correct |MinValue|.
        var abs = number < 0 ? unchecked((ulong)-number) : (ulong)number;
        return (int)(uint)Math.Log10(abs) + 1;
    }

    /// <summary>
    /// Computes a 64-bit hash code that is stable across application runs and process
    /// architectures.
    /// </summary>
    /// <param name="number">The value.</param>
    public static ulong GetRepeatableHashCode(this long number)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(number);
    }
}

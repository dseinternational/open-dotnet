// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open;

/// <summary>
/// Extension methods for <see cref="int"/>.
/// </summary>
public static class Int32Extensions
{
    /// <summary>
    /// Reinterprets the value's bit pattern as a <see cref="uint"/>. Negative values wrap
    /// into the upper half of the <see cref="uint"/> range.
    /// </summary>
    public static uint ToUInt32(this int value)
    {
        return (uint)value;
    }

    /// <summary>
    /// Converts the value to a <see cref="uint"/>, throwing <see cref="OverflowException"/>
    /// when <paramref name="value"/> is negative.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is less than zero.</exception>
    public static uint ToUInt32Checked(this int value)
    {
        checked
        {
            return (uint)value;
        }
    }

    /// <summary>
    /// Gets the number of base-10 digits required to represent the value, ignoring its sign.
    /// Returns <c>1</c> for <c>0</c>.
    /// </summary>
    /// <param name="number">The value.</param>
    public static int GetDigitCount(this int number)
    {
        if (number == 0)
        {
            return 1;
        }

        // Widen to long so that Math.Abs(int.MinValue) does not overflow.
        return (int)(uint)Math.Log10(Math.Abs((long)number)) + 1;
    }

    /// <summary>
    /// Computes a 64-bit hash code that is stable across application runs and process
    /// architectures.
    /// </summary>
    /// <param name="number">The value.</param>
    /// <returns>A repeatable 64-bit hash code for <paramref name="number"/>.</returns>
    public static ulong GetRepeatableHashCode(this int number)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(number);
    }
}

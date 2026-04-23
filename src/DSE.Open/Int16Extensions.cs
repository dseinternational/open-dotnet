// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open;

/// <summary>
/// Extension methods for <see cref="short"/>.
/// </summary>
public static class Int16Extensions
{
    /// <summary>
    /// Reinterprets the value's bit pattern as a <see cref="ushort"/>. Negative values wrap
    /// into the upper half of the <see cref="ushort"/> range.
    /// </summary>
    public static ushort ToUInt16(this short value)
    {
        return (ushort)value;
    }

    /// <summary>
    /// Converts the value to a <see cref="ushort"/>, throwing <see cref="OverflowException"/>
    /// when <paramref name="value"/> is negative.
    /// </summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is less than zero.</exception>
    public static ushort ToUInt16Checked(this short value)
    {
        checked
        {
            return (ushort)value;
        }
    }

    /// <summary>
    /// Gets the number of base-10 digits required to represent the value, ignoring its sign.
    /// Returns <c>1</c> for <c>0</c>.
    /// </summary>
    /// <param name="number">The value.</param>
    public static int GetDigitCount(this short number)
    {
        int magnitude = number < 0 ? -number : number;

        if (magnitude >= 10000)
        {
            return 5;
        }

        if (magnitude >= 1000)
        {
            return 4;
        }

        if (magnitude >= 100)
        {
            return 3;
        }

        if (magnitude >= 10)
        {
            return 2;
        }

        return 1;
    }

    /// <summary>
    /// Computes a 64-bit hash code that is stable across application runs and process
    /// architectures.
    /// </summary>
    /// <param name="number">The value.</param>
    public static ulong GetRepeatableHashCode(this short number)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(number);
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open;

public static class Int64Extensions
{
    public static ulong ToUInt64(this long value)
    {
        return (ulong)value;
    }

    public static ulong ToUInt64Checked(this long value)
    {
        checked
        {
            return (ulong)value;
        }
    }

    /// <summary>
    /// Gets the count of the digits in the value.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int GetDigitCount(this long number)
    {
        // Compute absolute value as ulong to handle long.MinValue, which would
        // overflow Math.Abs. For long.MinValue, unchecked -number wraps to the
        // same bit pattern, which reinterpreted as ulong is the correct |MinValue|.
        var abs = number < 0 ? unchecked((ulong)-number) : (ulong)number;
        return (int)(uint)Math.Log10(abs) + 1;
    }

    public static ulong GetRepeatableHashCode(this long number)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(number);
    }
}

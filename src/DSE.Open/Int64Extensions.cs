// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

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
        return (int)(uint)Math.Log10(Math.Abs(number)) + 1;
    }
}

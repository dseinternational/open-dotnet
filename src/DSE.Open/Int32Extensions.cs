// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class Int32Extensions
{
    public static uint ToUInt32(this int value)
    {
        return (uint)value;
    }

    public static uint ToUInt32Checked(this int value)
    {
        checked
        {
            return (uint)value;
        }
    }

    /// <summary>
    /// Gets the count of the digits in the value.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int GetDigitCount(this int number)
    {
        return (int)(uint)Math.Log10(Math.Abs(number)) + 1;
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open;

public static class Int16Extensions
{
    public static ushort ToUInt16(this short value)
    {
        return (ushort)value;
    }

    public static ushort ToUInt16Checked(this short value)
    {
        checked
        {
            return (ushort)value;
        }
    }

    /// <summary>
    /// Gets the count of the digits in the value.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int GetDigitCount(this short number)
    {
        return (int)(uint)Math.Log10(Math.Abs(number)) + 1;
    }

    public static ulong GetRepeatableHashCode(this short number)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(number);
    }
}

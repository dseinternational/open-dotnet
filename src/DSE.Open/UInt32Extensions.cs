// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open;

public static class UInt32Extensions
{
    /// <summary>
    /// Gets the count of the digits in the value.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int GetDigitCount(this uint number)
    {
        return (int)(uint)Math.Log10(number) + 1;
    }

    public static ulong GetRepeatableHashCode(this uint number)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(number);
    }
}

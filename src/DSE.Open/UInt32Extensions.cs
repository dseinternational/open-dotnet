// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open;

/// <summary>
/// Extension methods for <see cref="uint"/>.
/// </summary>
public static class UInt32Extensions
{
    /// <summary>
    /// Gets the number of base-10 digits required to represent the value. Returns <c>1</c>
    /// for <c>0</c>.
    /// </summary>
    /// <param name="number">The value.</param>
    public static int GetDigitCount(this uint number)
    {
        if (number == 0)
        {
            return 1;
        }

        return (int)(uint)Math.Log10(number) + 1;
    }

    /// <summary>
    /// Computes a 64-bit hash code that is stable across application runs and process
    /// architectures.
    /// </summary>
    /// <param name="number">The value.</param>
    public static ulong GetRepeatableHashCode(this uint number)
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(number);
    }
}

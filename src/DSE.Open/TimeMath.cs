// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class TimeMath
{
    /// <summary>
    /// Returns the latest of two <see cref="DateTime"/> values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static DateTime Max(DateTime a, DateTime b)
    {
        return a >= b ? a : b;
    }

    /// <summary>
    /// Returns the latest of two <see cref="DateTimeOffset"/> values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static DateTimeOffset Max(DateTimeOffset a, DateTimeOffset b)
    {
        return a >= b ? a : b;
    }

    /// <summary>
    /// Returns the longest of two <see cref="TimeSpan"/> values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static TimeSpan Max(TimeSpan a, TimeSpan b)
    {
        return a >= b ? a : b;
    }

    /// <summary>
    /// Returns the earliest of two <see cref="DateTime"/> values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static DateTime Min(DateTime a, DateTime b)
    {
        return a <= b ? a : b;
    }

    /// <summary>
    /// Returns the earliest of two <see cref="DateTimeOffset"/> values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static DateTimeOffset Min(DateTimeOffset a, DateTimeOffset b)
    {
        return a <= b ? a : b;
    }

    /// <summary>
    /// Returns the shortest of two <see cref="TimeSpan"/> values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static TimeSpan Min(TimeSpan a, TimeSpan b)
    {
        return a <= b ? a : b;
    }
}

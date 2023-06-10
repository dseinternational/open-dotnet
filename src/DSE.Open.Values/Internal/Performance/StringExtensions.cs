// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Internal.Performance;

internal static class StringExtensions
{
    public static string ToStringInterned(this object obj)
    {
        Guard.IsNotNull(obj);

        var s = obj.ToString();

        if (s is null)
        {
            ThrowHelper.ThrowInvalidOperationException("ToString() returned null");
        }

        return string.IsInterned(s) ?? s;
    }

    public static string ToStringInterned<T>(this T value)
        where T : struct, IFormattable
    {
        var s = value.ToString(null, null);
        return string.IsInterned(s) ?? s;
    }

    public static string ToStringInterned(this string s) => string.IsInterned(s) ?? s;
}

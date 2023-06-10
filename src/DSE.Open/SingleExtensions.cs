// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open;

public static class SingleExtensions
{
    public static int GetDecimalPlacesCount(this float val)
    {
        var i = 0;

        while (i < 10 && Math.Round(val, i) != val)
        {
            i++;
        }

        return i;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasDecimalPlaces(this float val) => Math.Round(val, 0) != val;
}

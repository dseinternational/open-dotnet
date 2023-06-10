// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open;

public static class DoubleExtensions
{
    public static int GetDecimalPlacesCount(this double val)
    {
        var i = 0;

        while (i < 18 && Math.Round(val, i) != val)
        {
            i++;
        }

        return i;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasDecimalPlaces(this double val) => Math.Round(val, 0) != val;
}

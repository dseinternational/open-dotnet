// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class DecimalExtensions
{
    public static int GetDecimalPlacesCount(this decimal val)
    {
        var i = 0;

        while (i < 30 && Math.Round(val, i) != val)
        {
            i++;
        }

        return i;
    }

    public static bool HasDecimalPlaces(this decimal val)
    {
        return Math.Round(val, 0) != val;
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Drawing;

namespace DSE.Open.Drawing;

public static class SizeFExtensions
{
    public static Orientation GetOrientation(this SizeF size)
    {
        return size.Width > size.Height ? Orientation.Landscape : size.Height > size.Width ? Orientation.Portrait : Orientation.Square;
    }

    public static bool IsLandscape(this SizeF size)
    {
        return size.GetOrientation() == Orientation.Landscape;
    }

    public static bool IsPortrait(this SizeF size)
    {
        return size.GetOrientation() == Orientation.Portrait;
    }

    public static bool IsSquare(this SizeF size)
    {
        return size.GetOrientation() == Orientation.Square;
    }
}

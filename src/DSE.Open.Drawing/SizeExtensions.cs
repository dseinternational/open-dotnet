// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Drawing;

namespace DSE.Open.Drawing;

/// <summary>
/// Extension methods for <see cref="Size"/> to determine visual orientation.
/// </summary>
public static class SizeExtensions
{
    /// <summary>
    /// Returns the <see cref="Orientation"/> of the size based on the ratio of width to height.
    /// </summary>
    public static Orientation GetOrientation(this Size size)
    {
        return size.Width > size.Height ? Orientation.Landscape : size.Height > size.Width ? Orientation.Portrait : Orientation.Square;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the size's width is greater than its height.
    /// </summary>
    public static bool IsLandscape(this Size size)
    {
        return size.GetOrientation() == Orientation.Landscape;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the size's height is greater than its width.
    /// </summary>
    public static bool IsPortrait(this Size size)
    {
        return size.GetOrientation() == Orientation.Portrait;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the size's width is equal to its height.
    /// </summary>
    public static bool IsSquare(this Size size)
    {
        return size.GetOrientation() == Orientation.Square;
    }
}

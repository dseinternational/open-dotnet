// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Drawing;

/// <summary>
/// Describes the visual orientation of a rectangular shape.
/// </summary>
public enum Orientation
{
    /// <summary>
    /// The shape has equal width and height.
    /// </summary>
    Square,

    /// <summary>
    /// The shape's width exceeds its height.
    /// </summary>
    Landscape,

    /// <summary>
    /// The shape's height exceeds its width.
    /// </summary>
    Portrait
}

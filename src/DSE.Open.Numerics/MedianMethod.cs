// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// How to choose the median when the input has an even number of elements.
/// (When the count is odd, the single middle element is always the median
/// regardless of this setting.)
/// </summary>
public enum MedianMethod
{
    /// <summary>Return the arithmetic mean of the two middle elements.</summary>
    MeanOfMiddleTwo,

    /// <summary>Return the smaller of the two middle elements (the lower median).</summary>
    SmallerOfMiddleTwo,

    /// <summary>Return the larger of the two middle elements (the upper median).</summary>
    LargerOfMiddleTwo
}

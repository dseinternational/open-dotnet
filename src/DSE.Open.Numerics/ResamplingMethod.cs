// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public enum ResamplingMethod
{
    /// <summary>
    /// Resample by taking the mean of the values in each new bin.
    /// </summary>
    Mean,

    /// <summary>
    /// Resample by taking the sum of the values in each new bin.
    /// </summary>
    Sum,

    /// <summary>
    /// Resample by taking the first value in each new bin.
    /// </summary>
    First,

    /// <summary>
    /// Resample by taking the last value in each new bin.
    /// </summary>
    Last,

    /// <summary>
    /// Resample by taking the minimum value in each new bin.
    /// </summary>
    Min,

    /// <summary>
    /// Resample by taking the maximum value in each new bin.
    /// </summary>
    Max,
}

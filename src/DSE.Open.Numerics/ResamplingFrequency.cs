// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// The bin width to use when resampling a time series.
/// </summary>
public enum ResamplingFrequency
{
    /// <summary>One bin per calendar day.</summary>
    Daily,

    /// <summary>One bin per calendar week.</summary>
    Weekly,

    /// <summary>One bin per calendar month.</summary>
    Monthly,

    /// <summary>One bin per calendar year.</summary>
    Yearly,
}

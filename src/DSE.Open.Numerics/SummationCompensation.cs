// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// The compensated-summation strategy used when summing floating-point values.
/// Stronger strategies trade throughput for reduced rounding error.
/// </summary>
public enum SummationCompensation
{
    /// <summary>Naive left-to-right summation with no error compensation.</summary>
    None,

    /// <summary>Pairwise (cascade) summation; lower error than naive, very low overhead.</summary>
    Pairwise,

    /// <summary>Kahan–Babuška compensated summation; tracks a running correction term.</summary>
    KahanBabushka,

    /// <summary>Kahan–Babuška–Neumaier compensated summation; handles addends larger than the running sum.</summary>
    KahanBabushkaNeumaier
}

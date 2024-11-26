// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public enum ValueType
{
    /// <summary>
    /// A binary value.
    /// </summary>
    Binary,

    /// <summary>
    /// An unsigned 8-bit integer representing a value on an ordinal scale.
    /// </summary>
    Ordinal,

    /// <summary>
    /// An unsigned 64-bit integer representing a count in the inclusive range
    /// of 0 to 9007199254740991.
    /// </summary>
    Count,

    /// <summary>
    /// A <see cref="Decimal"/> value representing an amount in the inclusive range
    /// of 0 to 999999999999.9999999999999999.
    /// </summary>
    Amount,

    /// <summary>
    /// A <see cref="Decimal"/> value representing a ratio in the inclusive range
    /// of -1.000000000000000000 to 1.000000000000000000.
    /// </summary>
    Ratio,

    /// <summary>
    /// A <see cref="Decimal"/> value representing a frequency in the inclusive range
    /// of 0 to 100.000000000000000000.
    /// </summary>
    Frequency,
}

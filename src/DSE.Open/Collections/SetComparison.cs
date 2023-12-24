// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections;

/// <summary>
/// Specifies a comparison between two sets.
/// </summary>
public enum SetComparison
{
    /// <summary>
    /// The evaluated and specified sets of values are equal.
    /// </summary>
    Equal,

    /// <summary>
    /// The evaluated and specified sets of values are not equal.
    /// </summary>
    NotEqual,

    /// <summary>
    /// The evaluated set of values is a subset of the specified set of values.
    /// </summary>
    Subset,

    /// <summary>
    /// The evaluated set of values is a superset of the specified set of values.
    /// </summary>
    Superset,

    /// <summary>
    /// The evaluated set of values is a subset of the specified set of values.
    /// </summary>
    ProperSubset,

    /// <summary>
    /// The evaluated set of values is a superset of the specified set of values.
    /// </summary>
    ProperSuperset,
}


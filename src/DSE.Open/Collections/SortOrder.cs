// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections;

/// <summary>
/// Specifies the direction in which a sequence should be ordered.
/// </summary>
public enum SortOrder
{
    /// <summary>Smallest to largest (e.g. <c>1, 2, 3</c> or <c>"a", "b", "c"</c>).</summary>
    Ascending,

    /// <summary>Largest to smallest (e.g. <c>3, 2, 1</c> or <c>"c", "b", "a"</c>).</summary>
    Descending,
}

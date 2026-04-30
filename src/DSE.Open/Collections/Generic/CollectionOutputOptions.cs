// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

/// <summary>
/// Options that control the output produced by <see cref="CollectionWriter"/>.
/// </summary>
public class CollectionOutputOptions
{
    /// <summary>
    /// The default options instance.
    /// </summary>
    public static readonly CollectionOutputOptions Default = new();

    /// <summary>
    /// Gets or sets the initial capacity of the underlying string buffer used during writing.
    /// </summary>
    public int InitialOutputCapacity { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of elements to include in the output. Must be non-negative.
    /// </summary>
    public int MaximumCount { get; set => field = Ensure.EqualOrGreaterThan(value, 0); } = int.MaxValue;

    /// <summary>
    /// Gets or sets a value indicating whether non-numeric values should be wrapped in double quotes.
    /// </summary>
    public bool QuoteNonNumericValues { get; set; } = true;
}

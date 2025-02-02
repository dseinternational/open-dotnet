// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Collections.Generic;

public class CollectionOutputOptions
{
    public static readonly CollectionOutputOptions Default = new();

    public int InitialOutputCapacity { get; set; }

    public int MaximumCount { get; set => field = Ensure.EqualOrGreaterThan(value, 0); } = int.MaxValue;

    public bool QuoteNonNumericValues { get; set; } = true;
}

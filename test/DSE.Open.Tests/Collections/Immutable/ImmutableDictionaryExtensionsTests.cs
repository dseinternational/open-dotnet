// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;

namespace DSE.Open.Collections.Immutable;

public class ImmutableDictionaryExtensionsTests
{
    [Fact]
    public void AddOrSetRange()
    {
        var original = new Dictionary<int, string>
            {
                { 1, "One" },
                { 2, "Two" }
            }
        .ToImmutableDictionary();

        var items = new Dictionary<int, string>
        {
                { 2, "TWO" },
                { 3, "THREE" }
            };

        var result = original.AddOrSetRange(items);

        Assert.Equal(3, result.Count);
        Assert.Equal("One", result[1]);
        Assert.Equal("TWO", result[2]);
        Assert.Equal("THREE", result[3]);
    }
}

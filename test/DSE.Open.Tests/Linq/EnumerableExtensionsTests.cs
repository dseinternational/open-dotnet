// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Linq;

namespace DSE.Open.Tests.Linq;

public class EnumerableExtensionsTests
{
    private static readonly int[] s_items = [1, 2, 3];

    [Fact]
    public void TryGetSpan_FromArray_ReturnsSpan()
    {
        IEnumerable<int> source = s_items;

        var success = source.TryGetSpan(out var span);

        Assert.True(success);
        Assert.Equal(3, span.Length);
        Assert.Equal(1, span[0]);
        Assert.Equal(3, span[2]);
    }

    [Fact]
    public void TryGetSpan_FromList_ReturnsSpan()
    {
        IEnumerable<int> source = new List<int>(s_items);

        var success = source.TryGetSpan(out var span);

        Assert.True(success);
        Assert.Equal(3, span.Length);
    }

    [Fact]
    public void TryGetSpan_FromValueCollection_ReturnsSpan()
    {
        IEnumerable<int> source = new ValueCollection<int>(s_items);

        var success = source.TryGetSpan(out var span);

        Assert.True(success);
        Assert.Equal(3, span.Length);
        Assert.Equal(1, span[0]);
        Assert.Equal(3, span[2]);
    }

    [Fact]
    public void TryGetSpan_FromReadOnlyValueCollection_ReturnsSpan()
    {
        IEnumerable<int> source = new ReadOnlyValueCollection<int>(s_items);

        var success = source.TryGetSpan(out var span);

        Assert.True(success);
        Assert.Equal(3, span.Length);
    }

    [Fact]
    public void TryGetSpan_FromUnsupportedSource_ReturnsFalse()
    {
        IEnumerable<int> source = Enumerable.Range(0, 3);

        var success = source.TryGetSpan(out var span);

        Assert.False(success);
        Assert.Equal(0, span.Length);
    }
}

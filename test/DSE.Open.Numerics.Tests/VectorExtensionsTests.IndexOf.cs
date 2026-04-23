// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class VectorExtensionsIndexOfTests
{
    [Fact]
    public void IndexOf_Value_FoundAtFirstOccurrence()
    {
        var vector = Vector.Create([1, 2, 3, 2, 1]);

        Assert.Equal(1, vector.IndexOf(2));
    }

    [Fact]
    public void IndexOf_Value_NotFound_ReturnsNegativeOne()
    {
        var vector = Vector.Create([1, 2, 3]);

        Assert.Equal(-1, vector.IndexOf(99));
    }

    [Fact]
    public void IndexOf_Span_FindsContiguousMatch()
    {
        var vector = Vector.Create([1, 2, 3, 4, 5, 2, 3]);
        ReadOnlySpan<int> needle = [2, 3];

        Assert.Equal(1, vector.IndexOf(needle));
    }

    [Fact]
    public void IndexOf_Span_NotFound_ReturnsNegativeOne()
    {
        var vector = Vector.Create([1, 2, 3]);
        ReadOnlySpan<int> needle = [9, 9];

        Assert.Equal(-1, vector.IndexOf(needle));
    }

    [Fact]
    public void LastIndexOf_Value_FoundAtLastOccurrence()
    {
        var vector = Vector.Create([1, 2, 3, 2, 1]);

        Assert.Equal(3, vector.LastIndexOf(2));
    }

    [Fact]
    public void LastIndexOf_Value_NotFound_ReturnsNegativeOne()
    {
        var vector = Vector.Create([1, 2, 3]);

        Assert.Equal(-1, vector.LastIndexOf(99));
    }

    [Fact]
    public void IndexOf_NullVector_Throws()
    {
        IReadOnlyVector<int> vector = null!;

        _ = Assert.Throws<ArgumentNullException>(() => vector.IndexOf(1));
    }
}

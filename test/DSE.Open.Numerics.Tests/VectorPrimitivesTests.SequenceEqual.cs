// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void SequenceEqual_ReadOnlySpan()
    {
        ReadOnlySpan<int> s1 = [1, 2, 3];
        ReadOnlySpan<int> s2 = [1, 2, 3];
        Assert.True(s1.SequenceEqual(s2));
    }

    [Fact]
    public void SequenceEqual_ReadOnlySpan_False()
    {
        ReadOnlySpan<int> s1 = [1, 2, 3];
        ReadOnlySpan<int> s2 = [1, 2, 4];
        Assert.False(s1.SequenceEqual(s2));
    }
}

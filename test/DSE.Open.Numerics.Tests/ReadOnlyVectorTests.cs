// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class ReadOnlyVectorTests
{
    [Fact]
    public void Init()
    {
        ReadOnlyVector<int> v1 = [1, 2, 3, 4, 5, 6];

        var v2 = new ReadOnlyVector<int>([1, 2, 3, 4, 5, 6]);

        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);

        Assert.True(v1.Span.SequenceEqual(v2.Span));
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class VectorTests
{
    [Fact]
    public void Init()
    {
        Vector<int> v1 = [1, 2, 3, 4, 5, 6];

        var v2 = new Vector<int>([1, 2, 3, 4, 5, 6] );

        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);

        Assert.True(v1.Sequence.SequenceEqual(v2.Sequence));
    }
}

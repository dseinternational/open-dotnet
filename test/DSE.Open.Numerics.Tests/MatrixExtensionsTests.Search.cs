// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class MatrixExtensionsTests
{
    [Fact]
    public void Contains()
    {
        var m = new Matrix<int>( [ [1, 2], [3, 4], [5, 6] ]);
        Assert.True(m.Contains(4));
        Assert.True(m.Contains(5));
        Assert.False(m.Contains(7));
        Assert.False(m.Contains(8));
    }

    [Fact]
    public void ContainsAny()
    {
        var m = new Matrix<int>( [ [1, 2], [3, 4], [5, 6] ]);
        Assert.True(m.ContainsAny([7, 4]));
    }

    [Fact]
    public void IndexOf()
    {
        var m = new Matrix<int>( [ [1, 2], [3, 4], [5, 6] ]);
        var l = m.IndexOf(4);
        Assert.Equal(new MatrixIndex(1,1), l);
    }

}

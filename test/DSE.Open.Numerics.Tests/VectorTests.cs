// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorTests
{
    [Fact]
    public void Init()
    {
        Vector<int> v1 = [1, 2, 3, 4, 5, 6];

        var v2 = new Vector<int>([1, 2, 3, 4, 5, 6]);

        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);

        Assert.True(v1.Memory.SequenceEqual(v2.Memory));
    }

    [Fact]
    public void CreateDefault()
    {
        var v1 = Vector.CreateDefault<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.Memory.SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateZeroes()
    {
        var v1 = Vector.CreateZeroes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.Memory.SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateOnes()
    {
        var v1 = Vector.CreateOnes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.Memory.SequenceEqual([1, 1, 1, 1, 1, 1]));
    }
}

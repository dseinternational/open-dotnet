// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorTests
{

    [Fact]
    public void AddInPlace_Int32_Zeroes_Ones()
    {
        var v1 = Vector.CreateZeroes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        Vector.AddInPlace(v1, v2);
        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void AddInPlace_Nullable_Int32()
    {
        var v1 = Vector.Create<NullableNumber<int>>([1, 2, 3, 4, 5, 6]);
        var v2 = Vector.Create<NullableNumber<int>>([1, 2, 3, 4, 5, 6]);
        var v3 = Vector.Create<NullableNumber<int>>([2, 4, 6, 8, 10, 12]);

        Vector.AddInPlace(v1, v2);
        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);
        Assert.Equal(v1, v3);
    }

    [Fact]
    public void AddInPlace_Nullable_Int32_NullValues()
    {
        var v1 = Vector.Create<NullableNumber<int>>([1, 2, 3, 4, null, 6]);
        var v2 = Vector.Create<NullableNumber<int>>([1, null, 3, 4, 5, 6]);
        var v3 = Vector.Create<NullableNumber<int>>([2, null, 6, 8, null, 12]);

        Vector.AddInPlace(v1, v2);

        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);
        // vectors containing NAs are not equal to any other vector
        Assert.NotEqual(v1, v3);
    }

    [Fact]
    public void AddInPlace_Int32_Ones()
    {
        var v1 = Vector.Create([.. Enumerable.Range(0, 100)]);
        var v2 = Vector.CreateOnes<int>(100);

        Vector.AddInPlace(v1, v2);

        Assert.Equal(100, v1.Length);
        Assert.Equal(100, v2.Length);
        Assert.NotEqual(v1, v2);

        for (var i = 0; i < v1.Length; i++)
        {
            Assert.Equal(v1[i], i + 1);
        }
    }

    [Fact]
    public void AddInPlace_Int32_Nullable_Ones()
    {
        var v1 = Vector.Create<NullableNumber<int>>([.. Enumerable.Range(0, 100)]);
        var v2 = Vector.CreateOnes<NullableNumber<int>>(100);

        Vector.AddInPlace(v1, v2);

        Assert.Equal(100, v1.Length);
        Assert.Equal(100, v2.Length);
        Assert.NotEqual(v1, v2);

        for (var i = 0; i < v1.Length; i++)
        {
            Assert.Equal(v1[i], i + 1);
        }
    }
}

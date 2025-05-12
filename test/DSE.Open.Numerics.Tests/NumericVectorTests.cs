// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorTests
{
    [Fact]
    public void Init()
    {
        Vector<int> v1 = [1, 2, 3, 4, 5, 6];

        var v2 = Vector.CreateNumeric([1, 2, 3, 4, 5, 6]);

        Assert.Equal(6, v1.Length);
        Assert.Equal(6, v2.Length);

        Assert.True(v1.AsSpan().SequenceEqual(v2.AsSpan()));
    }

    [Fact]
    public void CreateDefault()
    {
        var v1 = Vector.CreateNumeric<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.AsSpan().SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateZeroes()
    {
        var v1 = Vector.CreateZeroes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.AsSpan().SequenceEqual(new int[6]));
    }

    [Fact]
    public void CreateOnes()
    {
        var v1 = Vector.CreateOnes<int>(6);
        Assert.Equal(6, v1.Length);
        Assert.True(v1.AsSpan().SequenceEqual([1, 1, 1, 1, 1, 1]));
    }

    [Fact]
    public void Equality()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        Assert.Equal(v1, v2);
    }

    [Fact]
    public void AdditionOperator()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        var v3 = v1 + v2;
        Assert.Equal(6, v3.Length);
        Assert.True(v3.AsSpan().SequenceEqual([2, 2, 2, 2, 2, 2]));
    }

    [Fact]
    public void AdditionOperatorScalar()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = v1 + 1;
        Assert.Equal(6, v2.Length);
        Assert.True(v2.AsSpan().SequenceEqual([2, 2, 2, 2, 2, 2]));
    }

    [Fact]
    public void SubtractionOperator()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = Vector.CreateOnes<int>(6);
        var v3 = v1 - v2;
        Assert.Equal(6, v3.Length);
        Assert.True(v3.AsSpan().SequenceEqual([0, 0, 0, 0, 0, 0]));
    }

    [Fact]
    public void SubtractionOperatorScalar()
    {
        var v1 = Vector.CreateOnes<int>(6);
        var v2 = v1 - 1;
        Assert.Equal(6, v2.Length);
        Assert.True(v2.AsSpan().SequenceEqual([0, 0, 0, 0, 0, 0]));
    }

    [Fact]
    public void AsReadOnly_ShouldReturnReadOnlyNumericVector()
    {
        // Arrange
        var vector = Vector.CreateNumeric([1, 2, 3, 4, 5]);

        // Act
        var readOnlyVector = vector.AsReadOnly();

        // Assert
        Assert.IsType<ReadOnlyNumericVector<int>>(readOnlyVector);
    }
}

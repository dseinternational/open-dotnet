// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    [Fact]
    public void Sum_Int32_100()
    {
        var vector = Vector.CreateOnes<int>(100);
        var sum = vector.Sum();
        Assert.Equal(100, sum);
    }

    [Fact]
    public void Sum_Int32_22202()
    {
        var vector = Vector.Create([2, 200, 2000, 20000]);
        var sum = vector.Sum();
        Assert.Equal(22202, sum);
    }

    [Fact]
    public void Sum_Int16_32202()
    {
        var vector = Vector.Create<short>([2, 200, 2000, 30000]);
        var sum = vector.Sum();
        Assert.Equal(32202, sum);
    }

    [Fact]
    public void Sum_Int16_Overflows()
    {
        var vector = Vector.Create<short>([2, 200, 4000, 30000]);
        var sum = vector.Sum();
        Assert.Equal(-31334, sum);
    }

    [Fact]
    public void Sum_Float32_Max()
    {
        var vector = Vector.Create([2, 200, 4000, float.MaxValue]);
        var sum = vector.Sum();
        Assert.Equal(float.MaxValue, sum);
    }

    [Fact]
    public void SumChecked_Int16_Throws_OverflowException()
    {
        var vector = Vector.Create<short>([2, 200, 4000, 30000]);
        _ = Assert.Throws<OverflowException>(() => vector.SumChecked());
    }

    [Fact]
    public void SumChecked_Float32_Max()
    {
        var vector = Vector.Create([2, 200, 4000, float.MaxValue]);
        var sum = vector.SumChecked();
        Assert.Equal(float.MaxValue, sum);
    }

    [Fact]
    public void SumChecked_Float32_Float64()
    {
        var vector = Vector.Create([2, 200, 4000, float.MaxValue]);
        var sum = vector.SumChecked<float, double>();
        Assert.Equal(3.4028234663852886E+38, sum);
    }
}

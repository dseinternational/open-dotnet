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

    [Fact]
    public void SumChecked_Double_Double_WithNaN_ReturnsNaN()
    {
        // Regression for #363: the general SumChecked<T, TResult> overload
        // previously routed NaN through TResult.CreateChecked(value), which
        // misbehaves when TResult is an integer and was a wasted check for
        // integer T. For floating-point TResult, NaN must propagate via +=.
        ReadOnlySpan<double> span = [1.0, 2.0, double.NaN, 4.0];
        var sum = VectorPrimitives.SumChecked<double, double>(span);
        Assert.True(double.IsNaN(sum));
    }

    [Fact]
    public void SumChecked_Float_Double_WithNaN_ReturnsNaN()
    {
        ReadOnlySpan<float> span = [1.0f, 2.0f, float.NaN, 4.0f];
        var sum = VectorPrimitives.SumChecked<float, double>(span);
        Assert.True(double.IsNaN(sum));
    }

    [Fact]
    public void SumChecked_Double_Int64_WithNaN_Throws()
    {
        // Integer TResult cannot represent NaN — CreateChecked throws
        // OverflowException. Documented behaviour.
        double[] values = [1.0, 2.0, double.NaN, 4.0];
        _ = Assert.Throws<OverflowException>(
            () => VectorPrimitives.SumChecked<double, long>(values));
    }
}

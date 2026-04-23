// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesPrimitivesTests
{
    // -------- Unary transforms: Name is preserved --------

    [Fact]
    public void Abs_PreservesName()
    {
        var x = new Series<int>([-1, 2, -3]) { Name = "price" };
        var result = x.Abs();
        Assert.Equal("price", result.Name);
    }

    [Fact]
    public void Negate_PreservesName()
    {
        var x = new Series<int>([1, 2, 3]) { Name = "price" };
        var result = x.Negate();
        Assert.Equal("price", result.Name);
    }

    [Fact]
    public void Sqrt_PreservesName()
    {
        var x = new Series<double>([1, 4, 9]) { Name = "area" };
        var result = x.Sqrt();
        Assert.Equal("area", result.Name);
    }

    [Fact]
    public void Log_Then_Exp_PreservesName()
    {
        var x = new Series<double>([1.0, 2.0, 4.0]) { Name = "volume" };
        var result = x.Log().Exp();
        Assert.Equal("volume", result.Name);
    }

    [Fact]
    public void Sigmoid_PreservesName()
    {
        var x = new Series<double>([-1.0, 0.0, 1.0]) { Name = "logits" };
        var result = x.Sigmoid();
        Assert.Equal("logits", result.Name);
    }

    [Fact]
    public void SoftMax_PreservesName()
    {
        var x = new Series<double>([1.0, 2.0, 3.0]) { Name = "logits" };
        var result = x.SoftMax();
        Assert.Equal("logits", result.Name);
    }

    [Fact]
    public void Round_PreservesName()
    {
        var x = new Series<double>([1.4, 2.6, 3.5]) { Name = "value" };
        var result = x.Round();
        Assert.Equal("value", result.Name);
    }

    [Fact]
    public void Sin_PreservesName()
    {
        var x = new Series<double>([0.0, Math.PI / 2]) { Name = "theta" };
        var result = x.Sin();
        Assert.Equal("theta", result.Name);
    }

    [Fact]
    public void Tanh_PreservesName()
    {
        var x = new Series<double>([-1.0, 0.0, 1.0]) { Name = "activations" };
        var result = x.Tanh();
        Assert.Equal("activations", result.Name);
    }

    [Fact]
    public void DegreesToRadians_PreservesName()
    {
        var x = new Series<double>([0.0, 90.0, 180.0]) { Name = "angle" };
        var result = x.DegreesToRadians();
        Assert.Equal("angle", result.Name);
    }

    [Fact]
    public void Clamp_PreservesName()
    {
        var x = new Series<int>([-1, 5, 11]) { Name = "reading" };
        var result = x.Clamp(0, 10);
        Assert.Equal("reading", result.Name);
    }

    [Fact]
    public void Add_Scalar_PreservesName()
    {
        var x = new Series<int>([1, 2, 3]) { Name = "counts" };
        var result = x.Add(10);
        Assert.Equal("counts", result.Name);
    }

    [Fact]
    public void Pow_Scalar_PreservesName()
    {
        var x = new Series<double>([2.0, 3.0, 4.0]) { Name = "side" };
        var result = x.Pow(2.0);
        Assert.Equal("side", result.Name);
    }

    // -------- Binary transforms: LHS.Name wins --------

    [Fact]
    public void Add_UsesLhsName()
    {
        var x = new Series<int>([1, 2, 3]) { Name = "price" };
        var y = new Series<int>([4, 5, 6]) { Name = "discount" };
        var result = x.Add(y);
        Assert.Equal("price", result.Name);
    }

    [Fact]
    public void Subtract_UsesLhsName()
    {
        var x = new Series<int>([10, 20, 30]) { Name = "total" };
        var y = new Series<int>([1, 2, 3]) { Name = "tax" };
        var result = x.Subtract(y);
        Assert.Equal("total", result.Name);
    }

    [Fact]
    public void Multiply_UsesLhsName()
    {
        var x = new Series<int>([1, 2, 3]) { Name = "count" };
        var y = new Series<int>([4, 4, 4]) { Name = "multiplier" };
        var result = x.Multiply(y);
        Assert.Equal("count", result.Name);
    }

    [Fact]
    public void Divide_UsesLhsName()
    {
        var x = new Series<int>([10, 20, 30]) { Name = "sum" };
        var y = new Series<int>([2, 4, 5]) { Name = "divisor" };
        var result = x.Divide(y);
        Assert.Equal("sum", result.Name);
    }

    [Fact]
    public void Pow_VectorVector_UsesLhsName()
    {
        var x = new Series<double>([2.0, 3.0]) { Name = "base" };
        var y = new Series<double>([2.0, 2.0]) { Name = "exp" };
        var result = x.Pow(y);
        Assert.Equal("base", result.Name);
    }

    [Fact]
    public void CopySign_VectorVector_UsesLhsName()
    {
        var x = new Series<int>([1, 2, 3]) { Name = "magnitude" };
        var sign = new Series<int>([-1, 1, -1]) { Name = "sign" };
        var result = x.CopySign(sign);
        Assert.Equal("magnitude", result.Name);
    }

    // -------- Type-changing transforms: Name is carried across type boundary --------

    [Fact]
    public void GreaterThan_Series_Scalar_PreservesName()
    {
        var x = new Series<int>([1, 5, 10]) { Name = "score" };
        var result = x.GreaterThan(3);
        Assert.Equal("score", result.Name);
    }

    [Fact]
    public void GreaterThan_SeriesSeries_UsesLhsName()
    {
        var x = new Series<int>([1, 2, 3]) { Name = "a" };
        var y = new Series<int>([0, 2, 4]) { Name = "b" };
        var result = x.GreaterThan(y);
        Assert.Equal("a", result.Name);
    }

    [Fact]
    public void GreaterThan_Scalar_Series_UsesRhsName()
    {
        // LHS is a scalar; the only Series is y, so its Name is carried.
        var y = new Series<int>([0, 1, 2]) { Name = "y" };
        var result = 1.GreaterThan<int>(y);
        Assert.Equal("y", result.Name);
    }

    [Fact]
    public void LessThanOrEqual_SeriesScalar_PreservesName()
    {
        var x = new Series<int>([1, 5, 10]) { Name = "score" };
        var result = x.LessThanOrEqual(5);
        Assert.Equal("score", result.Name);
    }

    [Fact]
    public void Equals_SeriesSeries_UsesLhsName()
    {
        var x = new Series<int>([1, 2, 3]) { Name = "a" };
        var y = new Series<int>([1, 5, 3]) { Name = "b" };
        var result = SeriesPrimitives.Equals(x, y);
        Assert.Equal("a", result.Name);
    }

    [Fact]
    public void Equals_SeriesScalar_PreservesName()
    {
        var x = new Series<int>([1, 2, 3]) { Name = "x" };
        var result = SeriesPrimitives.Equals(x, 2);
        Assert.Equal("x", result.Name);
    }

    [Fact]
    public void Sign_PreservesName_Into_IntSeries()
    {
        var x = new Series<int>([-5, 0, 3]) { Name = "delta" };
        var result = x.Sign();
        Assert.Equal("delta", result.Name);
    }

    // -------- Categories / ValueLabels drop on transforms --------

    [Fact]
    public void Abs_Drops_Categories()
    {
        var categories = new CategorySet<int>([1, 2, 3]);
        var x = new Series<int>([1, 2, 3], name: "x", categories: categories);

        var result = x.Abs();

        Assert.False(result.IsCategorical);
    }

    [Fact]
    public void Add_Drops_ValueLabels()
    {
        var labels = new ValueLabelCollection<int>();
        labels.Add(1, "one");
        var x = new Series<int>([1, 2, 3], name: "x", valueLabels: labels);

        var result = x.Add(10);

        Assert.False(result.HasValueLabels);
    }

    [Fact]
    public void GreaterThan_Drops_Categories()
    {
        // Categories live on T, not bool, so they can never carry across the
        // type boundary. This makes the property test almost trivial, but it
        // documents the invariant at the test layer.
        var categories = new CategorySet<int>([1, 2, 3]);
        var x = new Series<int>([1, 2, 3], name: "x", categories: categories);

        var result = x.GreaterThan(1);

        Assert.False(result.IsCategorical);
    }

    // -------- Chained transforms preserve Name through the pipeline --------

    [Fact]
    public void Chained_Pipeline_Preserves_Name()
    {
        var x = new Series<double>([4.0, 9.0, 16.0]) { Name = "sample" };

        var result = x.Sqrt().Add(1.0).Abs();

        Assert.Equal("sample", result.Name);
    }
}

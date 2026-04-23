// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SeriesPrimitivesTests
{
    // -------- Parity: LessThanOrEqual + Equals family --------

    [Fact]
    public void LessThanOrEqual_SeriesSeries_ReturnsMask()
    {
        Series<int> x = [1, 2, 3, 4];
        Series<int> y = [2, 2, 2, 2];
        var mask = x.LessThanOrEqual(y);
        Assert.Equal(new Series<bool>([true, true, false, false]), mask);
    }

    [Fact]
    public void LessThanOrEqual_SeriesScalar_ReturnsMask()
    {
        Series<int> x = [1, 2, 3, 4];
        var mask = x.LessThanOrEqual(2);
        Assert.Equal(new Series<bool>([true, true, false, false]), mask);
    }

    [Fact]
    public void LessThanOrEqual_ScalarSeries_ReturnsMask()
    {
        Series<int> y = [1, 2, 3, 4];
        var mask = 2.LessThanOrEqual<int>(y);
        Assert.Equal(new Series<bool>([false, true, true, true]), mask);
    }

    [Fact]
    public void Equals_SeriesSeries_ReturnsMask()
    {
        Series<int> x = [1, 2, 3];
        Series<int> y = [1, 5, 3];
        var mask = SeriesPrimitives.Equals(x, y);
        Assert.Equal(new Series<bool>([true, false, true]), mask);
    }

    [Fact]
    public void Equals_SeriesScalar_ReturnsMask()
    {
        Series<int> x = [1, 2, 3, 2];
        var mask = SeriesPrimitives.Equals(x, 2);
        Assert.Equal(new Series<bool>([false, true, false, true]), mask);
    }

    [Fact]
    public void EqualsAll_Identical_ReturnsTrue()
    {
        Series<int> x = [1, 2, 3];
        Series<int> y = [1, 2, 3];
        Assert.True(x.EqualsAll(y));
    }

    [Fact]
    public void EqualsAny_OneMatch_ReturnsTrue()
    {
        Series<int> x = [1, 2, 3];
        Series<int> y = [9, 2, 7];
        Assert.True(x.EqualsAny(y));
    }

    // -------- Abs / Negate --------

    [Fact]
    public void Abs_Series_ReturnsAbsoluteValues()
    {
        Series<int> x = [-3, -1, 0, 1, 3];
        var result = x.Abs();
        Assert.Equal(new Series<int>([3, 1, 0, 1, 3]), result);
    }

    [Fact]
    public void Negate_Series_ReturnsNegatedValues()
    {
        Series<int> x = [1, -2, 3];
        var result = x.Negate();
        Assert.Equal(new Series<int>([-1, 2, -3]), result);
    }

    // -------- Reductions --------

    [Fact]
    public void Min_Max_Mean_Series()
    {
        Series<int> x = [5, 3, 8, 1, 4];
        Assert.Equal(1, x.Min());
        Assert.Equal(8, x.Max());
    }

    [Fact]
    public void Mean_Series_Double()
    {
        Series<double> x = [1.0, 2.0, 3.0, 4.0, 5.0];
        Assert.Equal(3.0, x.Mean(), 1e-10);
    }

    [Fact]
    public void Product_Series()
    {
        Series<int> x = [2, 3, 5];
        Assert.Equal(30, x.Product());
    }

    [Fact]
    public void SumOfSquares_Series()
    {
        Series<int> x = [1, 2, 3, 4];
        Assert.Equal(30, x.SumOfSquares());
    }

    [Fact]
    public void IndexOfMin_IndexOfMax()
    {
        Series<int> x = [5, 3, 8, 1, 4];
        Assert.Equal(3, x.IndexOfMin());
        Assert.Equal(2, x.IndexOfMax());
    }

    // -------- Statistics --------

    [Fact]
    public void Variance_Series_Double()
    {
        Series<double> x = [1.0, 2.0, 3.0, 4.0, 5.0];
        Assert.Equal(2.5, x.Variance());
    }

    [Fact]
    public void PopulationVariance_Series_Double()
    {
        Series<double> x = [1.0, 2.0, 3.0, 4.0, 5.0];
        Assert.Equal(2.0, x.PopulationVariance());
    }

    [Fact]
    public void StandardDeviation_Series_Double()
    {
        Series<double> x = [1.0, 2.0, 3.0, 4.0, 5.0];
        Assert.Equal(Math.Sqrt(2.5), x.StandardDeviation(), 1e-10);
    }

    [Fact]
    public void PopulationStandardDeviation_Series_Double()
    {
        Series<double> x = [1.0, 2.0, 3.0, 4.0, 5.0];
        Assert.Equal(Math.Sqrt(2.0), x.PopulationStandardDeviation(), 1e-10);
    }

    [Fact]
    public void GeometricMean_Series_Double()
    {
        Series<double> x = [1.0, 2.0, 4.0, 8.0];
        Assert.Equal(Math.Pow(64.0, 0.25), x.GeometricMean(), 1e-10);
    }

    [Fact]
    public void HarmonicMean_Series_Double()
    {
        Series<double> x = [1.0, 2.0, 4.0];
        Assert.Equal(12.0 / 7.0, x.HarmonicMean(), 1e-10);
    }

    [Fact]
    public void Median_Series_Odd()
    {
        Series<int> x = [3, 1, 2];
        Assert.Equal(2, x.Median());
    }

    [Fact]
    public void Mode_Series()
    {
        Series<int> x = [1, 2, 2, 3, 3, 3, 4];
        Assert.Equal(3, x.Mode());
    }

    [Fact]
    public void Quantiles_Series_Quartiles()
    {
        Series<double> x = [1.0, 2.0, 3.0, 4.0, 5.0];
        var q = x.Quantiles(4).ToArray();
        Assert.Equal(3, q.Length);
        Assert.Equal(1.5, q[0], 1e-10);
        Assert.Equal(3.0, q[1], 1e-10);
        Assert.Equal(4.5, q[2], 1e-10);
    }

    // -------- Element-wise math --------

    [Fact]
    public void Sqrt_Series_Double()
    {
        Series<double> x = [0, 1, 4, 9, 16];
        var result = x.Sqrt();
        Assert.Equal(new Series<double>([0, 1, 2, 3, 4]), result);
    }

    [Fact]
    public void Log_Then_Exp_RoundTrips()
    {
        Series<double> x = [1.0, 2.0, 4.0, 8.0];
        var back = x.Log().Exp();
        for (var i = 0; i < x.Length; i++)
        {
            Assert.Equal(x[i], back[i], 1e-10);
        }
    }

    [Fact]
    public void Pow_Series_Scalar()
    {
        Series<double> x = [2.0, 3.0, 4.0];
        var result = x.Pow(2.0);
        Assert.Equal(new Series<double>([4.0, 9.0, 16.0]), result);
    }

    // -------- Rounding --------

    [Fact]
    public void Floor_Ceiling_Round_Truncate()
    {
        Series<double> x = [1.4, 1.5, -1.4, -1.5];
        Assert.Equal(new Series<double>([1, 1, -2, -2]), x.Floor());
        Assert.Equal(new Series<double>([2, 2, -1, -1]), x.Ceiling());
        Assert.Equal(new Series<double>([1, 2, -1, -2]), x.Round()); // banker's rounding
        Assert.Equal(new Series<double>([1, 1, -1, -1]), x.Truncate());
    }

    // -------- Clamp / Sign --------

    [Fact]
    public void Clamp_Series_Scalar_Scalar()
    {
        Series<int> x = [-5, 0, 3, 7, 10];
        var result = x.Clamp(0, 6);
        Assert.Equal(new Series<int>([0, 0, 3, 6, 6]), result);
    }

    [Fact]
    public void Sign_Series()
    {
        Series<int> x = [-5, 0, 3];
        var result = x.Sign();
        Assert.Equal(new Series<int>([-1, 0, 1]), result);
    }

    // -------- Linear algebra --------

    [Fact]
    public void Dot_Series()
    {
        Series<double> x = [1.0, 2.0, 3.0];
        Series<double> y = [4.0, 5.0, 6.0];
        Assert.Equal(32.0, x.Dot(y));
    }

    [Fact]
    public void Norm_Series_3_4_5()
    {
        Series<double> x = [3.0, 4.0];
        Assert.Equal(5.0, x.Norm());
    }

    [Fact]
    public void Distance_Series_3_4_5()
    {
        Series<double> x = [0.0, 0.0];
        Series<double> y = [3.0, 4.0];
        Assert.Equal(5.0, x.Distance(y));
    }

    [Fact]
    public void CosineSimilarity_Identical_Returns1()
    {
        Series<double> x = [1.0, 2.0, 3.0];
        Series<double> y = [1.0, 2.0, 3.0];
        Assert.Equal(1.0, x.CosineSimilarity(y), 1e-10);
    }

    [Fact]
    public void HammingDistance_Series()
    {
        Series<int> x = [1, 2, 3, 4];
        Series<int> y = [1, 5, 3, 7];
        Assert.Equal(2, x.HammingDistance(y));
    }

    // -------- Trig / Hyperbolic --------

    [Fact]
    public void Sin_Cos_At_Zero()
    {
        Series<double> x = [0.0];
        Assert.Equal(0.0, x.Sin()[0], 1e-10);
        Assert.Equal(1.0, x.Cos()[0], 1e-10);
    }

    [Fact]
    public void Tanh_AtZero_ReturnsZero()
    {
        Series<double> x = [0.0];
        Assert.Equal(0.0, x.Tanh()[0], 1e-10);
    }

    // -------- Is* predicates --------

    [Fact]
    public void IsFinite_Mixed()
    {
        Series<double> x = [1.0, 0.0, double.PositiveInfinity, double.NaN];
        Span<bool> destination = stackalloc bool[4];
        x.IsFinite(destination);
        Assert.Equal([true, true, false, false], destination);
    }

    [Fact]
    public void IsZero_AllZeros_ReturnsTrue()
    {
        Series<double> x = [0.0, 0.0, 0.0];
        Assert.True(x.IsZeroAll());
    }

    [Fact]
    public void IsInfinity_Mixed()
    {
        Series<double> x = [1.0, double.PositiveInfinity];
        Assert.True(x.IsInfinityAny());
        Assert.False(x.IsInfinityAll());
    }

    // -------- Activations --------

    [Fact]
    public void Sigmoid_AtZero_ReturnsHalf()
    {
        Series<double> x = [0.0];
        Assert.Equal(0.5, x.Sigmoid()[0], 1e-10);
    }

    [Fact]
    public void SoftMax_SumsToOne()
    {
        Series<double> x = [1.0, 2.0, 3.0];
        var result = x.SoftMax();
        var sum = result[0] + result[1] + result[2];
        Assert.Equal(1.0, sum, 1e-10);
    }

    // -------- Degrees/Radians --------

    [Fact]
    public void DegreesToRadians_180_Equals_Pi()
    {
        Series<double> x = [180.0];
        Assert.Equal(Math.PI, x.DegreesToRadians()[0], 1e-10);
    }

    // -------- Bitwise --------

    [Fact]
    public void PopCountTotal_Series_UInt32()
    {
        Series<uint> x = [0b0011u, 0b0111u, 0b1111u];
        Assert.Equal(9L, x.PopCountTotal());
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class VectorPrimitivesTests
{
    // -------- Regression: Add/Subtract/Divide double-call bugfix --------

    [Fact]
    public void Add_VectorVectorSpan_WritesDestinationOnce()
    {
        // Regression: the IReadOnlyVector<T>,IReadOnlyVector<T>,Span<T> overload
        // used to call the underlying Add twice — writing the result correctly but
        // doubling the work. Behaviour check: a sentinel in destination must be
        // overwritten to the correct sum exactly once.
        Vector<int> v1 = [1, 2, 3];
        Vector<int> v2 = [4, 5, 6];
        Span<int> destination = stackalloc int[3];
        destination.Fill(int.MinValue);

        v1.Add(v2, destination);

        Assert.Equal([5, 7, 9], destination);
    }

    [Fact]
    public void Subtract_VectorVectorSpan_WritesDestinationOnce()
    {
        Vector<int> v1 = [10, 20, 30];
        Vector<int> v2 = [1, 2, 3];
        Span<int> destination = stackalloc int[3];

        v1.Subtract(v2, destination);

        Assert.Equal([9, 18, 27], destination);
    }

    [Fact]
    public void Divide_VectorVectorSpan_WritesDestinationOnce()
    {
        Vector<int> v1 = [10, 20, 30];
        Vector<int> v2 = [2, 4, 5];
        Span<int> destination = stackalloc int[3];

        v1.Divide(v2, destination);

        Assert.Equal([5, 5, 6], destination);
    }

    // -------- LessThanOrEqual --------

    [Fact]
    public void LessThanOrEqual_Span_ReturnsMask()
    {
        ReadOnlySpan<int> x = [1, 2, 3, 4];
        ReadOnlySpan<int> y = [2, 2, 2, 2];

        Span<bool> result = VectorPrimitives.LessThanOrEqual(x, y);

        Assert.Equal([true, true, false, false], result);
    }

    [Fact]
    public void LessThanOrEqual_Scalar_ReturnsMask()
    {
        ReadOnlySpan<int> x = [1, 2, 3, 4];

        Span<bool> result = VectorPrimitives.LessThanOrEqual(x, 2);

        Assert.Equal([true, true, false, false], result);
    }

    // -------- Abs / Negate --------

    [Fact]
    public void Abs_Vector_ReturnsAbsoluteValues()
    {
        Vector<int> v = [-3, -1, 0, 1, 3];
        var result = v.Abs();
        Assert.Equal([3, 1, 0, 1, 3], result);
    }

    [Fact]
    public void Negate_Vector_ReturnsNegatedValues()
    {
        Vector<int> v = [1, -2, 3];
        var result = v.Negate();
        Assert.Equal([-1, 2, -3], result);
    }

    // -------- Reductions: SumOfSquares, Product, IndexOfMin/Max --------

    [Fact]
    public void SumOfSquares_Span_ReturnsCorrectValue()
    {
        ReadOnlySpan<int> x = [1, 2, 3, 4];
        Assert.Equal(30, VectorPrimitives.SumOfSquares(x));
    }

    [Fact]
    public void Product_Span_ReturnsCorrectValue()
    {
        ReadOnlySpan<int> x = [2, 3, 5];
        Assert.Equal(30, VectorPrimitives.Product(x));
    }

    [Fact]
    public void IndexOfMin_Span_ReturnsCorrectIndex()
    {
        ReadOnlySpan<int> x = [5, 3, 8, 1, 4];
        Assert.Equal(3, VectorPrimitives.IndexOfMin(x));
    }

    [Fact]
    public void IndexOfMax_Span_ReturnsCorrectIndex()
    {
        ReadOnlySpan<int> x = [5, 3, 8, 1, 4];
        Assert.Equal(2, VectorPrimitives.IndexOfMax(x));
    }

    // -------- Elementwise math: Sqrt, Exp, Log, Pow --------

    [Fact]
    public void Sqrt_Span_ReturnsSquareRoots()
    {
        ReadOnlySpan<double> x = [0, 1, 4, 9, 16];
        Span<double> destination = stackalloc double[5];
        VectorPrimitives.Sqrt(x, destination);
        Assert.Equal([0, 1, 2, 3, 4], destination);
    }

    [Fact]
    public void Log_Then_Exp_RoundTrips()
    {
        ReadOnlySpan<double> x = [1, 2, 4, 8];
        Span<double> log = stackalloc double[4];
        Span<double> back = stackalloc double[4];
        VectorPrimitives.Log(x, log);
        VectorPrimitives.Exp(log, back);
        for (var i = 0; i < x.Length; i++)
        {
            Assert.Equal(x[i], back[i], 1e-10);
        }
    }

    [Fact]
    public void Pow_Span_Scalar_ReturnsPowers()
    {
        ReadOnlySpan<double> x = [2, 3, 4];
        Span<double> destination = stackalloc double[3];
        VectorPrimitives.Pow(x, 2.0, destination);
        Assert.Equal([4, 9, 16], destination);
    }

    // -------- Rounding --------

    [Fact]
    public void Floor_Ceiling_Round_Truncate_WorkAsExpected()
    {
        ReadOnlySpan<double> x = [1.4, 1.5, -1.4, -1.5];
        Span<double> floor = stackalloc double[4];
        Span<double> ceiling = stackalloc double[4];
        Span<double> round = stackalloc double[4];
        Span<double> trunc = stackalloc double[4];

        VectorPrimitives.Floor(x, floor);
        VectorPrimitives.Ceiling(x, ceiling);
        VectorPrimitives.Round(x, round);
        VectorPrimitives.Truncate(x, trunc);

        Assert.Equal([1, 1, -2, -2], floor);
        Assert.Equal([2, 2, -1, -1], ceiling);
        Assert.Equal([1, 2, -1, -2], round); // banker's rounding: .5 → nearest even
        Assert.Equal([1, 1, -1, -1], trunc);
    }

    // -------- Clamp / Sign --------

    [Fact]
    public void Clamp_Span_Scalar_Scalar_ClampsValues()
    {
        ReadOnlySpan<int> x = [-5, 0, 3, 7, 10];
        Span<int> destination = stackalloc int[5];
        VectorPrimitives.Clamp(x, 0, 6, destination);
        Assert.Equal([0, 0, 3, 6, 6], destination);
    }

    [Fact]
    public void Sign_Span_ReturnsSignVector()
    {
        ReadOnlySpan<int> x = [-5, 0, 3];
        Span<int> destination = stackalloc int[3];
        VectorPrimitives.Sign(x, destination);
        Assert.Equal([-1, 0, 1], destination);
    }

    // -------- Linear algebra: Dot / Norm / Distance / CosineSimilarity --------

    [Fact]
    public void Dot_Span_ReturnsDotProduct()
    {
        ReadOnlySpan<double> x = [1, 2, 3];
        ReadOnlySpan<double> y = [4, 5, 6];
        Assert.Equal(32, VectorPrimitives.Dot(x, y));
    }

    [Fact]
    public void Norm_Span_Returns3_4_5()
    {
        ReadOnlySpan<double> x = [3, 4];
        Assert.Equal(5, VectorPrimitives.Norm(x));
    }

    [Fact]
    public void Distance_Span_Returns3_4_5()
    {
        ReadOnlySpan<double> x = [0, 0];
        ReadOnlySpan<double> y = [3, 4];
        Assert.Equal(5, VectorPrimitives.Distance(x, y));
    }

    [Fact]
    public void CosineSimilarity_Identical_Returns1()
    {
        ReadOnlySpan<double> x = [1, 2, 3];
        ReadOnlySpan<double> y = [1, 2, 3];
        Assert.Equal(1.0, VectorPrimitives.CosineSimilarity(x, y), 1e-10);
    }

    [Fact]
    public void CosineSimilarity_Orthogonal_Returns0()
    {
        ReadOnlySpan<double> x = [1, 0];
        ReadOnlySpan<double> y = [0, 1];
        Assert.Equal(0.0, VectorPrimitives.CosineSimilarity(x, y), 1e-10);
    }

    // -------- Stats stubs: PopulationVariance / StdDev / PopulationStdDev --------

    [Fact]
    public void PopulationVariance_Span_Double()
    {
        ReadOnlySpan<double> x = [1, 2, 3, 4, 5];
        // mean = 3, sumSqDev = 10, popvar = 10/5 = 2
        Assert.Equal(2.0, VectorPrimitives.PopulationVariance(x));
    }

    [Fact]
    public void PopulationVariance_WithMean_UsesSuppliedMean()
    {
        ReadOnlySpan<double> x = [1, 2, 3, 4, 5];
        // Σ(xᵢ − 0)² / n = 55/5 = 11
        Assert.Equal(11.0, VectorPrimitives.PopulationVariance(x, mean: 0.0));
    }

    [Fact]
    public void StandardDeviation_Span_Double_MatchesSampleVariance()
    {
        ReadOnlySpan<double> x = [1, 2, 3, 4, 5];
        var stddev = VectorPrimitives.StandardDeviation(x);
        // sample variance = 2.5, stddev = sqrt(2.5)
        Assert.Equal(Math.Sqrt(2.5), stddev, 1e-10);
    }

    [Fact]
    public void PopulationStandardDeviation_Span_Double_MatchesPopulationVariance()
    {
        ReadOnlySpan<double> x = [1, 2, 3, 4, 5];
        var popstddev = VectorPrimitives.PopulationStandardDeviation(x);
        // population variance = 2, popstddev = sqrt(2)
        Assert.Equal(Math.Sqrt(2.0), popstddev, 1e-10);
    }

    // -------- Stats stubs: GeometricMean / HarmonicMean --------

    [Fact]
    public void GeometricMean_Span_Double()
    {
        // Geometric mean of [1, 2, 4, 8] = (1·2·4·8)^(1/4) = 64^0.25 = 2.828...
        ReadOnlySpan<double> x = [1, 2, 4, 8];
        Assert.Equal(Math.Pow(64.0, 0.25), VectorPrimitives.GeometricMean(x), 1e-10);
    }

    [Fact]
    public void HarmonicMean_Span_Double()
    {
        // Harmonic mean of [1, 2, 4] = 3 / (1/1 + 1/2 + 1/4) = 3 / 1.75 = 12/7
        ReadOnlySpan<double> x = [1, 2, 4];
        Assert.Equal(12.0 / 7.0, VectorPrimitives.HarmonicMean(x), 1e-10);
    }

    // -------- Stats stubs: Median / Mode --------

    [Fact]
    public void Median_Span_Odd_ReturnsMiddleValue()
    {
        ReadOnlySpan<int> x = [3, 1, 2];
        Assert.Equal(2, VectorPrimitives.Median(x));
    }

    [Fact]
    public void Median_Span_Even_MeanOfMiddleTwo()
    {
        ReadOnlySpan<int> x = [1, 2, 3, 4];
        Assert.Equal(2, VectorPrimitives.Median(x));
    }

    [Fact]
    public void Median_Span_Even_DoesNotMutateInput()
    {
        int[] arr = [5, 1, 3, 2, 4];
        ReadOnlySpan<int> x = arr;
        _ = VectorPrimitives.Median(x);
        Assert.Equal([5, 1, 3, 2, 4], arr);
    }

    [Fact]
    public void Median_Span_Even_Smaller()
    {
        ReadOnlySpan<int> x = [1, 2, 3, 4];
        Assert.Equal(2, VectorPrimitives.Median(x, MedianMethod.SmallerOfMiddleTwo));
    }

    [Fact]
    public void Median_Span_Even_Larger()
    {
        ReadOnlySpan<int> x = [1, 2, 3, 4];
        Assert.Equal(3, VectorPrimitives.Median(x, MedianMethod.LargerOfMiddleTwo));
    }

    [Fact]
    public void Mode_Span_ReturnsMostFrequent()
    {
        ReadOnlySpan<int> x = [1, 2, 2, 3, 3, 3, 4];
        Assert.Equal(3, VectorPrimitives.Mode(x));
    }

    [Fact]
    public void Mode_Span_TieReturnsFirstToReachPeak()
    {
        // 2 reaches frequency 2 before 1 does → 2 wins the tie.
        ReadOnlySpan<int> x = [1, 2, 2, 1];
        Assert.Equal(2, VectorPrimitives.Mode(x));
    }

    // -------- Stats stubs: Quantiles --------

    [Fact]
    public void Quantiles_Span_Double_Quartiles()
    {
        // Python: statistics.quantiles([1, 2, 3, 4, 5], n=4, method="exclusive")
        //   → [1.5, 3.0, 4.5]
        ReadOnlySpan<double> x = [1, 2, 3, 4, 5];
        var q = VectorPrimitives.Quantiles(x, 4).ToArray();
        Assert.Equal(3, q.Length);
        Assert.Equal(1.5, q[0], 1e-10);
        Assert.Equal(3.0, q[1], 1e-10);
        Assert.Equal(4.5, q[2], 1e-10);
    }

    // -------- Is* predicate family --------

    [Fact]
    public void IsFinite_IsInfinity_IsZero()
    {
        ReadOnlySpan<double> x = [1.0, 0.0, double.PositiveInfinity, double.NegativeInfinity, double.NaN];
        Span<bool> finite = stackalloc bool[5];
        Span<bool> infinity = stackalloc bool[5];
        Span<bool> zero = stackalloc bool[5];

        VectorPrimitives.IsFinite(x, finite);
        VectorPrimitives.IsInfinity(x, infinity);
        VectorPrimitives.IsZero(x, zero);

        Assert.Equal([true, true, false, false, false], finite);
        Assert.Equal([false, false, true, true, false], infinity);
        Assert.Equal([false, true, false, false, false], zero);
    }

    // -------- Activations --------

    [Fact]
    public void Sigmoid_AtZero_ReturnsHalf()
    {
        ReadOnlySpan<double> x = [0.0];
        Span<double> destination = stackalloc double[1];
        VectorPrimitives.Sigmoid(x, destination);
        Assert.Equal(0.5, destination[0], 1e-10);
    }

    [Fact]
    public void SoftMax_SumsToOne()
    {
        ReadOnlySpan<double> x = [1.0, 2.0, 3.0];
        Span<double> destination = stackalloc double[3];
        VectorPrimitives.SoftMax(x, destination);

        var sum = destination[0] + destination[1] + destination[2];
        Assert.Equal(1.0, sum, 1e-10);
    }

    // -------- Bitwise: PopCountTotal --------

    [Fact]
    public void PopCountTotal_Span_UInt32()
    {
        // 0b0011 = 2 bits, 0b0111 = 3 bits, 0b1111 = 4 bits → 9 total
        ReadOnlySpan<uint> x = [0b0011u, 0b0111u, 0b1111u];
        Assert.Equal(9L, VectorPrimitives.PopCountTotal(x));
    }
}

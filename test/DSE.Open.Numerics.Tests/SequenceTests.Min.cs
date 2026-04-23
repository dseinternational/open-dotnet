// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class SequenceTests
{
    [Fact]
    public void Min_Int32_Array_FastPath()
    {
        // Arrays go through the TryGetSpan fast path.
        Assert.Equal(-3, Sequence.Min(new[] { 5, 2, -3, 8, 0 }));
    }

    [Fact]
    public void Min_Int32_EnumerableFallback()
    {
        // HashSet bypasses the TryGetSpan fast path, so the enumerator
        // fallback is exercised.
        IEnumerable<int> source = new HashSet<int> { 5, 2, -3, 8, 0 };
        Assert.Equal(-3, Sequence.Min(source));
    }

    [Fact]
    public void Min_Int32_SingleElement()
    {
        IEnumerable<int> single = Enumerable.Repeat(7, 1);
        Assert.Equal(7, Sequence.Min(single));
    }

    [Fact]
    public void Min_Empty_EnumerableFallback_Throws()
    {
        // HashSet bypasses TryGetSpan's type checks, forcing the
        // enumerator-based fallback path.
        IEnumerable<int> empty = new HashSet<int>();
        _ = Assert.Throws<InvalidOperationException>(() => Sequence.Min(empty));
    }

    [Fact]
    public void MinFloatingPoint_Double_ReturnsSmallest()
    {
        IEnumerable<double> source = new HashSet<double> { 10.0, 9.0, 8.0, 7.0, 6.0 };
        Assert.Equal(6.0, source.MinFloatingPoint());
    }

    [Fact]
    public void MinFloatingPoint_NaNPresent_ReturnsNaN()
    {
        IEnumerable<double> source = new HashSet<double> { 1.0, 2.0, double.NaN, 4.0 };
        Assert.True(double.IsNaN(source.MinFloatingPoint()));
    }

    [Fact]
    public void MinFloatingPoint_Empty_ReturnsNaN()
    {
        // HashSet bypasses the TryGetSpan fast path; the enumerator fallback
        // returns NaN for an empty input.
        IEnumerable<double> empty = new HashSet<double>();
        Assert.True(double.IsNaN(empty.MinFloatingPoint()));
    }
}

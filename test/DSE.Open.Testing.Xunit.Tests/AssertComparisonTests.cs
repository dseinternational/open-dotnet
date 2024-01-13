// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public partial class AssertSequenceTests
{
    [Fact]
    public void GreaterThan()
    {
        AssertComparison.GreaterThan(1, 100);
    }

    [Fact]
    public void GreaterThan_throws_if_not()
    {
        _ = Assert.Throws<ComparisonException>(() => AssertComparison.GreaterThan(1, 1));
        _ = Assert.Throws<ComparisonException>(() => AssertComparison.GreaterThan(100, 1));
    }

    [Fact]
    public void GreaterThanOrEqual_throws_if_not()
    {
        _ = Assert.Throws<ComparisonException>(() => AssertComparison.GreaterThanOrEqual(1, 0));
        _ = Assert.Throws<ComparisonException>(() => AssertComparison.GreaterThanOrEqual(100, 1));
    }

    [Fact]
    public void LessThan_throws_if_not()
    {
        _ = Assert.Throws<ComparisonException>(() => AssertComparison.LessThan(1, 1));
        _ = Assert.Throws<ComparisonException>(() => AssertComparison.LessThan(100, 1000));
    }

    [Fact]
    public void LessThanOrEqual_throws_if_not()
    {
        _ = Assert.Throws<ComparisonException>(() => AssertComparison.LessThanOrEqual(1, 2));
        _ = Assert.Throws<ComparisonException>(() => AssertComparison.LessThanOrEqual(100, 1000));
    }

    [Fact]
    public void EachGreaterThanPrevious()
    {
        int[] sequence = [1, 2, 3, 4, 5];
        AssertComparison.EachGreaterThanPrevious(sequence);
    }

    [Fact]
    public void EachGreaterThanPrevious_throws_if_not()
    {
        int[] sequence = [1, 2, 3, 4, 5, 4];
        _ = Assert.Throws<ComparisonException>(() => AssertComparison.EachGreaterThanPrevious(sequence));
    }
}

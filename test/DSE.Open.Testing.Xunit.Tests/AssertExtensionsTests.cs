// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public class AssertExtensionsTests
{
    [Fact]
    public void GreaterThan()
    {
        AssertExtensions.GreaterThan(1, 100);
    }

    [Fact]
    public void GreaterThan_throws_if_not()
    {
        _ = Assert.Throws<ComparisonException>(() => AssertExtensions.GreaterThan(1, 1));
        _ = Assert.Throws<ComparisonException>(() => AssertExtensions.GreaterThan(100, 1));
    }

    [Fact]
    public void GreaterThanOrEqual_throws_if_not()
    {
        _ = Assert.Throws<ComparisonException>(() => AssertExtensions.GreaterThanOrEqual(1, 0));
        _ = Assert.Throws<ComparisonException>(() => AssertExtensions.GreaterThanOrEqual(100, 1));
    }

    [Fact]
    public void LessThan_throws_if_not()
    {
        _ = Assert.Throws<ComparisonException>(() => AssertExtensions.LessThan(1, 1));
        _ = Assert.Throws<ComparisonException>(() => AssertExtensions.LessThan(100, 1000));
    }

    [Fact]
    public void LessThanOrEqual_throws_if_not()
    {
        _ = Assert.Throws<ComparisonException>(() => AssertExtensions.LessThanOrEqual(1, 2));
        _ = Assert.Throws<ComparisonException>(() => AssertExtensions.LessThanOrEqual(100, 1000));
    }

    [Fact]
    public void EachGreaterThanPrevious()
    {
        int[] sequence = [1, 2, 3, 4, 5];
        AssertExtensions.EachGreaterThanPrevious(sequence);
    }

    [Fact]
    public void EachGreaterThanPrevious_throws_if_not()
    {
        int[] sequence = [1, 2, 3, 4, 5, 4];
        _ = Assert.Throws<ComparisonException>(() => AssertExtensions.EachGreaterThanPrevious(sequence));
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious()
    {
        int[] sequence = [1, 2, 3, 4, 5];
        AssertExtensions.EachGreaterThanOrEqualToPrevious(sequence);

        int[] sequence2 = [5, 5, 5, 5];
        AssertExtensions.EachGreaterThanOrEqualToPrevious(sequence2);
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious_throws_if_not()
    {
        int[] sequence = [1, 1, 3, 4, 3];
        _ = Assert.Throws<ComparisonException>(() => AssertExtensions.EachGreaterThanOrEqualToPrevious(sequence));
    }
}

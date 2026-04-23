// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public class AssertSequenceMemoryTests
{
    private static readonly int[] s_twoInts = [1, 2];
    private static readonly int[] s_oneInt = [42];
    private static readonly int[] s_twoOnePair = [1, 2];

    [Fact]
    public void True_Memory_Success()
    {
        AssertSequence.True<int>(s => s.Length > 0, s_twoInts.AsMemory());
    }

    [Fact]
    public void True_Memory_Throws_When_Predicate_False()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.True<int>(s => s.Length > 10, s_twoInts.AsMemory()));
    }

    [Fact]
    public void True_Memory_NullAssertion_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.True<int>(null!, s_twoInts.AsMemory()));
    }

    [Fact]
    public void TrueForAll()
    {
        AssertSequence.TrueForAll<int>(v => v < 101, Enumerable.Range(1, 100).ToArray().AsMemory());
        AssertSequence.TrueForAll(v => v < 101, Enumerable.Range(1, 100).ToArray().AsSpan());
    }

    [Fact]
    public void TrueForAll_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
            AssertSequence.TrueForAll<int>(v => v < 50, Enumerable.Range(1, 100).ToArray().AsMemory()));
        _ = Assert.Throws<SequenceException>(() =>
        {
            AssertSequence.TrueForAll(v => v < 50, Enumerable.Range(1, 100).ToArray().AsSpan());
        });
    }

    [Fact]
    public void TrueForAny()
    {
        AssertSequence.TrueForAny<int>(v => v > 99, Enumerable.Range(1, 100).ToArray().AsMemory());
        AssertSequence.TrueForAny(v => v > 99, Enumerable.Range(1, 100).ToArray().AsSpan());
    }

    [Fact]
    public void TrueForAny_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
            AssertSequence.TrueForAny<int>(v => v > 1000, Enumerable.Range(1, 100).ToArray().AsMemory()));
        _ = Assert.Throws<SequenceException>(() =>
        {
            AssertSequence.TrueForAny(v => v > 1000, Enumerable.Range(1, 100).ToArray().AsSpan());
        });
    }

    [Fact]
    public void Empty()
    {
        AssertSequence.Empty<int>(Enumerable.Empty<int>().ToArray().AsMemory());
        AssertSequence.Empty(Enumerable.Empty<int>().ToArray().AsSpan());
    }

    [Fact]
    public void Empty_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
            AssertSequence.Empty<int>(Enumerable.Range(1, 10).ToArray().AsMemory()));
        _ = Assert.Throws<SequenceException>(() =>
        {
            AssertSequence.Empty(Enumerable.Range(1, 10).ToArray().AsSpan());
        });
    }

    [Fact]
    public void NotEmpty()
    {
        AssertSequence.NotEmpty<int>(Enumerable.Range(1, 10).ToArray().AsMemory());
        AssertSequence.NotEmpty(Enumerable.Range(1, 10).ToArray().AsSpan());
    }

    [Fact]
    public void NotEmpty_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
            AssertSequence.NotEmpty<int>(Enumerable.Empty<int>().ToArray().AsMemory()));
        _ = Assert.Throws<SequenceException>(() =>
        {
            AssertSequence.NotEmpty(Enumerable.Empty<int>().ToArray().AsSpan());
        });
    }

    [Fact]
    public void CountEqual()
    {
        AssertSequence.CountEqual<int>(10, Enumerable.Range(1, 10).ToArray().AsMemory());
        AssertSequence.CountEqual(10, Enumerable.Range(1, 10).ToArray().AsSpan());
    }

    [Fact]
    public void CountEqual_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
            AssertSequence.CountEqual<int>(10, Enumerable.Range(1, 3).ToArray().AsMemory()));
        _ = Assert.Throws<SequenceException>(() =>
        {
            AssertSequence.CountEqual(10, Enumerable.Range(1, 3).ToArray().AsSpan());
        });
    }

    [Fact]
    public void CountGreaterThan()
    {
        AssertSequence.CountGreaterThan<int>(9, Enumerable.Range(1, 10).ToArray().AsMemory());
        AssertSequence.CountGreaterThan(9, Enumerable.Range(1, 10).ToArray().AsSpan());
    }

    [Fact]
    public void CountGreaterThan_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
            AssertSequence.CountGreaterThan<int>(10, Enumerable.Range(1, 10).ToArray().AsMemory()));
        _ = Assert.Throws<SequenceException>(() =>
        {
            AssertSequence.CountGreaterThan(10, Enumerable.Range(1, 10).ToArray().AsSpan());
        });
    }

    [Fact]
    public void SingleElement_Memory()
    {
        AssertSequence.SingleElement<int>(s_oneInt.AsMemory());
    }

    [Fact]
    public void SingleElement_Memory_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
            AssertSequence.SingleElement<int>(s_twoOnePair.AsMemory()));
    }

    [Fact]
    public void SingleElement_Span()
    {
        AssertSequence.SingleElement<int>([42]);
    }

    [Fact]
    public void SingleElement_Span_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
        {
            ReadOnlySpan<int> s = [1, 2];
            AssertSequence.SingleElement(s);
        });
    }

    [Fact]
    public void AllZero()
    {
        AssertSequence.AllZero<long>(Enumerable.Range(1, 10).Select(i => 0L).ToArray().AsMemory());
        AssertSequence.AllZero<double>(Enumerable.Range(1, 10).Select(i => 0.0).ToArray().AsMemory());
        AssertSequence.AllZero<decimal>(Enumerable.Range(1, 10).Select(i => 0m).ToArray().AsMemory());
        AssertSequence.AllZero(Enumerable.Range(1, 10).Select(i => 0L).ToArray().AsSpan());
        AssertSequence.AllZero(Enumerable.Range(1, 10).Select(i => 0.0).ToArray().AsSpan());
        AssertSequence.AllZero(Enumerable.Range(1, 10).Select(i => 0m).ToArray().AsSpan());
    }

    [Fact]
    public void AllZero_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
            AssertSequence.AllZero<long>(new long[] { 0, 0, 1, 0 }.AsMemory()));
    }

    [Fact]
    public void EachGreaterThanPrevious()
    {
        ReadOnlySpan<int> sequence = [1, 2, 3, 4, 5];
        AssertSequence.EachGreaterThanPrevious(sequence);

        ReadOnlySpan<int> sequence2 = [10, 12, 14, 20];
        AssertSequence.EachGreaterThanPrevious(sequence2);
    }

    [Fact]
    public void EachGreaterThanPrevious_RespectsExpectedAllAbove()
    {
        ReadOnlySpan<int> sequence = [11, 12, 13];
        AssertSequence.EachGreaterThanPrevious(sequence, 10);
    }

    [Fact]
    public void EachGreaterThanPrevious_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
        {
            ReadOnlySpan<int> sequence = [1, 2, 3, 4, 4];
            AssertSequence.EachGreaterThanPrevious(sequence);
        });
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious()
    {
        ReadOnlySpan<int> sequence = [1, 2, 3, 4, 5];
        AssertSequence.EachGreaterThanOrEqualToPrevious(sequence);

        ReadOnlySpan<int> sequence2 = [5, 5, 5, 5];
        AssertSequence.EachGreaterThanOrEqualToPrevious(sequence2);
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(() =>
        {
            ReadOnlySpan<int> sequence = [1, 1, 3, 4, 3];
            AssertSequence.EachGreaterThanOrEqualToPrevious(sequence);
        });
    }
}

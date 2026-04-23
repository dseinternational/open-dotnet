// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing.Xunit;

public class AssertSequenceEnumerableTests
{
    [Fact]
    public void True_Success()
    {
        AssertSequence.True<int>(s => s.Any(), Enumerable.Range(1, 3));
    }

    [Fact]
    public void True_Throws_When_Predicate_False()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.True<int>(s => !s.Any(), Enumerable.Range(1, 3)));
    }

    [Fact]
    public void True_NullAssertion_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.True<int>(null!, Enumerable.Range(1, 3)));
    }

    [Fact]
    public void True_NullSequence_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.True<int>(s => true, (IEnumerable<int>)null!));
    }

    [Fact]
    public void TrueForAll()
    {
        AssertSequence.TrueForAll(v => v < 101, Enumerable.Range(1, 100));
    }

    [Fact]
    public void TrueForAll_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.TrueForAll(v => v < 50, Enumerable.Range(1, 100)));
    }

    [Fact]
    public void TrueForAll_NullAssertion_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.TrueForAll((Func<int, bool>)null!, Enumerable.Range(1, 1)));
    }

    [Fact]
    public void TrueForAll_NullSequence_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.TrueForAll<int>(v => true, (IEnumerable<int>)null!));
    }

    [Fact]
    public void TrueForAny()
    {
        AssertSequence.TrueForAny(v => v > 99, Enumerable.Range(1, 100));
    }

    [Fact]
    public void TrueForAny_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.TrueForAny(v => v > 1000, Enumerable.Range(1, 100)));
    }

    [Fact]
    public void TrueForAny_NullAssertion_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.TrueForAny((Func<int, bool>)null!, Enumerable.Range(1, 1)));
    }

    [Fact]
    public void TrueForAny_NullSequence_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.TrueForAny<int>(v => true, (IEnumerable<int>)null!));
    }

    [Fact]
    public void Empty()
    {
        AssertSequence.Empty(Enumerable.Empty<int>());
    }

    [Fact]
    public void Empty_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.Empty(Enumerable.Range(1, 3)));
    }

    [Fact]
    public void NotEmpty()
    {
        AssertSequence.NotEmpty(Enumerable.Range(1, 10));
    }

    [Fact]
    public void NotEmpty_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.NotEmpty(Enumerable.Empty<int>()));
    }

    [Fact]
    public void AllZero()
    {
        AssertSequence.AllZero(Enumerable.Range(1, 10).Select(i => 0L));
        AssertSequence.AllZero(Enumerable.Range(1, 10).Select(i => 0.0));
        AssertSequence.AllZero(Enumerable.Range(1, 10).Select(i => 0m));
    }

    [Fact]
    public void AllZero_throws_if_not()
    {
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.AllZero(new long[] { 0, 0, 1, 0 }.AsEnumerable()));
    }

    [Fact]
    public void EachGreaterThanPrevious()
    {
        int[] sequence = [1, 2, 3, 4, 5];
        AssertSequence.EachGreaterThanPrevious(sequence);

        int[] sequence2 = [10, 12, 14, 20];
        AssertSequence.EachGreaterThanPrevious(sequence2);
    }

    [Fact]
    public void EachGreaterThanPrevious_RespectsExpectedAllAbove()
    {
        int[] sequence = [11, 12, 13];
        AssertSequence.EachGreaterThanPrevious(sequence, 10);
    }

    [Fact]
    public void EachGreaterThanPrevious_throws_if_not()
    {
        int[] sequence = [1, 2, 3, 4, 4];
        _ = Assert.Throws<SequenceException>(() => AssertSequence.EachGreaterThanPrevious(sequence));
    }

    [Fact]
    public void EachGreaterThanPrevious_ThrowsWhenFirstValueBelowBound()
    {
        int[] sequence = [5, 10, 20];
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.EachGreaterThanPrevious(sequence, 10));
    }

    [Fact]
    public void EachGreaterThanPrevious_NullSource_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.EachGreaterThanPrevious((IEnumerable<int>)null!));
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious()
    {
        int[] sequence = [1, 2, 3, 4, 5];
        AssertSequence.EachGreaterThanOrEqualToPrevious(sequence);

        int[] sequence2 = [5, 5, 5, 5];
        AssertSequence.EachGreaterThanOrEqualToPrevious(sequence2);
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious_throws_if_not()
    {
        int[] sequence = [1, 1, 3, 4, 3];
        _ = Assert.Throws<SequenceException>(() => AssertSequence.EachGreaterThanOrEqualToPrevious(sequence));
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious_ThrowsWhenFirstValueBelowBound()
    {
        int[] sequence = [5, 10, 20];
        _ = Assert.Throws<SequenceException>(
            () => AssertSequence.EachGreaterThanOrEqualToPrevious(sequence, 10));
    }

    [Fact]
    public void EachGreaterThanOrEqualToPrevious_NullSource_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => AssertSequence.EachGreaterThanOrEqualToPrevious((IEnumerable<int>)null!));
    }
}

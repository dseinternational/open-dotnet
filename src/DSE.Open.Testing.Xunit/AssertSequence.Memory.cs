// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Testing.Xunit;

public static partial class AssertSequence
{
    /// <summary>
    /// Asserts that <paramref name="assertion"/> returns <see langword="true"/> when
    /// applied to <paramref name="sequence"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="assertion"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">The assertion returned
    /// <see langword="false"/>.</exception>
    public static void True<T>(
        Func<ReadOnlyMemory<T>, bool> assertion,
        ReadOnlyMemory<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        ArgumentNullException.ThrowIfNull(assertion);

        if (!assertion(sequence))
        {
            throw new SequenceException($"Sequence assertion failed: {message}");
        }
    }

    /// <summary>
    /// Asserts that <paramref name="assertion"/> returns <see langword="true"/> for every
    /// element in <paramref name="sequence"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="assertion"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">The assertion returned <see langword="false"/>
    /// for at least one element.</exception>
    public static void TrueForAll<T>(
        Func<T, bool> assertion,
        ReadOnlyMemory<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        TrueForAll(assertion, sequence.Span, message);
    }

    /// <summary>
    /// Asserts that <paramref name="assertion"/> returns <see langword="true"/> for every
    /// element in <paramref name="sequence"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="assertion"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">The assertion returned <see langword="false"/>
    /// for at least one element.</exception>
    public static void TrueForAll<T>(
        Func<T, bool> assertion,
        ReadOnlySpan<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        ArgumentNullException.ThrowIfNull(assertion);

        foreach (var value in sequence)
        {
            if (!assertion(value))
            {
                throw new SequenceException($"Expected true for all: {message} (failed for {value})");
            }
        }
    }

    /// <summary>
    /// Asserts that <paramref name="assertion"/> returns <see langword="true"/> for at
    /// least one element in <paramref name="sequence"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="assertion"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">The assertion returned <see langword="false"/>
    /// for every element.</exception>
    public static void TrueForAny<T>(
        Func<T, bool> assertion,
        ReadOnlyMemory<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        TrueForAny(assertion, sequence.Span, message);
    }

    /// <summary>
    /// Asserts that <paramref name="assertion"/> returns <see langword="true"/> for at
    /// least one element in <paramref name="sequence"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="assertion"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">The assertion returned <see langword="false"/>
    /// for every element.</exception>
    public static void TrueForAny<T>(
        Func<T, bool> assertion,
        ReadOnlySpan<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        ArgumentNullException.ThrowIfNull(assertion);

        foreach (var value in sequence)
        {
            if (assertion(value))
            {
                return;
            }
        }

        throw new SequenceException($"Expected true for any: {message}");
    }

    /// <summary>
    /// Asserts that <paramref name="sequence"/> is empty.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> is not empty.</exception>
    public static void Empty<T>(ReadOnlyMemory<T> sequence)
    {
        if (!sequence.IsEmpty)
        {
            throw new SequenceException($"Expected empty sequence");
        }
    }

    /// <summary>
    /// Asserts that <paramref name="sequence"/> is empty.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> is not empty.</exception>
    public static void Empty<T>(ReadOnlySpan<T> sequence)
    {
        if (!sequence.IsEmpty)
        {
            throw new SequenceException($"Expected empty sequence");
        }
    }

    /// <summary>
    /// Asserts that <paramref name="sequence"/> is not empty.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> is empty.</exception>
    public static void NotEmpty<T>(ReadOnlyMemory<T> sequence)
    {
        if (sequence.IsEmpty)
        {
            throw new SequenceException($"Expected non-empty sequence");
        }
    }

    /// <summary>
    /// Asserts that <paramref name="sequence"/> is not empty.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> is empty.</exception>
    public static void NotEmpty<T>(ReadOnlySpan<T> sequence)
    {
        if (sequence.IsEmpty)
        {
            throw new SequenceException($"Expected non-empty sequence");
        }
    }

    /// <summary>
    /// Asserts that <paramref name="sequence"/> contains exactly one element.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> does not contain
    /// exactly one element.</exception>
    public static void SingleElement<T>(ReadOnlyMemory<T> sequence)
    {
        CountEqual(1, sequence);
    }

    /// <summary>
    /// Asserts that <paramref name="sequence"/> contains exactly one element.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> does not contain
    /// exactly one element.</exception>
    public static void SingleElement<T>(ReadOnlySpan<T> sequence)
    {
        CountEqual(1, sequence);
    }

    /// <summary>
    /// Asserts that the count of elements in <paramref name="sequence"/> equals
    /// <paramref name="expectedCount"/>.
    /// </summary>
    /// <exception cref="SequenceException">The count did not match.</exception>
    public static void CountEqual<T>(int expectedCount, ReadOnlyMemory<T> sequence)
    {
        CountEqual(expectedCount, sequence.Span);
    }

    /// <summary>
    /// Asserts that the count of elements in <paramref name="sequence"/> is strictly
    /// greater than <paramref name="expectedCountAbove"/>.
    /// </summary>
    /// <exception cref="SequenceException">The count did not exceed the expected
    /// threshold.</exception>
    public static void CountGreaterThan<T>(int expectedCountAbove, ReadOnlyMemory<T> sequence)
    {
        CountGreaterThan(expectedCountAbove, sequence.Span);
    }

    /// <summary>
    /// Asserts that the count of elements in <paramref name="sequence"/> equals
    /// <paramref name="expectedCount"/>.
    /// </summary>
    /// <exception cref="SequenceException">The count did not match.</exception>
    public static void CountEqual<T>(int expectedCount, ReadOnlySpan<T> sequence)
    {
        if (sequence.Length != expectedCount)
        {
            throw new SequenceException(
                $"Expected sequence count equal to {expectedCount}, was {sequence.Length}");
        }
    }

    /// <summary>
    /// Asserts that the count of elements in <paramref name="sequence"/> is strictly
    /// greater than <paramref name="expectedCountAbove"/>.
    /// </summary>
    /// <exception cref="SequenceException">The count did not exceed the expected
    /// threshold.</exception>
    public static void CountGreaterThan<T>(int expectedCountAbove, ReadOnlySpan<T> sequence)
    {
        if (!(sequence.Length > expectedCountAbove))
        {
            throw new SequenceException(
                $"Expected sequence count above {expectedCountAbove}, was {sequence.Length}");
        }
    }

    /// <summary>
    /// Asserts that every numeric value in <paramref name="sequence"/> equals
    /// <see cref="INumberBase{TSelf}.Zero"/>.
    /// </summary>
    /// <exception cref="SequenceException">A non-zero value was found.</exception>
    public static void AllZero<T>(ReadOnlyMemory<T> sequence)
        where T : struct, INumber<T>
    {
        TrueForAll(v => v == T.Zero, sequence);
    }

    /// <summary>
    /// Asserts that every numeric value in <paramref name="sequence"/> equals
    /// <see cref="INumberBase{TSelf}.Zero"/>.
    /// </summary>
    /// <exception cref="SequenceException">A non-zero value was found.</exception>
    public static void AllZero<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumber<T>
    {
        TrueForAll(v => v == T.Zero, sequence);
    }

    /// <summary>
    /// Asserts that each value in <paramref name="source"/> is strictly greater than the
    /// previous value and that the first value is strictly greater than
    /// <paramref name="expectedAllAbove"/>.
    /// </summary>
    /// <param name="source">The span of values to inspect.</param>
    /// <param name="expectedAllAbove">A lower bound that every value must exceed. Defaults
    /// to <c>default(T)</c>.</param>
    /// <exception cref="SequenceException">A value was not strictly greater than the
    /// previous value (or the lower bound).</exception>
    public static void EachGreaterThanPrevious<T>(ReadOnlySpan<T> source, T expectedAllAbove = default)
        where T : struct, IComparable<T>
    {
        var p = expectedAllAbove;

        try
        {
            foreach (var value in source)
            {
                AssertComparison.GreaterThan(p, value);
                p = value;
            }
        }
        catch (ComparisonException ex)
        {
            throw new SequenceException(
                "Expected each value in sequence to be greater than previous", ex);
        }
    }

    /// <summary>
    /// Asserts that each value in <paramref name="source"/> is greater than or equal to
    /// the previous value and that the first value is greater than or equal to
    /// <paramref name="expectedAllEqualOrAbove"/>.
    /// </summary>
    /// <param name="source">The span of values to inspect.</param>
    /// <param name="expectedAllEqualOrAbove">An inclusive lower bound every value must meet.
    /// Defaults to <c>default(T)</c>.</param>
    /// <exception cref="SequenceException">A value was less than the previous value
    /// (or the lower bound).</exception>
    public static void EachGreaterThanOrEqualToPrevious<T>(ReadOnlySpan<T> source, T expectedAllEqualOrAbove = default)
        where T : struct, IComparable<T>
    {
        var p = expectedAllEqualOrAbove;

        try
        {
            foreach (var value in source)
            {
                AssertComparison.GreaterThanOrEqual(p, value);
                p = value;
            }
        }
        catch (ComparisonException ex)
        {
            throw new SequenceException(
                "Expected each value in sequence to be greater than or equal to previous", ex);
        }
    }
}

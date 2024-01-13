// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Testing.Xunit;

public static partial class AssertSequence
{
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

    public static void TrueForAll<T>(
        Func<T, bool> assertion,
        ReadOnlyMemory<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        TrueForAll(assertion, sequence.Span, message);
    }

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

    public static void TrueForAny<T>(
        Func<T, bool> assertion,
        ReadOnlyMemory<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        TrueForAny(assertion, sequence.Span, message);
    }

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

    public static void Empty<T>(ReadOnlyMemory<T> sequence)
    {
        if (!sequence.IsEmpty)
        {
            throw new SequenceException($"Expected empty sequence");
        }
    }

    public static void Empty<T>(ReadOnlySpan<T> sequence)
    {
        if (!sequence.IsEmpty)
        {
            throw new SequenceException($"Expected empty sequence");
        }
    }

    public static void NotEmpty<T>(ReadOnlyMemory<T> sequence)
    {
        if (sequence.IsEmpty)
        {
            throw new SequenceException($"Expected non-empty sequence");
        }
    }

    public static void NotEmpty<T>(ReadOnlySpan<T> sequence)
    {
        if (sequence.IsEmpty)
        {
            throw new SequenceException($"Expected non-empty sequence");
        }
    }

    public static void SingleElement<T>(ReadOnlyMemory<T> sequence)
    {
        CountEqual(1, sequence);
    }

    public static void CountEqual<T>(int expectedCount, ReadOnlyMemory<T> sequence)
    {
        CountEqual(expectedCount, sequence.Span);
    }

    public static void CountGreaterThan<T>(int expectedCountAbove, ReadOnlyMemory<T> sequence)
    {
        CountGreaterThan(expectedCountAbove, sequence.Span);
    }

    public static void CountEqual<T>(int expectedCount, ReadOnlySpan<T> sequence)
    {
        if (sequence.Length != expectedCount)
        {
            throw new SequenceException(
                $"Expected sequence count equal to {expectedCount}, was {sequence.Length}");
        }
    }

    public static void CountGreaterThan<T>(int expectedCountAbove, ReadOnlySpan<T> sequence)
    {
        if (!(sequence.Length > expectedCountAbove))
        {
            throw new SequenceException(
                $"Expected sequence count above {expectedCountAbove}, was {sequence.Length}");
        }
    }

    public static void AllZero<T>(ReadOnlyMemory<T> sequence)
        where T : struct, INumber<T>
    {
        TrueForAll(v => v == T.Zero, sequence);
    }

    public static void AllZero<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumber<T>
    {
        TrueForAll(v => v == T.Zero, sequence);
    }

    /// <summary>
    /// Asserts that each value in a sequence is greater than the previous value in
    /// the sequence and that the first value is greater than <paramref name="expectedAllEqualOrAbove"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="expectedAllEqualOrAbove"></param>
    /// <exception cref="ComparisonException"></exception>
    public static void EachGreaterThanPrevious<T>(ReadOnlySpan<T> source, T expectedAllEqualOrAbove = default)
        where T : struct, IComparable<T>
    {
        var p = expectedAllEqualOrAbove;

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
                "Expected each value in sequence to be greater than or equal to previous", ex);
        }
    }

    /// <summary>
    /// Asserts that each value in a sequence is greater than or equal to the previous value in
    /// the sequence and that the first value is greater than or equal to <paramref name="expectedAllEqualOrAbove"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="expectedAllEqualOrAbove"></param>
    /// <exception cref="ComparisonException"></exception>
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

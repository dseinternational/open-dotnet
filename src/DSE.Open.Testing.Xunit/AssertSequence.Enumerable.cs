// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Testing.Xunit;

public static partial class AssertSequence
{
    public static void True<T>(
        Func<IEnumerable<T>, bool> assertion,
        IEnumerable<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        ArgumentNullException.ThrowIfNull(assertion);
        ArgumentNullException.ThrowIfNull(sequence);

        if (!assertion(sequence))
        {
            throw new SequenceException($"Sequence assertion failed: {message}");
        }
    }

    public static void TrueForAll<T>(
        Func<T, bool> assertion,
        IEnumerable<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        ArgumentNullException.ThrowIfNull(assertion);
        ArgumentNullException.ThrowIfNull(sequence);

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
        IEnumerable<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        ArgumentNullException.ThrowIfNull(assertion);
        ArgumentNullException.ThrowIfNull(sequence);

        foreach (var value in sequence)
        {
            if (assertion(value))
            {
                return;
            }
        }

        throw new SequenceException($"Expected true for any: {message}");
    }

    public static void Empty<T>(IEnumerable<T> sequence)
    {
        True(s => !s.Any(), sequence);
    }

    public static void NotEmpty<T>(IEnumerable<T> sequence)
    {
        True(s => s.Any(), sequence);
    }

    /// <summary>
    /// Asserts that each value in a sequence is greater than the previous value in
    /// the sequence and that the first value is greater than <paramref name="expectedAllEqualOrAbove"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="expectedAllEqualOrAbove"></param>
    /// <exception cref="ComparisonException"></exception>
    public static void EachGreaterThanPrevious<T>(IEnumerable<T> source, T expectedAllEqualOrAbove = default)
        where T : struct, IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(source);

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
    public static void EachGreaterThanOrEqualToPrevious<T>(IEnumerable<T> source, T expectedAllEqualOrAbove = default)
        where T : struct, IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(source);

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

    public static void AllZero<T>(IEnumerable<T> sequence)
        where T : struct, INumber<T>
    {
        TrueForAll(v => v == T.Zero, sequence);
    }


}

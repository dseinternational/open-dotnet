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
    /// <exception cref="ArgumentNullException"><paramref name="assertion"/> or
    /// <paramref name="sequence"/> is <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">The assertion returned
    /// <see langword="false"/>.</exception>
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

    /// <summary>
    /// Asserts that <paramref name="assertion"/> returns <see langword="true"/> for every
    /// element in <paramref name="sequence"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="assertion"/> or
    /// <paramref name="sequence"/> is <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">The assertion returned <see langword="false"/>
    /// for at least one element.</exception>
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

    /// <summary>
    /// Asserts that <paramref name="assertion"/> returns <see langword="true"/> for at
    /// least one element in <paramref name="sequence"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="assertion"/> or
    /// <paramref name="sequence"/> is <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">The assertion returned <see langword="false"/>
    /// for every element.</exception>
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

    /// <summary>
    /// Asserts that <paramref name="sequence"/> contains no elements.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> is not empty.</exception>
    public static void Empty<T>(IEnumerable<T> sequence)
    {
        True(s => !s.Any(), sequence);
    }

    /// <summary>
    /// Asserts that <paramref name="sequence"/> contains at least one element.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> is empty.</exception>
    public static void NotEmpty<T>(IEnumerable<T> sequence)
    {
        True(s => s.Any(), sequence);
    }

    /// <summary>
    /// Asserts that each value in <paramref name="source"/> is strictly greater than the
    /// previous value and that the first value is strictly greater than
    /// <paramref name="expectedAllAbove"/>.
    /// </summary>
    /// <param name="source">The sequence of values to inspect.</param>
    /// <param name="expectedAllAbove">A lower bound that every value must exceed. Defaults
    /// to <c>default(T)</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">A value was not strictly greater than the
    /// previous value (or the lower bound).</exception>
    public static void EachGreaterThanPrevious<T>(IEnumerable<T> source, T expectedAllAbove = default)
        where T : struct, IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(source);

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
    /// <param name="source">The sequence of values to inspect.</param>
    /// <param name="expectedAllEqualOrAbove">An inclusive lower bound every value must meet.
    /// Defaults to <c>default(T)</c>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">A value was less than the previous value
    /// (or the lower bound).</exception>
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

    /// <summary>
    /// Asserts that every numeric value in <paramref name="sequence"/> equals
    /// <see cref="INumberBase{TSelf}.Zero"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="sequence"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="SequenceException">A non-zero value was found.</exception>
    public static void AllZero<T>(IEnumerable<T> sequence)
        where T : struct, INumber<T>
    {
        TrueForAll(v => v == T.Zero, sequence);
    }
}

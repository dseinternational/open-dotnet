// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Testing.Xunit;

/// <summary>
/// Order-comparison assertions for struct values that implement
/// <see cref="IComparable{T}"/>.
/// </summary>
public static class AssertComparison
{
    /// <summary>
    /// Asserts that <paramref name="comparison"/> returns <see langword="true"/> when
    /// applied to <paramref name="value"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="comparison"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ComparisonException">The comparison returned
    /// <see langword="false"/>.</exception>
    public static void True<T>(
        Func<T, bool> comparison,
        T value,
        [CallerArgumentExpression(nameof(comparison))] string? message = default)
        where T : struct, IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(comparison);

        if (!comparison(value))
        {
            throw new ComparisonException($"Expected comparison failed: {message}");
        }
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is strictly greater than
    /// <paramref name="expectedAbove"/>.
    /// </summary>
    /// <exception cref="ComparisonException">The assertion failed.</exception>
    public static void GreaterThan<T>(T expectedAbove, T value)
        where T : struct, IComparable<T>
    {
        True(v => v.CompareTo(expectedAbove) > 0, value);
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is greater than or equal to
    /// <paramref name="expectedAboveOrEqual"/>.
    /// </summary>
    /// <exception cref="ComparisonException">The assertion failed.</exception>
    public static void GreaterThanOrEqual<T>(T expectedAboveOrEqual, T value)
        where T : struct, IComparable<T>
    {
        True(v => v.CompareTo(expectedAboveOrEqual) >= 0, value);
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is strictly less than
    /// <paramref name="expectedBelow"/>.
    /// </summary>
    /// <exception cref="ComparisonException">The assertion failed.</exception>
    public static void LessThan<T>(T expectedBelow, T value)
        where T : struct, IComparable<T>
    {
        True(v => v.CompareTo(expectedBelow) < 0, value);
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is less than or equal to
    /// <paramref name="expectedBelowOrEqual"/>.
    /// </summary>
    /// <exception cref="ComparisonException">The assertion failed.</exception>
    public static void LessThanOrEqual<T>(T expectedBelowOrEqual, T value)
        where T : struct, IComparable<T>
    {
        True(v => v.CompareTo(expectedBelowOrEqual) <= 0, value);
    }

    /// <summary>
    /// Asserts that each value in <paramref name="source"/> is strictly greater than the
    /// previous value and that the first value is strictly greater than
    /// <paramref name="expectedAllAbove"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is
    /// <see langword="null"/>.</exception>
    /// <exception cref="ComparisonException">The assertion failed.</exception>
    public static void EachGreaterThanPrevious<T>(IEnumerable<T> source, T expectedAllAbove = default)
        where T : struct, IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(source);

        var p = expectedAllAbove;

        foreach (var value in source)
        {
            GreaterThan(p, value);
            p = value;
        }
    }
}

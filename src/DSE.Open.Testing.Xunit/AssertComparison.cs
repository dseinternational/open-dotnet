// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Testing.Xunit;

public static class AssertComparison
{
    /// <summary>
    /// Asserts that a comparison is true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="comparison"></param>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <exception cref="ComparisonException"></exception>
    public static void True<T>(
        Func<T, bool> comparison,
        T value,
        [CallerArgumentExpression(nameof(comparison))] string? message = default)
        where T : struct, IComparable<T>
    {
        Guard.IsNotNull(comparison);

        if (!comparison(value))
        {
            throw new ComparisonException($"Expected comparison failed: {message}");
        }
    }

    /// <summary>
    /// Asserts that the value is greater than an expected value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expectedAbove"></param>
    /// <param name="value"></param>
    /// <exception cref="ComparisonException"></exception>
    public static void GreaterThan<T>(T expectedAbove, T value)
        where T : struct, IComparable<T>
    {
        True(v => v.CompareTo(expectedAbove) > 0, value);
    }

    /// <summary>
    /// Asserts that the value is greater than or equal to an expected value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expectedAboveOrEqual"></param>
    /// <param name="value"></param>
    /// <exception cref="ComparisonException"></exception>
    public static void GreaterThanOrEqual<T>(T expectedAboveOrEqual, T value)
        where T : struct, IComparable<T>
    {
        True(v => v.CompareTo(expectedAboveOrEqual) >= 0, value);
    }

    /// <summary>
    /// Asserts that the value is less than an expected value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expectedBelow"></param>
    /// <param name="value"></param>
    /// <exception cref="ComparisonException"></exception>
    public static void LessThan<T>(T expectedBelow, T value)
        where T : struct, IComparable<T>
    {
        True(v => v.CompareTo(expectedBelow) < 0, value);
    }

    /// <summary>
    /// Asserts that the value is less than or equal to an expected value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expectedBelowOrEqual"></param>
    /// <param name="value"></param>
    /// <exception cref="ComparisonException"></exception>
    public static void LessThanOrEqual<T>(T expectedBelowOrEqual, T value)
        where T : struct, IComparable<T>
    {
        True(v => v.CompareTo(expectedBelowOrEqual) <= 0, value);
    }

    /// <summary>
    /// Asserts that each value in a sequence is greater than than the previous value in
    /// the sequence and that the first value is greater than <paramref name="expectedAllAbove"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="expectedAllAbove"></param>
    /// <exception cref="ComparisonException"></exception>
    public static void EachGreaterThanPrevious<T>(IEnumerable<T> source, T expectedAllAbove = default)
        where T : struct, IComparable<T>
    {
        Guard.IsNotNull(source);

        var p = expectedAllAbove;

        foreach (var value in source)
        {
            GreaterThan(p, value);
            p = value;
        }
    }
}

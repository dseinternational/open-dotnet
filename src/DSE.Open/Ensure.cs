// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open;

/// <summary>
/// Helpers for validating arguments.
/// </summary>
public static class Ensure
{
    #region Structs

    /// <summary>
    /// Ensures the value is not <see langword="null"/> and returns the validated value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T NotNull<T>([NotNull] T? value)
        where T : struct
    {
        if (value is null)
        {
            ThrowHelper.ThrowArgumentNullException();
            return default!; // unreachable
        }

        return value.Value;
    }

    public static T NotDefault<T>(T value)
        where T : struct
    {
        return NotDefault(value, EqualityComparer<T>.Default);
    }

    public static T NotDefault<T>(T value, IEqualityComparer<T> comparer)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(comparer);

        if (comparer.Equals(value, default))
        {
            ThrowHelper.ThrowArgumentNullException();
            return default!; // unreachable
        }

        return value;
    }

    #endregion

    /// <summary>
    /// Ensures the value is not <see langword="null"/> and returns the validated value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the value.</param>
    /// <returns></returns>
    public static T NotNull<T>([NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? name = null)
        where T : class
    {
        if (value is null)
        {
            ThrowHelper.ThrowArgumentNullException(name);
            return default!; // unreachable
        }

        return value;
    }

    #region Strings

    /// <summary>
    /// Ensures the value is not <see langword="null"/> and returns the validated value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the value.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    public static string NotNull([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        ArgumentNullException.ThrowIfNull(value, name);
        return value;
    }

    /// <summary>
    /// Ensures the value is not <see langword="null"/>, empty or contains only whitespace, and returns
    /// the validated value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the value.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="value"/> is empty or contains only whitespace.</exception>
    public static string NotNullOrWhitespace([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        ArgumentNullException.ThrowIfNull(value, name);
        ArgumentException.ThrowIfNullOrWhiteSpace(value, name);
        return value;
    }

    /// <summary>
    /// Ensures the value is at least a minimum length.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="minimumLength"></param>
    /// <param name="allowWhitespace"></param>
    /// <param name="name">The name of the value.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="allowWhitespace"/> is <see langword="false"/> and
    /// <paramref name="value"/> is empty or contains only whitespace; or, the <see cref="string.Length"/> of
    /// <paramref name="value"/> is less than <paramref name="minimumLength"/>.</exception>
    public static string MinimumLength(
        [NotNull] string? value,
        int minimumLength,
        bool allowWhitespace = false,
        [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        ArgumentNullException.ThrowIfNull(value, name);

        if (!allowWhitespace)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, name);
        }

        if (value.Length < minimumLength)
        {
            ThrowHelper.ThrowArgumentException(name, $"Value must be at least {minimumLength} characters long.");
        }

        return value;
    }

    /// <summary>
    /// Ensures the value is at most a maximum length.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maximumLength"></param>
    /// <param name="allowWhitespace"></param>
    /// <param name="name">The name of the value.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="allowWhitespace"/> is <see langword="false"/> and
    /// <paramref name="value"/> is empty or contains only whitespace; or, the <see cref="string.Length"/> of
    /// <paramref name="value"/> is greater than <paramref name="maximumLength"/>.</exception>
    public static string MaximumLength(
        [NotNull] string? value,
        int maximumLength,
        bool allowWhitespace = false,
        [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        ArgumentNullException.ThrowIfNull(value, name);

        if (!allowWhitespace)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, name);
        }

        if (value.Length > maximumLength)
        {
            ThrowHelper.ThrowArgumentException(name, $"Value must be at most {maximumLength} characters long.");
        }

        return value;
    }

    #endregion

    #region IComparable

    /// <summary>
    /// Ensures the value is at least a given minimum.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minimumValue"></param>
    /// <param name="name">The name of the value.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is less than
    /// <paramref name="minimumValue" />.</exception>
    public static T EqualOrGreaterThan<T>(T value, T minimumValue, [CallerArgumentExpression(nameof(value))] string? name = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(minimumValue) < 0)
        {
            ThrowHelper.ThrowArgumentException($"{name} must be equal to or greater than {minimumValue}.", nameof(value));
        }

        return value;
    }

    /// <summary>
    /// Ensures the value is at most a given maximum.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maximumValue"></param>
    /// <param name="name">The name of the value.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is greater than
    /// <paramref name="maximumValue" />.</exception>
    public static T EqualOrLessThan<T>(T value, T maximumValue, [CallerArgumentExpression(nameof(value))] string? name = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(maximumValue) > 0)
        {
            ThrowHelper.ThrowArgumentException($"{name} must be equal to or less than {maximumValue}.", nameof(value));
        }

        return value;
    }

    /// <summary>
    /// Ensures the value is within the a range of values.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="name">The name of the value.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is less than <paramref name="start"/>
    /// or is greater than <paramref name="end" />.</exception>
    public static T InRange<T>(T value, T start, T end, [CallerArgumentExpression(nameof(value))] string? name = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(start) < 0 || value.CompareTo(end) > 0)
        {
            ThrowHelper.ThrowArgumentException($"{name} must within the inclusive range {start} to {end}.", nameof(value));
        }

        return value;
    }

    #endregion
}

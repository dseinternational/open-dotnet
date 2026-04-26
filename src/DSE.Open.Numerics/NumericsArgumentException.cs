// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

/// <summary>
/// The exception thrown by guard helpers in <c>DSE.Open.Numerics</c> when a
/// numeric-input invariant (length match, value-in-set, etc.) is violated.
/// </summary>
/// <remarks>
/// Inherits from <see cref="ArgumentException"/>; callers that catch
/// <see cref="ArgumentException"/> will also catch this type.
/// </remarks>
public class NumericsArgumentException : ArgumentException
{
    private const string DefaultMessage = "Numerics argument error.";

    /// <summary>Creates a new exception with the default message.</summary>
    public NumericsArgumentException()
        : base(DefaultMessage)
    {
    }

    /// <summary>Creates a new exception with the given <paramref name="message"/>.</summary>
    public NumericsArgumentException(string? message)
        : this(message, null, null)
    {
    }

    /// <summary>Creates a new exception with the given <paramref name="message"/> and <paramref name="innerException"/>.</summary>
    public NumericsArgumentException(string? message, Exception? innerException)
        : this(message, null, innerException)
    {
    }

    /// <summary>Creates a new exception with the given <paramref name="message"/> and <paramref name="paramName"/>.</summary>
    public NumericsArgumentException(string? message, string? paramName)
        : this(message, paramName, null)
    {
    }

    /// <summary>Creates a new exception with full context.</summary>
    public NumericsArgumentException(string? message, string? paramName, Exception? innerException)
        : base(message ?? DefaultMessage, paramName, innerException)
    {
    }

    /// <summary>Throws a new <see cref="NumericsArgumentException"/> with the default message.</summary>
    [DoesNotReturn]
    public static void Throw()
    {
        throw new NumericsArgumentException(DefaultMessage);
    }

    /// <summary>Throws a new <see cref="NumericsArgumentException"/> with the given <paramref name="message"/>.</summary>
    [DoesNotReturn]
    public static void Throw(string message)
    {
        throw new NumericsArgumentException(message);
    }

    /// <summary>Throws a new <see cref="NumericsArgumentException"/> with the given <paramref name="message"/> and <paramref name="innerException"/>.</summary>
    [DoesNotReturn]
    public static void Throw(string message, Exception innerException)
    {
        throw new NumericsArgumentException(message, innerException);
    }

    /// <summary>
    /// Throws <see cref="NumericsArgumentException"/> if <paramref name="condition"/>
    /// is <see langword="false"/>; otherwise no-op. The default message is the source
    /// expression of <paramref name="condition"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNot(
        [DoesNotReturnIf(false)] bool condition,
        [CallerArgumentExpression(nameof(condition))] string? message = null)
    {
        if (!condition)
        {
            Throw(message ?? DefaultMessage);
        }
    }

    /// <summary>Throws when the series in <paramref name="collection"/> are not all of equal <see cref="IReadOnlySeries.Length"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public static void ThrowIfNotEqualLength(IReadOnlyCollection<IReadOnlySeries> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count == 0)
        {
            return;
        }

        var length = collection.First().Length;

        foreach (var series in collection)
        {
            if (series.Length != length)
            {
                Throw($"Each collection must have the same length.");
            }
        }
    }

    /// <summary>Throws when the collections in <paramref name="collection"/> are not all of equal count.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public static void ThrowIfNotEqualLength<T>(IReadOnlyCollection<IReadOnlyCollection<T>> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count == 0)
        {
            return;
        }

        var count = collection.First().Count;

        foreach (var series in collection)
        {
            if (series.Count != count)
            {
                Throw($"Each collection must have the same length.");
            }
        }
    }

    /// <summary>Throws when any element of <paramref name="values"/> is not a member of <paramref name="set"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> or <paramref name="set"/> is <see langword="null"/>.</exception>
    public static void ThrowIfNotInSet<T>(IReadOnlyCollection<T> values, IReadOnlySet<T> set)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(set);

        if (values.Count == 0)
        {
            return;
        }

        foreach (var value in values)
        {
            if (!set.Contains(value))
            {
                Throw($"Value {value} is not in the set.");
            }
        }
    }

    /// <summary>Throws when <paramref name="value"/> is not a member of <paramref name="set"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="set"/> is <see langword="null"/>.</exception>
    public static void ThrowIfNotInSet<T>(T value, IReadOnlySet<T> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        if (!set.Contains(value))
        {
            Throw($"Value {value} is not in the set.");
        }
    }

    /// <summary>Throws when <paramref name="x"/> and <paramref name="y"/> are not of equal count.</summary>
    [Obsolete("Use ThrowIfNot(x.Count == y.Count)")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(
        [NotNull] ICollection<T> x,
        [NotNull] ICollection<T> y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Count != y.Count)
        {
            Throw("Collections must be the same length.");
        }
    }

    /// <summary>Throws when <paramref name="x"/> and <paramref name="y"/> are not of equal count.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(
        [NotNull] IReadOnlyCollection<T> x,
        [NotNull] IReadOnlyCollection<T> y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Count != y.Count)
        {
            Throw("Collections must be the same length.");
        }
    }

    /// <summary>Throws when <paramref name="x"/> and <paramref name="y"/> are not of equal length.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(in ReadOnlySpan<T> x, in ReadOnlySpan<T> y)
    {
        if (x.Length != y.Length)
        {
            Throw("Vectors must be the same length.");
        }
    }

    /// <summary>Throws when <paramref name="x"/>'s count and <paramref name="y"/>'s length differ.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(
        [NotNull] ICollection<T> x,
        in ReadOnlySpan<T> y)
    {
        ArgumentNullException.ThrowIfNull(x);

        if (x.Count != y.Length)
        {
            Throw("Collections must be the same length.");
        }
    }

    /// <summary>Throws when <paramref name="x"/>'s count and <paramref name="y"/>'s length differ.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(
        [NotNull] IReadOnlyCollection<T> x,
        in ReadOnlySpan<T> y)
    {
        ArgumentNullException.ThrowIfNull(x);

        if (x.Count != y.Length)
        {
            Throw("Collections must be the same length.");
        }
    }

    /// <summary>Throws when <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> are not all of equal count.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(
        [NotNull] ICollection<T> x,
        [NotNull] ICollection<T> y,
        [NotNull] ICollection<T> z)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(z);

        if (x.Count != y.Count || x.Count != z.Count)
        {
            Throw("Collections must be the same length.");
        }
    }

    /// <summary>Throws when <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> are not all of equal count.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(
        [NotNull] IReadOnlyCollection<T> x,
        [NotNull] IReadOnlyCollection<T> y,
        [NotNull] IReadOnlyCollection<T> z)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        ArgumentNullException.ThrowIfNull(z);

        if (x.Count != y.Count || x.Count != z.Count)
        {
            Throw("Collections must be the same length.");
        }
    }

    /// <summary>Throws when <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> are not all of equal length.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(
        in ReadOnlySpan<T> x,
        in ReadOnlySpan<T> y,
        in ReadOnlySpan<T> z)
    {
        if (x.Length != y.Length || x.Length != z.Length)
        {
            Throw("Spans must be the same length.");
        }
    }

    /// <summary>Throws when <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> are not all of equal count/length.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(
        [NotNull] ICollection<T> x,
        [NotNull] ICollection<T> y,
        in ReadOnlySpan<T> z)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Count != y.Count || x.Count != z.Length)
        {
            Throw("Collections must be the same length.");
        }
    }

    /// <summary>Throws when <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> are not all of equal count/length.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(
        [NotNull] IReadOnlyCollection<T> x,
        [NotNull] IReadOnlyCollection<T> y,
        in ReadOnlySpan<T> z)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Count != y.Count || x.Count != z.Length)
        {
            Throw("Collections must be the same length.");
        }
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public class NumericsArgumentException : ArgumentException
{
    private const string DefaultMessage = "Numerics argument error.";

    public NumericsArgumentException()
        : base(DefaultMessage)
    {
    }

    public NumericsArgumentException(string? message)
        : this(message, null, null)
    {
    }

    public NumericsArgumentException(string? message, Exception? innerException)
        : this(message, null, innerException)
    {
    }

    public NumericsArgumentException(string? message, string? paramName)
        : this(message, paramName, null)
    {
    }

    public NumericsArgumentException(string? message, string? paramName, Exception? innerException)
        : base(message ?? DefaultMessage, paramName, innerException)
    {
    }

    public static void Throw()
    {
        throw new NumericsArgumentException(DefaultMessage);
    }

    public static void Throw(string message)
    {
        throw new NumericsArgumentException(message);
    }

    public static void Throw(string message, Exception innerException)
    {
        throw new NumericsArgumentException(message, innerException);
    }

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

    public static void ThrowIfNotInSet<T>(T value, IReadOnlySet<T> set)
    {
        ArgumentNullException.ThrowIfNull(set);

        if (!set.Contains(value))
        {
            Throw($"Value {value} is not in the set.");
        }
    }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(in ReadOnlySpan<T> x, in ReadOnlySpan<T> y)
    {
        if (x.Length != y.Length)
        {
            Throw("Vectors must be the same length.");
        }
    }

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

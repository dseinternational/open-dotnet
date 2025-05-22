// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

public class NumericsException : Exception
{
    private const string DefaultMessage = "Numerics error.";

    public NumericsException()
        : base(DefaultMessage)
    {
    }

    public NumericsException(string message)
        : base(message ?? DefaultMessage)
    {
    }

    public NumericsException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }

    public static void Throw()
    {
        throw new NumericsException(DefaultMessage);
    }

    public static void Throw(string message)
    {
        throw new NumericsException(message);
    }

    public static void Throw(string message, Exception innerException)
    {
        throw new NumericsException(message, innerException);
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

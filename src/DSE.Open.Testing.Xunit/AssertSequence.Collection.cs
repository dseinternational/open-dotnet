// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

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
        Func<ICollection<T>, bool> assertion,
        ICollection<T> sequence,
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
    /// Asserts that <paramref name="sequence"/> is empty.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> is not empty.</exception>
    public static void Empty<T>(ICollection<T> sequence)
    {
        True(s => s.Count == 0, sequence);
    }

    /// <summary>
    /// Asserts that <paramref name="sequence"/> contains at least one element.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> is empty.</exception>
    public static void NotEmpty<T>(ICollection<T> sequence)
    {
        True(s => s.Count > 0, sequence);
    }

    /// <summary>
    /// Asserts that <paramref name="sequence"/> contains exactly one element.
    /// </summary>
    /// <exception cref="SequenceException"><paramref name="sequence"/> does not contain
    /// exactly one element.</exception>
    public static void SingleElement<T>(ICollection<T> sequence)
    {
        CountEqual(1, sequence);
    }

    /// <summary>
    /// Asserts that <see cref="ICollection{T}.Count"/> of <paramref name="sequence"/>
    /// equals <paramref name="expectedCount"/>.
    /// </summary>
    /// <exception cref="SequenceException">The count did not match.</exception>
    public static void CountEqual<T>(int expectedCount, ICollection<T> sequence)
    {
        True(s => s.Count == expectedCount, sequence);
    }

    /// <summary>
    /// Asserts that <see cref="ICollection{T}.Count"/> of <paramref name="sequence"/> is
    /// strictly greater than <paramref name="expectedCountAbove"/>.
    /// </summary>
    /// <exception cref="SequenceException">The count did not exceed the expected
    /// threshold.</exception>
    public static void CountGreaterThan<T>(int expectedCountAbove, ICollection<T> sequence)
    {
        True(s => s.Count > expectedCountAbove, sequence);
    }
}

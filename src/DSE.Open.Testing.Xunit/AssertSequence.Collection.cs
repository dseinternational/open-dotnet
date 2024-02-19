// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Testing.Xunit;

public static partial class AssertSequence
{
    public static void True<T>(
        Func<ICollection<T>, bool> assertion,
        ICollection<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        Guard.IsNotNull(assertion);
        Guard.IsNotNull(sequence);

        if (!assertion(sequence))
        {
            throw new SequenceException($"Sequence assertion failed: {message}");
        }
    }

    public static void Empty<T>(ICollection<T> sequence)
    {
        True(s => s.Count == 0, sequence);
    }

    public static void NotEmpty<T>(ICollection<T> sequence)
    {
        True(s => s.Count > 0, sequence);
    }

    public static void SingleElement<T>(ICollection<T> sequence)
    {
        CountEqual(1, sequence);
    }

    public static void CountEqual<T>(int expectedCount, ICollection<T> sequence)
    {
        True(s => s.Count == expectedCount, sequence);
    }

    public static void CountGreaterThan<T>(int expectedCountAbove, ICollection<T> sequence)
    {
        True(s => s.Count > expectedCountAbove, sequence);
    }
}

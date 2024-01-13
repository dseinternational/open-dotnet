// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open.Testing.Xunit;

public static class AssertCollection
{
    public static void True<T>(
        Func<ICollection<T>, bool> assertion,
        ICollection<T> sequence,
        [CallerArgumentExpression(nameof(assertion))] string? message = default)
    {
        ArgumentNullException.ThrowIfNull(assertion);
        ArgumentNullException.ThrowIfNull(sequence);

        if (!assertion(sequence))
        {
            throw new CollectionException($"Collection assertion failed: {message}");
        }
    }

    public static void Empty<T>(ICollection<T> sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        True(s => s.Count == 0, sequence);
    }

    public static void NotEmpty<T>(ICollection<T> sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        True(s => s.Count > 0, sequence);
    }

    public static void CountGreaterThan<T>(int expectedCountAbove, ICollection<T> sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        True(s => s.Count > expectedCountAbove, sequence);
    }
}

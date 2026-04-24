// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public class VectorExtensionsMemoryTests
{
    [Fact]
    public void ToVector_FromMemory_ShouldShareBackingMemory()
    {
        int[] values = [1, 2, 3];

        var vector = values.AsMemory().ToVector();
        values[0] = 42;

        Assert.Equal(42, vector[0]);
    }

    [Fact]
    public void ToVector_FromSpan_ShouldCopyValues()
    {
        int[] values = [1, 2, 3];

        var vector = values.AsSpan().ToVector();
        values[0] = 42;

        Assert.Equal(1, vector[0]);
    }

    [Fact]
    public void ToReadOnlyVector_FromReadOnlyMemory_ShouldShareBackingMemory()
    {
        int[] values = [1, 2, 3];
        ReadOnlyMemory<int> memory = values;

        var vector = memory.ToReadOnlyVector();
        values[0] = 42;

        Assert.Equal(42, vector[0]);
    }

    [Fact]
    public void ToReadOnlyVector_FromEmptyReadOnlyMemory_ShouldReturnEmptyVector()
    {
        ReadOnlyMemory<int> memory = ReadOnlyMemory<int>.Empty;

        var vector = memory.ToReadOnlyVector();

        Assert.True(vector.IsEmpty);
        Assert.Same(ReadOnlyVector<int>.Empty, vector);
    }

    [Fact]
    public void ToReadOnlyVector_FromReadOnlySpan_ShouldCopyValues()
    {
        int[] values = [1, 2, 3];

        var vector = values.AsSpan().ToReadOnlyVector();
        values[0] = 42;

        Assert.Equal(1, vector[0]);
    }
}

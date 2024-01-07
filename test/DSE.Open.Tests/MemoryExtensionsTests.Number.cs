// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Tests;

#pragma warning disable CA2211 // Non-constant fields should not be visible

public partial class MemoryExtensionsTests
{
    private static readonly int[] s_oneInt32 = [1];

    private static readonly int[] s_oneToEightInt32 = [1, 2, 3, 4, 5, 6, 7, 8];

    public static TheoryData<int[], int> MinDataInt32 = new()
    {
        { s_oneInt32, 1 },
        { s_oneToEightInt32, 1 }
    };

    public static TheoryData<int[], int> MaxDataInt32 = new()
    {
        { s_oneInt32, 1 },
        { s_oneToEightInt32, 8 }
    };

    [Fact]
    public void Min_WithEmptySource_ShouldThrowArgumentException()
    {
        // Act
        static void Act()
        {
            ReadOnlySpan<int> source = Array.Empty<int>();
            source.Min();
        }

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Theory]
    [MemberData(nameof(MinDataInt32))]
    public void Min_ShouldReturnMinValue(int[] source, int expected)
    {
        // Act
        var actual = ((ReadOnlySpan<int>)source).Min();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MinBy_WithEmptySource_ShouldThrowArgumentException()
    {
        // Act
        static void Act()
        {
            ReadOnlySpan<int> source = Array.Empty<int>();
            source.MinBy(x => x);
        }

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Theory]
    [MemberData(nameof(MinDataInt32))]
    public void MinBy_ShouldReturnMinValue(int[] source, int expected)
    {
        // Act
        var actual = source.MinBy(x => x);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Max_WithEmptySource_ShouldThrowArgumentException()
    {
        // Act
        static void Act()
        {
            ReadOnlySpan<int> source = Array.Empty<int>();
            source.Max();
        }

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Theory]
    [MemberData(nameof(MaxDataInt32))]
    public void Max_ShouldReturnMaxValue(int[] source, int expected)
    {
        // Act
        var actual = ((ReadOnlySpan<int>)source).Max();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MaxBy_WithEmptySource_ShouldThrowArgumentException()
    {
        // Act
        static void Act()
        {
            ReadOnlySpan<int> source = Array.Empty<int>();
            source.MaxBy(x => x);
        }

        // Assert
        _ = Assert.Throws<ArgumentException>(Act);
    }

    [Theory]
    [MemberData(nameof(MaxDataInt32))]
    public void MaxBy_ShouldReturnMaxValue(int[] source, int expected)
    {
        // Act
        var actual = ((ReadOnlySpan<int>)source).MaxBy(x => x);

        // Assert
        Assert.Equal(expected, actual);
    }
}

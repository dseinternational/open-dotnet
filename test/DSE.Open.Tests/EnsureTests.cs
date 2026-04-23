// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public class EnsureTests
{
    [Fact]
    public void NotNull_Struct_ReturnsValue()
    {
        int? value = 42;

        var result = Ensure.NotNull(value);

        Assert.Equal(42, result);
    }

    [Fact]
    public void NotNull_Struct_NullThrows()
    {
        int? value = null;

        _ = Assert.Throws<ArgumentNullException>(() => Ensure.NotNull(value));
    }

    [Fact]
    public void NotNull_Class_ReturnsValue()
    {
        const string value = "hello";

        var result = Ensure.NotNull(value);

        Assert.Equal("hello", result);
    }

    [Fact]
    public void NotNull_Class_NullThrows()
    {
        string? value = null;

        _ = Assert.Throws<ArgumentNullException>(() => Ensure.NotNull(value));
    }

    [Fact]
    public void NotDefault_NonDefault_ReturnsValue()
    {
        Assert.Equal(42, Ensure.NotDefault(42));
    }

    [Fact]
    public void NotDefault_Default_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => Ensure.NotDefault(0));
    }

    [Fact]
    public void NotDefault_WithComparer_UsesComparer()
    {
        var comparer = EqualityComparer<int>.Default;

        Assert.Equal(5, Ensure.NotDefault(5, comparer));
    }

    [Fact]
    public void NotDefault_NullComparer_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => Ensure.NotDefault(5, null!));
    }

    [Fact]
    public void NotNull_String_ReturnsValue()
    {
        Assert.Equal("hello", Ensure.NotNull("hello"));
    }

    [Fact]
    public void NotNull_String_NullThrows()
    {
        string? s = null;
        _ = Assert.Throws<ArgumentNullException>(() => Ensure.NotNull(s));
    }

    [Fact]
    public void NotNullOrWhitespace_Valid_ReturnsValue()
    {
        Assert.Equal("hello", Ensure.NotNullOrWhitespace("hello"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t\n")]
    public void NotNullOrWhitespace_EmptyOrWhitespace_Throws(string value)
    {
        _ = Assert.Throws<ArgumentException>(() => Ensure.NotNullOrWhitespace(value));
    }

    [Fact]
    public void NotNullOrWhitespace_Null_Throws()
    {
        string? value = null;
        _ = Assert.Throws<ArgumentNullException>(() => Ensure.NotNullOrWhitespace(value));
    }

    [Theory]
    [InlineData("abc", 3)]
    [InlineData("abcdef", 3)]
    public void MinimumLength_MetOrExceeded_ReturnsValue(string value, int minLength)
    {
        Assert.Equal(value, Ensure.MinimumLength(value, minLength));
    }

    [Fact]
    public void MinimumLength_BelowMinimum_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => Ensure.MinimumLength("ab", 3));
    }

    [Fact]
    public void MinimumLength_AllowsWhitespaceWhenFlagSet()
    {
        Assert.Equal("   hello", Ensure.MinimumLength("   hello", 3, allowWhitespace: true));
    }

    [Fact]
    public void MinimumLength_WhitespaceNotAllowedByDefault()
    {
        _ = Assert.Throws<ArgumentException>(() => Ensure.MinimumLength("   ", 3));
    }

    [Theory]
    [InlineData("abc", 5)]
    [InlineData("abc", 3)]
    public void MaximumLength_WithinLimit_ReturnsValue(string value, int maxLength)
    {
        Assert.Equal(value, Ensure.MaximumLength(value, maxLength));
    }

    [Fact]
    public void MaximumLength_Exceeded_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => Ensure.MaximumLength("abcdef", 3));
    }

    [Theory]
    [InlineData(5, 5)]
    [InlineData(10, 5)]
    [InlineData(int.MaxValue, 0)]
    public void EqualOrGreaterThan_Satisfied_ReturnsValue(int value, int minimum)
    {
        Assert.Equal(value, Ensure.EqualOrGreaterThan(value, minimum));
    }

    [Fact]
    public void EqualOrGreaterThan_Violated_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => Ensure.EqualOrGreaterThan(1, 5));
    }

    [Theory]
    [InlineData(5, 5)]
    [InlineData(1, 5)]
    [InlineData(int.MinValue, 0)]
    public void EqualOrLessThan_Satisfied_ReturnsValue(int value, int maximum)
    {
        Assert.Equal(value, Ensure.EqualOrLessThan(value, maximum));
    }

    [Fact]
    public void EqualOrLessThan_Violated_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => Ensure.EqualOrLessThan(10, 5));
    }

    [Theory]
    [InlineData(5, 1, 10)]
    [InlineData(1, 1, 10)]
    [InlineData(10, 1, 10)]
    public void InRange_Within_ReturnsValue(int value, int start, int end)
    {
        Assert.Equal(value, Ensure.InRange(value, start, end));
    }

    [Theory]
    [InlineData(0, 1, 10)]
    [InlineData(11, 1, 10)]
    public void InRange_Outside_Throws(int value, int start, int end)
    {
        _ = Assert.Throws<ArgumentException>(() => Ensure.InRange(value, start, end));
    }
}

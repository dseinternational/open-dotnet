// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Abstractions.Tests;

public class TrileanTests
{
    public static IEnumerable<object[]> BoolToTrileanData()
    {
        yield return new object[] { true, Trilean.True };
        yield return new object[] { false, Trilean.False };
    }

    [Fact]
    public void DefaultValueIsUnknown()
    {
        Assert.Equal(Trilean.Na, default);
    }

    [Theory]
    [MemberData(nameof(BoolToTrileanData))]
    public void ImplicitBoolConversion_CreatesExpectedTrilean(bool input, Trilean expected)
    {
        Trilean t = input;
        Assert.False(t.IsNa);
        Assert.True(t.IsTrue == input);
        Assert.True(t.IsFalse == !input);
        Assert.True(t.IsTrue || t.IsFalse);
        Assert.True((t == expected).IsTrue);
        Assert.True(t.Equals(expected).IsTrue);
        Assert.Equal(expected, t);
    }

    [Fact]
    public void ImplicitNullableBoolConversion_ToUnknown()
    {
        bool? n = null;
        Trilean t = n;
        Assert.True(t.IsNa);
    }

    [Fact]
    public void ToNullableBoolean_ReturnsExpected()
    {
        Assert.True(Trilean.True.ToNullableBoolean());
        Assert.False(Trilean.False.ToNullableBoolean());
        Assert.Null(Trilean.Na.ToNullableBoolean());
    }

    [Fact]
    public void ToBoolean_ThrowsOnUnknown()
    {
        _ = Assert.Throws<NaValueException>(() => Trilean.Na.ToBoolean());
    }

    [Theory]
    [InlineData(1, "True")]
    [InlineData(2, "False")]
    [InlineData(0, "Unknown")]
    public void ToString_ReturnsExpected(byte value, string expected)
    {
        var t = Trilean.FromUnsignedNumber(value);
        Assert.Equal(expected, t.ToString());
    }

    /*
    If either operand is F → result F.
    Else if both operands are T → result T.
    Otherwise (the mixed / unknown cases) → result U.
     */
    [Theory]
    [InlineData(2, 2, 2)]
    [InlineData(2, 1, 2)]
    [InlineData(1, 2, 2)]
    [InlineData(1, 1, 1)]
    [InlineData(2, 0, 2)]
    [InlineData(0, 2, 2)]
    [InlineData(1, 0, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(0, 0, 0)]
    public void LogicalAnd(byte leftVal, byte rightVal, byte expectedVal)
    {
        var left = Trilean.FromUnsignedNumber(leftVal);
        var right = Trilean.FromUnsignedNumber(rightVal);
        var result = left & right;
        Assert.Equal(expectedVal, result.ToUnsignedNumber<byte>());
    }

    /*
    If either operand is T → result T.
    Else if both operands are F → result F.
    Otherwise (the mixed / unknown cases) → result U.
     */
    [Theory]
    [InlineData(2, 2, 2)]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 1)]
    [InlineData(2, 0, 0)]
    [InlineData(0, 2, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(0, 1, 1)]
    [InlineData(0, 0, 0)]
    public void LogicalOr(byte leftVal, byte rightVal, byte expectedVal)
    {
        var left = Trilean.FromUnsignedNumber(leftVal);
        var right = Trilean.FromUnsignedNumber(rightVal);
        var result = left | right;
        Assert.Equal(expectedVal, result.ToUnsignedNumber<byte>());
    }

    [Theory]
    [InlineData(2, 2, 2)]
    [InlineData(2, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 2)]
    [InlineData(2, 0, 0)]
    [InlineData(0, 2, 0)]
    [InlineData(1, 0, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(0, 0, 0)]
    public void LogicalXor(byte leftVal, byte rightVal, byte expectedVal)
    {
        var left = Trilean.FromUnsignedNumber(leftVal);
        var right = Trilean.FromUnsignedNumber(rightVal);
        var result = left ^ right;
        Assert.Equal(expectedVal, result.ToUnsignedNumber<byte>());
    }
}

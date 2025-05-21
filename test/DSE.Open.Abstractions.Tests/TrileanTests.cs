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
        Assert.True(t.TernaryEquals(expected).IsTrue);
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
    [InlineData(0, "False")]
    [InlineData(2, "Unknown")]
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
    [InlineData(0, 0, 0)] // F AND F -> F
    [InlineData(0, 2, 0)] // F AND U -> F
    [InlineData(0, 1, 0)] // F AND T -> F
    [InlineData(2, 0, 0)] // U AND F -> F
    [InlineData(2, 2, 2)] // U AND U -> U
    [InlineData(2, 1, 2)] // U AND T -> U
    [InlineData(1, 0, 0)] // T AND F -> F
    [InlineData(1, 2, 2)] // T AND U -> U
    [InlineData(1, 1, 1)] // T AND T -> T
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
    [InlineData(0, 0, 0)] // F OR F -> F
    [InlineData(0, 2, 2)] // F OR U -> U
    [InlineData(0, 1, 1)] // F OR T -> T
    [InlineData(2, 0, 2)] // U OR F -> U
    [InlineData(2, 2, 2)] // U OR U -> U
    [InlineData(2, 1, 1)] // U OR T -> T
    [InlineData(1, 0, 1)] // T OR F -> T
    [InlineData(1, 2, 1)] // T OR U -> T
    [InlineData(1, 1, 1)] // T OR T -> T
    public void LogicalOr(byte leftVal, byte rightVal, byte expectedVal)
    {
        var left = Trilean.FromUnsignedNumber(leftVal);
        var right = Trilean.FromUnsignedNumber(rightVal);
        var result = left | right;
        Assert.Equal(expectedVal, result.ToUnsignedNumber<byte>());
    }

    [Theory]
    [InlineData(0, 0, 0)] // F XOR F -> F
    [InlineData(0, 2, 2)] // F XOR U -> U
    [InlineData(0, 1, 1)] // F XOR T -> T
    [InlineData(2, 0, 2)] // U XOR F -> U
    [InlineData(2, 2, 2)] // U XOR U -> U
    [InlineData(2, 1, 2)] // U XOR T -> U
    [InlineData(1, 0, 1)] // T XOR F -> T
    [InlineData(1, 2, 2)] // T XOR U -> U
    [InlineData(1, 1, 0)] // T XOR T -> F
    public void LogicalXor(byte leftVal, byte rightVal, byte expectedVal)
    {
        var left = Trilean.FromUnsignedNumber(leftVal);
        var right = Trilean.FromUnsignedNumber(rightVal);
        var result = left ^ right;
        Assert.Equal(expectedVal, result.ToUnsignedNumber<byte>());
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var t1 = Trilean.True;
        var t2 = Trilean.True;
        Assert.True(t1.Equals(t2));
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var t1 = Trilean.True;
        var t2 = Trilean.False;
        Assert.False(t1.Equals(t2));
    }

    [Fact]
    public void Equals_NaValue_ReturnsFalse()
    {
        var t1 = Trilean.True;
        var t2 = Trilean.Na;
        Assert.False(t1.Equals(t2));
    }

    [Fact]
    public void EqualAndNotNa_BothTrue_ReturnsTrue()
    {
        var t1 = Trilean.True;
        var t2 = Trilean.True;
        Assert.True(t1.EqualAndNotNa(t2));
    }

    [Fact]
    public void EqualAndNotNa_OneNa_ReturnsFalse()
    {
        var t1 = Trilean.True;
        var t2 = Trilean.Na;
        Assert.False(t1.EqualAndNotNa(t2));
    }

    [Fact]
    public void EqualOrBothNa_BothNa_ReturnsTrue()
    {
        var t1 = Trilean.Na;
        var t2 = Trilean.Na;
        Assert.True(t1.EqualOrBothNa(t2));
    }

    [Fact]
    public void EqualOrBothNa_OneNa_ReturnsFalse()
    {
        var t1 = Trilean.True;
        var t2 = Trilean.Na;
        Assert.False(t1.EqualOrBothNa(t2));
    }

    [Fact]
    public void EqualOrEitherNa_OneNa_ReturnsTrue()
    {
        var t1 = Trilean.True;
        var t2 = Trilean.Na;
        Assert.True(t1.EqualOrEitherNa(t2));
    }

    [Fact]
    public void EqualOrEitherNa_BothFalse_ReturnsTrue()
    {
        var t1 = Trilean.False;
        var t2 = Trilean.False;
        Assert.True(t1.EqualOrEitherNa(t2));
    }

    [Fact]
    public void LogicalNot_True_ReturnsFalse()
    {
        var t = Trilean.True;
        var result = Trilean.LogicalNot(t);
        Assert.True(result.IsFalse);
    }

    [Fact]
    public void LogicalNot_Na_ReturnsNa()
    {
        var t = Trilean.Na;
        var result = Trilean.LogicalNot(t);
        Assert.True(result.IsNa);
    }

    [Fact]
    public void FromBoolean_NullableTrue_ReturnsTrue()
    {
        bool? value = true;
        var t = Trilean.FromBoolean(value);
        Assert.True(t.IsTrue);
    }

    [Fact]
    public void FromBoolean_NullableFalse_ReturnsFalse()
    {
        bool? value = false;
        var t = Trilean.FromBoolean(value);
        Assert.True(t.IsFalse);
    }

    [Fact]
    public void FromBoolean_NullableNull_ReturnsNa()
    {
        bool? value = null;
        var t = Trilean.FromBoolean(value);
        Assert.True(t.IsNa);
    }

    [Fact]
    public void ToSignedNumber_True_ReturnsOne()
    {
        var t = Trilean.True;
        var result = t.ToSignedNumber<int>();
        Assert.Equal(1, result);
    }

    [Fact]
    public void ToSignedNumber_Na_ReturnsNegativeOne()
    {
        var t = Trilean.Na;
        var result = t.ToSignedNumber<int>();
        Assert.Equal(-1, result);
    }

    [Fact]
    public void ToUnsignedNumber_Na_ReturnsTwo()
    {
        var t = Trilean.Na;
        var result = t.ToUnsignedNumber<byte>();
        Assert.Equal(2, result);
    }

    [Fact]
    public void Parse_TrueString_ReturnsTrue()
    {
        var t = Trilean.Parse("True", null);
        Assert.True(t.IsTrue);
    }

    [Fact]
    public void Parse_UnknownString_ReturnsNa()
    {
        var t = Trilean.Parse("Unknown", null);
        Assert.True(t.IsNa);
    }

    [Fact]
    public void TryParse_InvalidString_ReturnsFalse()
    {
        var success = Trilean.TryParse("Invalid", null, out var result);
        Assert.False(success);
        Assert.True(result.IsNa);
    }
}

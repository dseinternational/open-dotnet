// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Abstractions.Tests;

public class TernaryTests
{
    [Fact]
    public void NaValue_ValueAndNa_IsNa()
    {
        var t = Ternary.Equals(1, NaInt.Na);
        Assert.True(t.IsNa);
    }

    [Fact]
    public void NaValue_NaAndValue_IsNa()
    {
        var t = Ternary.Equals(NaInt.Na, 1);
        Assert.True(t.IsNa);
    }

    [Fact]
    public void NaValue_NaAndNa_IsNa()
    {
        var t = Ternary.Equals(NaInt.Na, NaInt.Na);
        Assert.True(t.IsNa);
    }

    [Fact]
    public void NaValue_ValueAndEqualValue_IsTrue()
    {
        var t = Ternary.Equals<NaInt>(1, 1);
        Assert.True(t.IsTrue);
    }

    [Fact]
    public void NaValue_ValueAndUnequalValue_IsFalse()
    {
        var t = Ternary.Equals<NaInt>(1, 2);
        Assert.True(t.IsFalse);
    }

    [Fact]
    public void Nullable_ValueAndNull_IsNa()
    {
        var t = Ternary.Equals<int>(1, null);
        Assert.True(t.IsNa);
    }

    [Fact]
    public void Nullable_NullAndValue_IsNa()
    {
        var t = Ternary.Equals<int>(null, 1);
        Assert.True(t.IsNa);
    }

    [Fact]
    public void Nullable_NullAndNull_IsNa()
    {
        var t = Ternary.Equals<int>(null, null);
        Assert.True(t.IsNa);
    }

    [Fact]
    public void Nullable_ValueAndEqualValue_IsTrue()
    {
        var t = Ternary.Equals<int>(1, 1);
        Assert.True(t.IsTrue);
    }

    [Fact]
    public void Nullable_ValueAndUnequalValue_IsFalse()
    {
        var t = Ternary.Equals<int>(1, 2);
        Assert.True(t.IsFalse);
    }

    [Fact]
    public void EqualAndNeitherNa_BothValuesEqualAndNotNa_ReturnsTrue()
    {
        var result = Ternary.EqualAndNeitherNa(new NaInt(1), new NaInt(1));
        Assert.True(result);
    }

    [Fact]
    public void EqualAndNeitherNa_OneValueIsNa_ReturnsFalse()
    {
        var result = Ternary.EqualAndNeitherNa(NaInt.Na, new NaInt(1));
        Assert.False(result);
    }

    [Fact]
    public void EqualOrBothNa_BothValuesEqualAndNotNa_ReturnsTrue()
    {
        var result = Ternary.EqualOrBothNa(new NaInt(1), new NaInt(1));
        Assert.True(result);
    }

    [Fact]
    public void EqualOrBothNa_BothValuesAreNa_ReturnsTrue()
    {
        var result = Ternary.EqualOrBothNa(NaInt.Na, NaInt.Na);
        Assert.True(result);
    }

    [Fact]
    public void EqualOrBothNa_OneValueIsNa_ReturnsFalse()
    {
        var result = Ternary.EqualOrBothNa(NaInt.Na, new NaInt(1));
        Assert.False(result);
    }

    [Fact]
    public void EqualOrEitherNa_BothValuesEqualAndNotNa_ReturnsTrue()
    {
        var result = Ternary.EqualOrEitherNa(new NaInt(1), new NaInt(1));
        Assert.True(result);
    }

    [Fact]
    public void EqualOrEitherNa_OneValueIsNa_ReturnsTrue()
    {
        var result = Ternary.EqualOrEitherNa(NaInt.Na, new NaInt(1));
        Assert.True(result);
    }

    [Fact]
    public void EqualOrEitherNa_BothValuesAreNa_ReturnsTrue()
    {
        var result = Ternary.EqualOrEitherNa(NaInt.Na, NaInt.Na);
        Assert.True(result);
    }

    [Fact]
    public void EqualAndNeitherNa_BothValuesEqualAndNotNa_Int_ReturnsTrue()
    {
        var result = Ternary.EqualAndNeitherNa<int>(1, 1);
        Assert.True(result);
    }

    [Fact]
    public void EqualAndNeitherNa_OneValueIsNa_Int_ReturnsFalse()
    {
        var result = Ternary.EqualAndNeitherNa<int>(null, 1);
        Assert.False(result);
    }

    [Fact]
    public void EqualOrBothNa_BothValuesEqualAndNotNa_Int_ReturnsTrue()
    {
        var result = Ternary.EqualOrBothNa<int>(1, 1);
        Assert.True(result);
    }

    [Fact]
    public void EqualOrBothNa_BothValuesAreNa_Int_ReturnsTrue()
    {
        var result = Ternary.EqualOrBothNa<int>(null, null);
        Assert.True(result);
    }

    [Fact]
    public void EqualOrBothNa_OneValueIsNa_Int_ReturnsFalse()
    {
        var result = Ternary.EqualOrBothNa<int>(null, 1);
        Assert.False(result);
    }

    [Fact]
    public void EqualOrEitherNa_BothValuesEqualAndNotNa_Int_ReturnsTrue()
    {
        var result = Ternary.EqualOrEitherNa<int>(1, 1);
        Assert.True(result);
    }

    [Fact]
    public void EqualOrEitherNa_OneValueIsNa_Int_ReturnsTrue()
    {
        var result = Ternary.EqualOrEitherNa<int>(null, 1);
        Assert.True(result);
    }

    [Fact]
    public void EqualOrEitherNa_BothValuesAreNa_Int_ReturnsTrue()
    {
        var result = Ternary.EqualOrEitherNa<int>(null, null);
        Assert.True(result);
    }
}

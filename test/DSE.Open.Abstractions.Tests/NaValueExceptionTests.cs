// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Abstractions.Tests;

public class NaValueExceptionTests
{
    [Fact]
    public void Ctor_NullMessage_UsesDefaultMessage()
    {
        var ex = new NaValueException(null);
        Assert.Equal("Cannot access value as the value is unknown.", ex.Message);
    }

    [Fact]
    public void Ctor_NullMessageAndInner_UsesDefaultMessageAndPreservesInner()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new NaValueException(null, inner);
        Assert.Equal("Cannot access value as the value is unknown.", ex.Message);
        Assert.Same(inner, ex.InnerException);
    }

    [Fact]
    public void Ctor_NullInnerException_Accepted()
    {
        var ex = new NaValueException("boom", null);
        Assert.Equal("boom", ex.Message);
        Assert.Null(ex.InnerException);
    }

    [Fact]
    public void ThrowIfNa_NullValue_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() => NaValueException.ThrowIfNa(null!));
    }

    [Fact]
    public void ThrowIfNa_ValueWithoutValue_ThrowsNaValueException()
    {
        INaValue naValue = NaInt.Na;
        _ = Assert.Throws<NaValueException>(() => NaValueException.ThrowIfNa(naValue));
    }

    [Fact]
    public void ThrowIfNa_ValueWithValue_DoesNotThrow()
    {
        INaValue naValue = new NaInt(42);
        NaValueException.ThrowIfNa(naValue);
    }

    [Fact]
    public void ThrowIfNa_CustomMessage_PropagatesToException()
    {
        INaValue naValue = NaInt.Na;
        var ex = Assert.Throws<NaValueException>(() => NaValueException.ThrowIfNa(naValue, "custom"));
        Assert.Equal("custom", ex.Message);
    }

    [Fact]
    public void ThrowIfUnknown_Na_ThrowsNaValueException()
    {
        _ = Assert.Throws<NaValueException>(() => NaValueException.ThrowIfUnknown(Trilean.Na));
    }

    [Fact]
    public void ThrowIfUnknown_Known_DoesNotThrow()
    {
        NaValueException.ThrowIfUnknown(Trilean.True);
        NaValueException.ThrowIfUnknown(Trilean.False);
    }

    [Fact]
    public void ThrowIfUnknown_NullMessage_UsesDefaultMessage()
    {
        var ex = Assert.Throws<NaValueException>(() => NaValueException.ThrowIfUnknown(Trilean.Na));
        Assert.Equal("Cannot access value as the value is unknown.", ex.Message);
    }
}

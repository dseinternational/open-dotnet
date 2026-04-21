// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public sealed class BinaryTests
{
    public static TheoryData<Binary> AllValues { get; } = new() { Binary.False, Binary.True };

    [Theory]
    [MemberData(nameof(AllValues))]
    public void CanRoundtripJson(Binary value)
    {
        var json = JsonSerializer.Serialize(value, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<Binary>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(value, deserialized);
    }

    [Fact]
    public void True_SerializesAsQuotedDigit()
    {
        var json = JsonSerializer.Serialize(Binary.True, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal("\"1\"", json);
    }

    [Fact]
    public void False_SerializesAsQuotedDigit()
    {
        var json = JsonSerializer.Serialize(Binary.False, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal("\"0\"", json);
    }

    [Fact]
    public void FromBoolean_ReturnsTrueOrFalseStatic()
    {
        Assert.Equal(Binary.True, Binary.FromBoolean(true));
        Assert.Equal(Binary.False, Binary.FromBoolean(false));
    }

    [Fact]
    public void ToBoolean_ReturnsExpected()
    {
        Assert.True(Binary.True.ToBoolean());
        Assert.False(Binary.False.ToBoolean());
    }

    [Fact]
    public void ImplicitConversion_ToBool_ReturnsExpected()
    {
        bool asTrue = Binary.True;
        bool asFalse = Binary.False;
        Assert.True(asTrue);
        Assert.False(asFalse);
    }

    [Fact]
    public void ImplicitConversion_FromBool_ReturnsExpected()
    {
        Binary fromTrue = true;
        Binary fromFalse = false;
        Assert.Equal(Binary.True, fromTrue);
        Assert.Equal(Binary.False, fromFalse);
    }

    [Fact]
    public void GetBinary_ReturnsUnderlying()
    {
        Assert.True(Binary.True.GetBinary());
        Assert.False(Binary.False.GetBinary());
    }

    [Fact]
    public void ValueType_IsBinary()
    {
        Assert.Equal(MeasurementValueType.Binary, Binary.True.ValueType);
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)1)]
    public void IsValidValue_ReturnsTrueForZeroOrOne(byte value)
    {
        Assert.True(Binary.IsValidValue(value));
    }

    [Theory]
    [InlineData((byte)2)]
    [InlineData(byte.MaxValue)]
    public void IsValidValue_ReturnsFalseForOther(byte value)
    {
        Assert.False(Binary.IsValidValue(value));
    }

    [Fact]
    public void GetOrdinal_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = Binary.True;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetOrdinal());
    }

    [Fact]
    public void GetCount_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = Binary.True;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetCount());
    }

    [Fact]
    public void GetAmount_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = Binary.True;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetAmount());
    }

    [Fact]
    public void GetRatio_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = Binary.True;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetRatio());
    }

    [Fact]
    public void GetFrequency_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = Binary.True;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetFrequency());
    }

    [Fact]
    public void GetRepeatableHashCode_IsStableAcrossInstances()
    {
        Assert.Equal(Binary.True.GetRepeatableHashCode(), Binary.FromBoolean(true).GetRepeatableHashCode());
    }

    [Fact]
    public void GetRepeatableHashCode_DistinguishesByValue()
    {
        Assert.NotEqual(Binary.True.GetRepeatableHashCode(), Binary.False.GetRepeatableHashCode());
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public sealed class SpeechClarityTests
{
    public static TheoryData<byte> ValidOrdinals { get; } = new() { 10, 50, 90 };

    public static TheoryData<SpeechClarity> AllValues { get; } = new()
    {
        SpeechClarity.Unclear,
        SpeechClarity.Developing,
        SpeechClarity.Clear,
    };

    [Fact]
    public void SerializesToNumber()
    {
        var json = JsonSerializer.Serialize(SpeechClarity.Developing);
        Assert.Equal("50", json);
    }

    [Theory]
    [MemberData(nameof(AllValues))]
    public void CanRoundtripJson(SpeechClarity value)
    {
        var json = JsonSerializer.Serialize(value, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<SpeechClarity>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(value, deserialized);
    }

    [Theory]
    [InlineData((byte)10)]
    [InlineData((byte)50)]
    [InlineData((byte)90)]
    public void GetOrdinal_ReturnsValue(byte value)
    {
        var speechClarity = (SpeechClarity)value;
        Assert.Equal(value, speechClarity.GetOrdinal());
    }

    [Fact]
    public void StaticMembers_HaveExpectedOrdinals()
    {
        Assert.Equal((byte)10, SpeechClarity.Unclear.GetOrdinal());
        Assert.Equal((byte)50, SpeechClarity.Developing.GetOrdinal());
        Assert.Equal((byte)90, SpeechClarity.Clear.GetOrdinal());
    }

    [Theory]
    [MemberData(nameof(ValidOrdinals))]
    public void IsValidValue_ReturnsTrueForKnownOrdinals(byte value)
    {
        Assert.True(SpeechClarity.IsValidValue(value));
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)11)]
    [InlineData((byte)49)]
    [InlineData((byte)91)]
    [InlineData(byte.MaxValue)]
    public void IsValidValue_ReturnsFalseForUnknownOrdinals(byte value)
    {
        Assert.False(SpeechClarity.IsValidValue(value));
    }

    [Fact]
    public void ValueType_IsOrdinal()
    {
        Assert.Equal(MeasurementValueType.Ordinal, SpeechClarity.Developing.ValueType);
    }

    [Fact]
    public void GetBinary_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = SpeechClarity.Developing;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetBinary());
    }

    [Fact]
    public void GetCount_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = SpeechClarity.Developing;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetCount());
    }

    [Fact]
    public void GetAmount_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = SpeechClarity.Developing;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetAmount());
    }

    [Fact]
    public void GetRatio_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = SpeechClarity.Developing;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetRatio());
    }

    [Fact]
    public void GetFrequency_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = SpeechClarity.Developing;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetFrequency());
    }

    [Theory]
    [MemberData(nameof(AllValues))]
    public void GetRepeatableHashCode_IsStableAcrossInstances(SpeechClarity value)
    {
        var copy = (SpeechClarity)(byte)value;
        Assert.Equal(value.GetRepeatableHashCode(), copy.GetRepeatableHashCode());
    }

    [Fact]
    public void GetRepeatableHashCode_DistinguishesByValue()
    {
        Assert.NotEqual(
            SpeechClarity.Unclear.GetRepeatableHashCode(),
            SpeechClarity.Clear.GetRepeatableHashCode());
    }

    [Fact]
    public void Equality_SameValue_Equal()
    {
        Assert.Equal(SpeechClarity.Developing, (SpeechClarity)(byte)50);
    }

    [Fact]
    public void Equality_DifferentValue_NotEqual()
    {
        Assert.NotEqual(SpeechClarity.Unclear, SpeechClarity.Clear);
    }
}

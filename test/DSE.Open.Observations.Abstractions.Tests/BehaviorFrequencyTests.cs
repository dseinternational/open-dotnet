// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public sealed class BehaviorFrequencyTests
{
    public static TheoryData<byte> ValidOrdinals { get; } = new() { 0, 10, 50, 90 };

    public static TheoryData<BehaviorFrequency> AllValues { get; } = new()
    {
        BehaviorFrequency.Never,
        BehaviorFrequency.Emerging,
        BehaviorFrequency.Developing,
        BehaviorFrequency.Achieved,
    };

    [Theory]
    [MemberData(nameof(ValidOrdinals))]
    public void SerializesToNumber(byte value)
    {
        var frequency = BehaviorFrequency.FromValue(value);
        var json = JsonSerializer.Serialize(frequency, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(value.ToStringInvariant(), json);
    }

    [Theory]
    [MemberData(nameof(AllValues))]
    public void CanRoundtripJson(BehaviorFrequency value)
    {
        var json = JsonSerializer.Serialize(value, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<BehaviorFrequency>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(value, deserialized);
    }

    [Theory]
    [MemberData(nameof(AllValues))]
    public void GetOrdinal_ReturnsUnderlyingByte(BehaviorFrequency value)
    {
        Assert.Equal((byte)value, value.GetOrdinal());
    }

    [Fact]
    public void StaticMembers_HaveExpectedOrdinals()
    {
        Assert.Equal((byte)0, ((IObservationValue)BehaviorFrequency.Never).GetOrdinal());
        Assert.Equal((byte)10, ((IObservationValue)BehaviorFrequency.Emerging).GetOrdinal());
        Assert.Equal((byte)50, ((IObservationValue)BehaviorFrequency.Developing).GetOrdinal());
        Assert.Equal((byte)90, ((IObservationValue)BehaviorFrequency.Achieved).GetOrdinal());
    }

    [Theory]
    [MemberData(nameof(ValidOrdinals))]
    public void IsValidValue_ReturnsTrueForKnownOrdinals(byte value)
    {
        Assert.True(BehaviorFrequency.IsValidValue(value));
    }

    [Theory]
    [InlineData((byte)1)]
    [InlineData((byte)11)]
    [InlineData((byte)49)]
    [InlineData((byte)91)]
    [InlineData(byte.MaxValue)]
    public void IsValidValue_ReturnsFalseForUnknownOrdinals(byte value)
    {
        Assert.False(BehaviorFrequency.IsValidValue(value));
    }

    [Fact]
    public void ValueType_IsOrdinal()
    {
        Assert.Equal(MeasurementValueType.Ordinal, BehaviorFrequency.Developing.ValueType);
    }

    [Fact]
    public void GetBinary_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = BehaviorFrequency.Developing;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetBinary());
    }

    [Fact]
    public void GetCount_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = BehaviorFrequency.Developing;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetCount());
    }

    [Fact]
    public void GetAmount_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = BehaviorFrequency.Developing;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetAmount());
    }

    [Fact]
    public void GetRatio_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = BehaviorFrequency.Developing;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetRatio());
    }

    [Fact]
    public void GetFrequency_ThrowsValueTypeMismatchException()
    {
        IObservationValue value = BehaviorFrequency.Developing;
        _ = Assert.Throws<ValueTypeMismatchException>(() => value.GetFrequency());
    }

    [Theory]
    [MemberData(nameof(AllValues))]
    public void GetRepeatableHashCode_IsStableAcrossInstances(BehaviorFrequency value)
    {
        var copy = BehaviorFrequency.FromValue((byte)value);
        Assert.Equal(value.GetRepeatableHashCode(), copy.GetRepeatableHashCode());
    }

    [Fact]
    public void GetRepeatableHashCode_DistinguishesByValue()
    {
        Assert.NotEqual(
            BehaviorFrequency.Emerging.GetRepeatableHashCode(),
            BehaviorFrequency.Achieved.GetRepeatableHashCode());
    }

    [Fact]
    public void Equality_SameValue_Equal()
    {
        Assert.Equal(BehaviorFrequency.Developing, BehaviorFrequency.FromValue(50));
    }

    [Fact]
    public void Equality_DifferentValue_NotEqual()
    {
        Assert.NotEqual(BehaviorFrequency.Never, BehaviorFrequency.Achieved);
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public sealed class CountTests
{
    [Theory]
    [InlineData(0uL)]
    [InlineData(1uL)]
    [InlineData(uint.MaxValue)]
    public void CanInitialize(ulong value)
    {
        var count = new Count(value);
        Assert.Equal(value, (ulong)count);
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(1u)]
    [InlineData(uint.MaxValue)]
    public void CanSerializeAndDeserialize(uint value)
    {
        var count = new Count(value);
        var json = JsonSerializer.Serialize(count, JsonSharedOptions.RelaxedJsonEscaping);
        var count2 = JsonSerializer.Deserialize<Count>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(count, count2);
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(1u)]
    [InlineData(uint.MaxValue)]
    public void SerializesToNumber(uint value)
    {
        var count = new Count(value);
        var json = JsonSerializer.Serialize(count, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(value.ToStringInvariant(), json);
    }

    [Fact]
    public void MaxSerializedCharLength_IsAtLeastAsLongAsMaxValue()
    {
        var maxValueLength = Count.MaxValue.ToStringInvariant().Length;
        Assert.True(
            Count.MaxSerializedCharLength >= maxValueLength,
            $"MaxSerializedCharLength ({Count.MaxSerializedCharLength}) must be >= "
            + $"length of MaxValue string ({maxValueLength})");
    }

    [Fact]
    public void MaxSerializedByteLength_IsAtLeastAsLongAsMaxValue()
    {
        var maxValueLength = Count.MaxValue.ToStringInvariant().Length;
        Assert.True(
            Count.MaxSerializedByteLength >= maxValueLength,
            $"MaxSerializedByteLength ({Count.MaxSerializedByteLength}) must be >= "
            + $"length of MaxValue string ({maxValueLength})");
    }
}

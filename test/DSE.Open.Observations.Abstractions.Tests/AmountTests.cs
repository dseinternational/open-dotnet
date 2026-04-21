// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public class AmountTests
{
    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(798165.249850)]
    public void CanInitializeWithNonNegativeValues(double value)
    {
        var amount = new Amount((decimal)value);
        Assert.Equal((decimal)value, (decimal)amount);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-798165.249850)]
    public void CannotInitializeWithNegativeValues(double value)
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => { var amount = new Amount((decimal)value); });
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(798165.249850)]
    public void CanSerializeAndDeserialize(decimal value)
    {
        var amount = new Amount(value);
        var json = JsonSerializer.Serialize(amount, JsonSharedOptions.RelaxedJsonEscaping);
        var amount2 = JsonSerializer.Deserialize<Amount>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(amount, amount2);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(798165.249850)]
    public void SerializesToNumber(decimal value)
    {
        var amount = new Amount(value);
        var json = JsonSerializer.Serialize(amount, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(value.ToStringInvariant(), json);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(798165.249850)]
    public void GetAmount_ReturnsValue(decimal value)
    {
        var amount = new Amount(value);

        var result = amount.GetAmount();

        Assert.Equal(value, result);
    }

    [Fact]
    public void GetCount_ThrowsValueTypeMismatchException()
    {
        var amount = new Amount(42m);
        IObservationValue observationValue = amount;

        var act = () => { _ = observationValue.GetCount(); };

        _ = Assert.Throws<ValueTypeMismatchException>(act);
    }
}

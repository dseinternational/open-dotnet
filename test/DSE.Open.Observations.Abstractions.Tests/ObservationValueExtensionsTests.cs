// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public class ObservationValueExtensionsTests
{
    // --- ConvertToDouble ---

    [Fact]
    public void ConvertToDouble_Binary_True_ReturnsOne()
    {
        IObservationValue value = Binary.True;
        Assert.Equal(1d, value.ConvertToDouble());
    }

    [Fact]
    public void ConvertToDouble_Binary_False_ReturnsZero()
    {
        IObservationValue value = Binary.False;
        Assert.Equal(0d, value.ConvertToDouble());
    }

    [Fact]
    public void ConvertToDouble_Ordinal_ReturnsByteValueAsDouble()
    {
        IObservationValue value = BehaviorFrequency.Developing;
        Assert.Equal(50d, value.ConvertToDouble());
    }

    [Fact]
    public void ConvertToDouble_Count_ReturnsCountAsDouble()
    {
        IObservationValue value = new Count(42UL);
        Assert.Equal(42d, value.ConvertToDouble());
    }

    [Fact]
    public void ConvertToDouble_Amount_ReturnsAmountAsDouble()
    {
        IObservationValue value = new Amount(123.5m);
        Assert.Equal(123.5d, value.ConvertToDouble());
    }

    [Fact]
    public void ConvertToDouble_Ratio_ReturnsRatioAsDouble()
    {
        IObservationValue value = new Ratio(0.25m);
        Assert.Equal(0.25d, value.ConvertToDouble());
    }

    [Fact]
    public void ConvertToDouble_Null_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((IObservationValue)null!).ConvertToDouble());
    }

    // --- ConvertToDecimal ---

    [Fact]
    public void ConvertToDecimal_Binary_True_ReturnsOne()
    {
        IObservationValue value = Binary.True;
        Assert.Equal(1m, value.ConvertToDecimal());
    }

    [Fact]
    public void ConvertToDecimal_Binary_False_ReturnsZero()
    {
        IObservationValue value = Binary.False;
        Assert.Equal(0m, value.ConvertToDecimal());
    }

    [Fact]
    public void ConvertToDecimal_Ordinal_ReturnsByteValueAsDecimal()
    {
        IObservationValue value = BehaviorFrequency.Achieved;
        Assert.Equal(90m, value.ConvertToDecimal());
    }

    [Fact]
    public void ConvertToDecimal_Count_ReturnsCountAsDecimal()
    {
        IObservationValue value = new Count(123UL);
        Assert.Equal(123m, value.ConvertToDecimal());
    }

    [Fact]
    public void ConvertToDecimal_Amount_PreservesPrecision()
    {
        IObservationValue value = new Amount(123.456m);
        Assert.Equal(123.456m, value.ConvertToDecimal());
    }

    [Fact]
    public void ConvertToDecimal_Ratio_PreservesPrecision()
    {
        IObservationValue value = new Ratio(-0.75m);
        Assert.Equal(-0.75m, value.ConvertToDecimal());
    }

    [Fact]
    public void ConvertToDecimal_Null_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((IObservationValue)null!).ConvertToDecimal());
    }
}

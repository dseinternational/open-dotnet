// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

public class UnitOfMassTests
{
    [Fact]
    public void TryParseGram()
    {
        Assert.True(UnitOfMass.TryParse("g", out var unit));
        Assert.Equal(UnitOfMass.Gram, unit);
    }

    [Fact]
    public void TryParseKilogram()
    {
        Assert.True(UnitOfMass.TryParse("kg", out var unit));
        Assert.Equal(UnitOfMass.Kilogram, unit);
    }

    [Fact]
    public void TryParseMilligram()
    {
        Assert.True(UnitOfMass.TryParse("mg", out var unit));
        Assert.Equal(UnitOfMass.Milligram, unit);
    }
}

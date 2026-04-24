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

    [Theory]
    [InlineData("oz")]
    [InlineData(" oz ")]
    public void TryParseOunce(string value)
    {
        Assert.True(UnitOfMass.TryParse(value, out var unit));
        Assert.Equal(UnitOfMass.Ounce, unit);
    }

    [Theory]
    [InlineData("lb")]
    [InlineData(" lb ")]
    public void TryParsePound(string value)
    {
        Assert.True(UnitOfMass.TryParse(value, out var unit));
        Assert.Equal(UnitOfMass.Pound, unit);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalseAndNullUnit()
    {
        Assert.False(UnitOfMass.TryParse(null, out var unit));
        Assert.Null(unit);
    }

    [Fact]
    public void Parse_WithUnknownAbbreviation_ShouldThrowArgumentOutOfRangeException()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => UnitOfMass.Parse("stone"));
    }

    [Fact]
    public void Equivalent_unit_instances_are_equal()
    {
        var unit = new UnitOfMass(1, "Gram", "g");

        Assert.Equal(UnitOfMass.Gram, unit);
        Assert.True(UnitOfMass.Gram == unit);
        Assert.False(UnitOfMass.Gram != unit);
        Assert.Equal(UnitOfMass.Gram.GetHashCode(), unit.GetHashCode());
    }

    [Fact]
    public void CompareTo_WithNull_ShouldThrowArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() => UnitOfMass.Gram.CompareTo(null));
    }

    [Fact]
    public void Constructor_WithEmptyNameOrAbbreviation_ShouldThrowArgumentException()
    {
        _ = Assert.Throws<ArgumentException>(() => new UnitOfMass(1, "", "g"));
        _ = Assert.Throws<ArgumentException>(() => new UnitOfMass(1, "Gram", ""));
    }
}

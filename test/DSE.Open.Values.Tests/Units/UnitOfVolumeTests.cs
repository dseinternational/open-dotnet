// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

public class UnitOfVolumeTests
{
    // ------------------------------------------------------------------
    //  Static instances
    // ------------------------------------------------------------------

    [Fact]
    public void CubicMetre_has_expected_properties()
    {
        Assert.Equal("cubic metre", UnitOfVolume.CubicMetre.Name);
        Assert.Equal("m³", UnitOfVolume.CubicMetre.Abbreviation);
    }

    [Fact]
    public void CubicCentimetre_has_expected_properties()
    {
        Assert.Equal("cubic centimetre", UnitOfVolume.CubicCentimetre.Name);
        Assert.Equal("cm³", UnitOfVolume.CubicCentimetre.Abbreviation);
    }

    [Fact]
    public void CubicMillimetre_has_expected_properties()
    {
        Assert.Equal("cubic millimetre", UnitOfVolume.CubicMillimetre.Name);
        Assert.Equal("mm³", UnitOfVolume.CubicMillimetre.Abbreviation);
    }

    [Fact]
    public void Litre_has_expected_properties()
    {
        Assert.Equal("litre", UnitOfVolume.Litre.Name);
        Assert.Equal("L", UnitOfVolume.Litre.Abbreviation);
    }

    [Fact]
    public void Millilitre_has_expected_properties()
    {
        Assert.Equal("millilitre", UnitOfVolume.Millilitre.Name);
        Assert.Equal("mL", UnitOfVolume.Millilitre.Abbreviation);
    }

    [Fact]
    public void Microlitre_has_expected_properties()
    {
        Assert.Equal("microlitre", UnitOfVolume.Microlitre.Name);
        Assert.Equal("μL", UnitOfVolume.Microlitre.Abbreviation);
    }

    // ------------------------------------------------------------------
    //  BaseUnitOfMeasure
    // ------------------------------------------------------------------

    [Fact]
    public void BaseUnitOfMeasure_is_microlitre()
    {
        Assert.Same(UnitOfVolume.Microlitre, UnitOfVolume.CubicMetre.BaseUnitOfMeasure);
        Assert.Same(UnitOfVolume.Microlitre, UnitOfVolume.Litre.BaseUnitOfMeasure);
    }

    // ------------------------------------------------------------------
    //  Equality
    // ------------------------------------------------------------------

    [Fact]
    public void Same_instance_is_equal()
    {
        Assert.True(UnitOfVolume.Litre.Equals(UnitOfVolume.Litre));
    }

    [Fact]
    public void Different_units_are_not_equal()
    {
        Assert.False(UnitOfVolume.Litre.Equals(UnitOfVolume.Millilitre));
    }

    [Fact]
    public void Millilitre_and_CubicCentimetre_have_same_base_units()
    {
        // Both have base units of 1000.0 but different names/abbreviations
        Assert.Equal(UnitOfVolume.Millilitre.BaseUnits, UnitOfVolume.CubicCentimetre.BaseUnits);
    }

    // ------------------------------------------------------------------
    //  Parse
    // ------------------------------------------------------------------

    [Theory]
    [InlineData("m³")]
    [InlineData("m3")]
    [InlineData("cbm")]
    [InlineData("cubic metre")]
    [InlineData("cubic meter")]
    public void Parse_cubic_metre(string input)
    {
        Assert.Same(UnitOfVolume.CubicMetre, UnitOfVolume.Parse(input));
    }

    [Theory]
    [InlineData("cm³")]
    [InlineData("cc")]
    [InlineData("cm3")]
    [InlineData("cbcm")]
    [InlineData("cubic centimetre")]
    [InlineData("cubic centimeter")]
    public void Parse_cubic_centimetre(string input)
    {
        Assert.Same(UnitOfVolume.CubicCentimetre, UnitOfVolume.Parse(input));
    }

    [Theory]
    [InlineData("L")]
    [InlineData("l")]
    [InlineData("litre")]
    [InlineData("liter")]
    public void Parse_litre(string input)
    {
        Assert.Same(UnitOfVolume.Litre, UnitOfVolume.Parse(input));
    }

    [Theory]
    [InlineData("mL")]
    [InlineData("ml")]
    [InlineData("millilitre")]
    [InlineData("milliliter")]
    public void Parse_millilitre(string input)
    {
        Assert.Same(UnitOfVolume.Millilitre, UnitOfVolume.Parse(input));
    }

    [Fact]
    public void Parse_null_throws()
    {
        Assert.Throws<ArgumentNullException>(() => UnitOfVolume.Parse(null!));
    }

    [Fact]
    public void Parse_empty_throws()
    {
        Assert.Throws<ArgumentException>(() => UnitOfVolume.Parse(""));
    }

    [Fact]
    public void Parse_unknown_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => UnitOfVolume.Parse("unknown"));
    }

    [Fact]
    public void Parse_trims_whitespace()
    {
        Assert.Same(UnitOfVolume.Litre, UnitOfVolume.Parse("  L  "));
    }

    // ------------------------------------------------------------------
    //  Hashing
    // ------------------------------------------------------------------

    [Fact]
    public void GetRepeatableHashCode_same_unit_same_hash()
    {
        Assert.Equal(
            UnitOfVolume.Litre.GetRepeatableHashCode(),
            UnitOfVolume.Litre.GetRepeatableHashCode());
    }

    [Fact]
    public void GetRepeatableHashCode_different_units_differ()
    {
        Assert.NotEqual(
            UnitOfVolume.Litre.GetRepeatableHashCode(),
            UnitOfVolume.Millilitre.GetRepeatableHashCode());
    }

    // ------------------------------------------------------------------
    //  ToString
    // ------------------------------------------------------------------

    [Fact]
    public void ToString_returns_name()
    {
        Assert.Equal("litre", UnitOfVolume.Litre.ToString());
    }
}

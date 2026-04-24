// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

public class VolumeTests
{
    // ------------------------------------------------------------------
    //  Factory methods
    // ------------------------------------------------------------------

    [Fact]
    public void CubicMetre_sets_amount_and_units()
    {
        var v = Volume.CubicMetre(2.5);
        Assert.Equal(2.5, v.Amount);
        Assert.Same(UnitOfVolume.CubicMetre, v.Units);
    }

    [Fact]
    public void CubicCentimetre_sets_amount_and_units()
    {
        var v = Volume.CubicCentimetre(100);
        Assert.Equal(100, v.Amount);
        Assert.Same(UnitOfVolume.CubicCentimetre, v.Units);
    }

    [Fact]
    public void Litre_sets_amount_and_units()
    {
        var v = Volume.Litre(3.0);
        Assert.Equal(3.0, v.Amount);
        Assert.Same(UnitOfVolume.Litre, v.Units);
    }

    [Fact]
    public void Millilitre_sets_amount_and_units()
    {
        var v = Volume.Millilitre(500);
        Assert.Equal(500, v.Amount);
        Assert.Same(UnitOfVolume.Millilitre, v.Units);
    }

    // ------------------------------------------------------------------
    //  Equality
    // ------------------------------------------------------------------

    [Fact]
    public void Equal_same_amount_and_unit()
    {
        var a = Volume.Litre(1.5);
        var b = Volume.Litre(1.5);
        Assert.Equal(a, b);
    }

    [Fact]
    public void Not_equal_different_amount()
    {
        var a = Volume.Litre(1.0);
        var b = Volume.Litre(2.0);
        Assert.NotEqual(a, b);
    }

    // ------------------------------------------------------------------
    //  Comparison — same units
    // ------------------------------------------------------------------

    [Fact]
    public void CompareTo_same_units_less()
    {
        var small = Volume.Litre(1);
        var large = Volume.Litre(5);
        Assert.True(small.CompareTo(large) < 0);
    }

    [Fact]
    public void CompareTo_same_units_equal()
    {
        var a = Volume.Litre(3);
        var b = Volume.Litre(3);
        Assert.Equal(0, a.CompareTo(b));
    }

    // ------------------------------------------------------------------
    //  Comparison — cross-unit
    // ------------------------------------------------------------------

    [Fact]
    public void CompareTo_cross_unit_litre_vs_millilitre()
    {
        var litres = Volume.Litre(1);
        var millilitres = Volume.Millilitre(1000);
        Assert.Equal(0, litres.CompareTo(millilitres));
    }

    [Fact]
    public void CompareTo_cross_unit_cubic_metre_vs_litre()
    {
        var cubicMetre = Volume.CubicMetre(1);
        var litres = Volume.Litre(1000);
        Assert.Equal(0, cubicMetre.CompareTo(litres));
    }

    // ------------------------------------------------------------------
    //  Comparison operators
    // ------------------------------------------------------------------

    [Fact]
    public void Operator_less_than()
    {
        var a = Volume.Litre(1);
        var b = Volume.Litre(2);
        Assert.True(a < b);
        Assert.False(b < a);
    }

    [Fact]
    public void Operator_greater_than()
    {
        var a = Volume.Litre(2);
        var b = Volume.Litre(1);
        Assert.True(a > b);
        Assert.False(b > a);
    }

    [Fact]
    public void Operator_less_than_or_equal()
    {
        var a = Volume.Litre(1);
        var b = Volume.Litre(2);
        var c = Volume.Litre(1);
        Assert.True(a <= b);
        Assert.True(a <= c);
    }

    [Fact]
    public void Operator_greater_than_or_equal()
    {
        var a = Volume.Litre(2);
        var b = Volume.Litre(1);
        var c = Volume.Litre(2);
        Assert.True(a >= b);
        Assert.True(a >= c);
    }

    // ------------------------------------------------------------------
    //  ToString
    // ------------------------------------------------------------------

    [Fact]
    public void ToString_default_uses_own_units()
    {
        var v = Volume.Litre(2.5);
        Assert.Equal("2.5 L", v.ToString());
    }

    [Fact]
    public void ToString_with_unit_conversion()
    {
        var v = Volume.Litre(2.5);
        var result = v.ToString(null, null, UnitOfVolume.Millilitre);
        Assert.Equal("2500 mL", result);
    }

    [Fact]
    public void ConvertValueTo_converts_between_units()
    {
        var v = Volume.CubicMetre(1);

        Assert.Equal(1000, v.ConvertValueTo(UnitOfVolume.Litre));
    }

    [Fact]
    public void ConvertValueTo_default_volume_throws_invalid_operation_exception()
    {
        var v = default(Volume);

        Assert.Throws<InvalidOperationException>(() => v.ConvertValueTo(UnitOfVolume.Litre));
    }

    [Fact]
    public void ToString_default_volume_throws_invalid_operation_exception()
    {
        var v = default(Volume);

        Assert.Throws<InvalidOperationException>(() => v.ToString(null, null, UnitOfVolume.Litre));
    }

    [Fact]
    public void ToString_null_unit_throws()
    {
        var v = Volume.Litre(1);
        Assert.Throws<ArgumentNullException>(() => v.ToString(null, null, null!));
    }

    // ------------------------------------------------------------------
    //  Hashing
    // ------------------------------------------------------------------

    [Fact]
    public void GetRepeatableHashCode_same_value_same_hash()
    {
        var a = Volume.Litre(2.5);
        var b = Volume.Litre(2.5);
        Assert.Equal(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }

    [Fact]
    public void GetRepeatableHashCode_different_values_differ()
    {
        var a = Volume.Litre(1);
        var b = Volume.Litre(2);
        Assert.NotEqual(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

/// <summary>
/// A unit of measure for volume. Units of measure are considered equal if the
/// quantity of base units represented by the units of measure are equal.
/// </summary>
public sealed class UnitOfVolume : UnitOfMeasure<double>
{
    public static readonly UnitOfVolume CubicMetre = new(1000000000.000000, "cubic metre", "m³");
    public static readonly UnitOfVolume CubicCentimetre = new(1000.000000, "cubic centimetre", "cm³");
    public static readonly UnitOfVolume CubicMillimetre = new(1000.000000, "cubic millimetre", "mm³");
    public static readonly UnitOfVolume Litre = new(1000000.000000, "litre", "L");
    public static readonly UnitOfVolume Millilitre = new(1000.000000, "millilitre", "mL");
    public static readonly UnitOfVolume Microlitre = new(1.000000, "microlitre", "μL");

    public UnitOfVolume(double units, string name, string abbreviation) : base(units, name, abbreviation)
    {
    }

    public override UnitOfMeasure<double> BaseUnitOfMeasure => Microlitre;

    public static UnitOfVolume Parse(string nameOrAbbreviation)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameOrAbbreviation);

        return nameOrAbbreviation.Trim().ToLowerInvariant() switch
        {
            "m³" or "cubic metre" or "cubic meter" or "m3" or "cbm" => CubicMetre,
            "cm³" or "cc" or "cubic centimetre" or "cubic centimeter" or "cm3" or "cbcm" => CubicCentimetre,
            "l" or "litre" or "liter" => Litre,
            "ml" or "millilitre" or "milliliter" => Millilitre,
            _ => throw new ArgumentOutOfRangeException(nameof(nameOrAbbreviation)),
        };
    }
}

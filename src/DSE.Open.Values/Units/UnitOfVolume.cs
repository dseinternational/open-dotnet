// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Hashing;

namespace DSE.Open.Values.Units;

/// <summary>
/// A unit of measure for volume. Units of measure are considered equal if the
/// quantity of base units represented by the units of measure are equal.
/// </summary>
public sealed class UnitOfVolume : UnitOfMeasure<double>, IRepeatableHash64
{
    /// <summary>The cubic metre, equal to 1,000,000,000 microlitres.</summary>
    public static readonly UnitOfVolume CubicMetre = new(1000000000.000000, "cubic metre", "m³");

    /// <summary>The cubic centimetre, equal to 1,000 microlitres.</summary>
    public static readonly UnitOfVolume CubicCentimetre = new(1000.000000, "cubic centimetre", "cm³");

    /// <summary>The cubic millimetre, equal to 1 microlitre.</summary>
    public static readonly UnitOfVolume CubicMillimetre = new(1.000000, "cubic millimetre", "mm³");

    /// <summary>The litre, equal to 1,000,000 microlitres.</summary>
    public static readonly UnitOfVolume Litre = new(1000000.000000, "litre", "L");

    /// <summary>The millilitre, equal to 1,000 microlitres.</summary>
    public static readonly UnitOfVolume Millilitre = new(1000.000000, "millilitre", "mL");

    /// <summary>The microlitre, the base unit used for volume values.</summary>
    public static readonly UnitOfVolume Microlitre = new(1.000000, "microlitre", "μL");

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfVolume"/> class.
    /// </summary>
    /// <param name="units">The number of microlitres represented by one of these units.</param>
    /// <param name="name">The full name of the unit.</param>
    /// <param name="abbreviation">The abbreviation for the unit.</param>
    public UnitOfVolume(double units, string name, string abbreviation) : base(units, name, abbreviation)
    {
    }

    /// <inheritdoc/>
    public override UnitOfMeasure<double> BaseUnitOfMeasure => Microlitre;

    /// <summary>
    /// Parses the name or abbreviation of a unit of volume (for example, "L", "mL",
    /// "cm³", "m³" or any of the recognised English-language names and aliases).
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not a recognised unit.</exception>
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

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        var h0 = RepeatableHash64Provider.Default.GetRepeatableHashCode(BaseUnits);
        var h1 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Abbreviation);
        var h2 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Name);
        return RepeatableHash64Provider.Default.CombineHashCodes(h0, h1, h2);
    }
}

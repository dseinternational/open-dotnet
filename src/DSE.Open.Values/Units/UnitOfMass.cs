// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Hashing;

namespace DSE.Open.Values.Units;

/// <summary>
/// A unit of measure for mass. Units of measure are considered equal if the
/// quantity of base units represented by the units of measure are equal.
/// </summary>
public sealed class UnitOfMass : UnitOfMeasure<double>, IRepeatableHash64
{
    public static readonly UnitOfMass Gram = new(1, "Gram", "g");
    public static readonly UnitOfMass Kilogram = new(1000, "Kilogram", "kg");
    public static readonly UnitOfMass Milligram = new(1000, "Milligram", "mg");
    public static readonly UnitOfMass Ounce = new(28.349523125, "Ounce", "oz");
    public static readonly UnitOfMass Pound = new(453.59237, "Pound", "lb");

    public UnitOfMass(double units, string name, string abbreviation) : base(units, name, abbreviation)
    {
    }

    public override UnitOfMeasure<double> BaseUnitOfMeasure => Gram;

    public static UnitOfMass Parse(string abbreviation)
    {
        if (TryParse(abbreviation, out var unitOfMass))
        {
            return unitOfMass;
        }

        throw new ArgumentOutOfRangeException(nameof(abbreviation));
    }

    public static UnitOfMass Parse(ReadOnlySpan<char> abbreviation)
    {
        if (TryParse(abbreviation, out var unitOfMass))
        {
            return unitOfMass;
        }

        throw new ArgumentOutOfRangeException(nameof(abbreviation));
    }

    public static bool TryParse([NotNullWhen(true)] string? abbreviation, [NotNullWhen(true)] out UnitOfMass? unitOfMass)
    {
        if (abbreviation is not null)
        {
            return TryParse(abbreviation.AsSpan(), out unitOfMass);
        }

        unitOfMass = null;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<char> abbreviation, [NotNullWhen(true)] out UnitOfMass? unitOfMass)
    {
        unitOfMass = null;
        var trimmed = abbreviation.Trim();

        switch (trimmed.Length)
        {
            case 1:
                {
                    if (trimmed[0] == 'g')
                    {
                        unitOfMass = Gram;
                    }

                    break;
                }
            case 2 when trimmed[0] == 'k' && trimmed[1] == 'g':
                unitOfMass = Kilogram;
                break;
            case 2 when trimmed[0] == 'm' && trimmed[1] == 'g':
                unitOfMass = Milligram;
                break;
            case 2 when trimmed[0] == 'l' && trimmed[1] == 'b':
                unitOfMass = Pound;
                break;
            case 2:
                {
                    if (trimmed[0] == 'o' && trimmed[1] == 'z')
                    {
                        unitOfMass = Ounce;
                    }

                    break;
                }

            default:
                break;
        }

        return unitOfMass is not null;
    }

    public ulong GetRepeatableHashCode()
    {
        var h0 = RepeatableHash64Provider.Default.GetRepeatableHashCode(BaseUnits);
        var h1 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Abbreviation);
        var h2 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Name);
        return RepeatableHash64Provider.Default.CombineHashCodes(h0, h1, h2);
    }
}

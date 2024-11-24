// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Hashing;

namespace DSE.Open.Values.Units;

/// <summary>
/// A unit of measure for length. Units of measure are considered equal if the
/// quantity of base units represented by the units of measure are equal.
/// </summary>
public sealed class UnitOfLength : UnitOfMeasure<double>, IRepeatableHash64
{
    public static readonly UnitOfLength Kilometre = new(1_000_000, "kilometre", "km");
    public static readonly UnitOfLength Metre = new(1_000, "metre", "m");
    public static readonly UnitOfLength Centimetre = new(10, "centimetre", "cm");
    public static readonly UnitOfLength Millimetre = new(1, "millimetre", "mm");

    private UnitOfLength(double units, string name, string abbreviation) : base(units, name, abbreviation)
    {
    }

    public override UnitOfMeasure<double> BaseUnitOfMeasure => Millimetre;

    public static UnitOfLength Parse(string abbreviation)
    {
        if (TryParse(abbreviation, out var unitOfLength))
        {
            return unitOfLength;
        }

        throw new ArgumentOutOfRangeException(nameof(abbreviation));
    }

    public static UnitOfLength Parse(ReadOnlySpan<char> abbreviation)
    {
        if (TryParse(abbreviation, out var unitOfLength))
        {
            return unitOfLength;
        }

        throw new ArgumentOutOfRangeException(nameof(abbreviation));
    }

    public static bool TryParse([NotNullWhen(true)] string? abbreviation, [NotNullWhen(true)] out UnitOfLength? unitOfLength)
    {
        if (abbreviation is not null)
        {
            return TryParse(abbreviation.AsSpan(), out unitOfLength);
        }

        unitOfLength = null;
        return false;
    }

    public static bool TryParse(ReadOnlySpan<char> abbreviation, [NotNullWhen(true)] out UnitOfLength? unitOfLength)
    {
        unitOfLength = null;
        var trimmed = abbreviation.Trim();

        switch (trimmed.Length)
        {
            case 2 when trimmed[0] == 'm' && trimmed[1] == 'm':
                unitOfLength = Millimetre;
                break;
            case 2 when trimmed[0] == 'c' && trimmed[1] == 'm':
                unitOfLength = Centimetre;
                break;
            case 2:
                {
                    if (trimmed[0] == 'k' && trimmed[1] == 'm')
                    {
                        unitOfLength = Kilometre;
                    }

                    break;
                }
            case 1:
                {
                    if (trimmed[0] == 'm')
                    {
                        unitOfLength = Metre;
                    }

                    break;
                }

            default:
                break;
        }

        return unitOfLength is not null;
    }

    public ulong GetRepeatableHashCode()
    {
        var h0 = RepeatableHash64Provider.Default.GetRepeatableHashCode(BaseUnits);
        var h1 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Abbreviation);
        var h2 = RepeatableHash64Provider.Default.GetRepeatableHashCode(Name);
        return RepeatableHash64Provider.Default.CombineHashCodes(h0, h1, h2);
    }
}


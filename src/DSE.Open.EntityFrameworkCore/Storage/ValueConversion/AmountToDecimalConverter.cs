// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public sealed class AmountToDecimalConverter : ValueConverter<Amount, decimal>
{
    public static readonly AmountToDecimalConverter Default = new();

    public AmountToDecimalConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    // keep public for EF Core compiled models
    public static decimal ConvertTo(Amount value)
    {
        return value;
    }

    // keep public for EF Core compiled models
    public static Amount ConvertFrom(decimal value)
    {
        if (Amount.TryFromValue(value, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert value {value} to {nameof(Amount)}");
        return default; // unreachable
    }
}

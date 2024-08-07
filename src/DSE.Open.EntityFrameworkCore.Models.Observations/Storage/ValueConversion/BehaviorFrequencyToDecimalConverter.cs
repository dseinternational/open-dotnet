// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Observations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Models.Observations.Storage.ValueConversion;

public sealed class BehaviorFrequencyToDecimalConverter : ValueConverter<BehaviorFrequency, decimal>
{
    public static readonly BehaviorFrequencyToDecimalConverter Default = new();

    public BehaviorFrequencyToDecimalConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    private static decimal ConvertTo(BehaviorFrequency value)
    {
        unchecked
        {
            return (uint)value;
        }
    }

    private static BehaviorFrequency ConvertFrom(decimal value)
    {
        return (BehaviorFrequency)(uint)Math.Round(value, 0);
    }
}

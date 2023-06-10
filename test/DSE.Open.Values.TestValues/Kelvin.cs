// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.TestValues;

[RatioValue]
public readonly partial struct Kelvin : IRatioValue<Kelvin, double>
{
    static int ISpanSerializable<Kelvin>.MaxSerializedCharLength { get; } = 128; // TODO

    public static readonly Kelvin Zero;

    static Kelvin IRatioValue<Kelvin, double>.Zero => Zero;

    public static bool IsValidValue(double value) => value is >= 0 and <= 100;

}

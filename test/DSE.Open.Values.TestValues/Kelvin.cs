// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.TestValues;

[DivisibleValue]
public readonly partial struct Kelvin : IDivisibleValue<Kelvin, double>
{
    public static int MaxSerializedCharLength => 128; // TODO

    public static int MaxSerializedByteLength => 128; // TODO

    public static readonly Kelvin Zero;

    static Kelvin IDivisibleValue<Kelvin, double>.Zero => Zero;

    public static bool IsValidValue(double value)
    {
        return value is >= 0 and <= 100;
    }
}

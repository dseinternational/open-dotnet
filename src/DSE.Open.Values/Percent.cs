// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace DSE.Open.Values;

/// <summary>
/// A value that expresses a ratio as a signed value between 0 and 100 (values between -100 and 100).
/// </summary>
[DivisibleValue]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Percent : IDivisibleValue<Percent, double>, IUtf8SpanSerializable<Percent>
{
    public static int MaxSerializedCharLength => 128; // TODO

    public static int MaxSerializedByteLength => 128; // TODO

    public static Percent Zero { get; } = new(0);

    public static bool IsValidValue(double value)
    {
        return value is >= -100 and <= 100;
    }

    public Ratio ToRatio()
    {
        return (Ratio)(_value * 100);
    }

    public static explicit operator Percent(Ratio value)
    {
        return FromRatio(value);
    }

    public static Percent FromRatio(Ratio value)
    {
        return new((double)value * 100);
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace DSE.Open.Values;

/// <summary>
/// A value that expresses a ratio as a signed value between 0 and 1 (values between -1 and 1).
/// </summary>
[DivisibleValue]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Ratio : IDivisibleValue<Ratio, double>, IUtf8SpanSerializable<Ratio>
{
    public static int MaxSerializedCharLength => 128; // TODO

    public static int MaxSerializedByteLength => 128; // TODO

    public static Ratio Zero { get; } = new(0);

    public static bool IsValidValue(double value)
    {
        return value is >= -1 and <= 1;
    }

    public Percent ToPercent()
    {
        return (Percent)(_value * 100);
    }

    public static explicit operator Ratio(Percent value)
    {
        return FromPercent(value);
    }

    public static Ratio FromPercent(Percent value)
    {
        return new Ratio((double)value / 100);
    }
}

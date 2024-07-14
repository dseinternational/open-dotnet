// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

[ComparableValue]
public readonly partial struct Months : IComparableValue<Months, int>, IUtf8SpanSerializable<Months>
{
    private static readonly int s_maxAsciiLength = int.MaxValue.GetDigitCount();

    public static int MaxSerializedCharLength => s_maxAsciiLength;

    public static int MaxSerializedByteLength => s_maxAsciiLength;

    public static bool IsValidValue(int value)
    {
        return true;
    }

    public static Months FromYears(int years)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(years, int.MaxValue / 12);
        return new Months(years * 12);
    }
}

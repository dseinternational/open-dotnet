// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A rating of the frequency of an emerging behavior.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonByteValueConverter<BehaviorFrequency>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct BehaviorFrequency
    : IComparableValue<BehaviorFrequency, byte>,
      IUtf8SpanSerializable<BehaviorFrequency>,
      IRepeatableHash64,
      IValueProvider
{
    private const byte NeverValue = 0;
    private const byte EmergingValue = 10;
    private const byte DevelopingValue = 50;
    private const byte AchievedValue = 90;

    public static int MaxSerializedCharLength => 2;

    public static int MaxSerializedByteLength => 2;

    public ValueType ValueType => ValueType.Ordinal;

    public static bool IsValidValue(byte value)
    {
        return value is NeverValue or EmergingValue or DevelopingValue or AchievedValue;
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    bool IValueProvider.GetBinary()
    {
        return IValueProvider.ThrowValueMismatchException<bool>();
    }

    public byte GetOrdinal()
    {
        return _value;
    }

    ulong IValueProvider.GetCount()
    {
        return IValueProvider.ThrowValueMismatchException<ulong>();
    }

    decimal IValueProvider.GetAmount()
    {
        return IValueProvider.ThrowValueMismatchException<decimal>();
    }

    decimal IValueProvider.GetRatio()
    {
        return IValueProvider.ThrowValueMismatchException<decimal>();
    }

    decimal IValueProvider.GetFrequency()
    {
        return IValueProvider.ThrowValueMismatchException<decimal>();
    }

    public static BehaviorFrequency Never => new(NeverValue);

    /// <summary>
    /// Behavior is observed for the first time.
    /// </summary>
    public static BehaviorFrequency Emerging => new(EmergingValue);

    /// <summary>
    /// Behavior is observed sometimes.
    /// </summary>
    public static BehaviorFrequency Developing => new(DevelopingValue);

    /// <summary>
    /// Behavior occurs often/usually.
    /// </summary>
    public static BehaviorFrequency Achieved => new(AchievedValue);
}

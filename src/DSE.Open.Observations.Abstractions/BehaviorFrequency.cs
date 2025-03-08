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
      IObservationValue
{
    private const byte NeverValue = 0;
    private const byte EmergingValue = 10;
    private const byte DevelopingValue = 50;
    private const byte AchievedValue = 90;

    public static int MaxSerializedCharLength => 2;

    public static int MaxSerializedByteLength => 2;

    public MeasurementValueType ValueType => MeasurementValueType.Ordinal;

    public static bool IsValidValue(byte value)
    {
        return value is NeverValue or EmergingValue or DevelopingValue or AchievedValue;
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    bool IObservationValue.GetBinary()
    {
        return IObservationValue.ThrowValueMismatchException<bool>();
    }

    public byte GetOrdinal()
    {
        return _value;
    }

    ulong IObservationValue.GetCount()
    {
        return IObservationValue.ThrowValueMismatchException<ulong>();
    }

    decimal IObservationValue.GetAmount()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }

    decimal IObservationValue.GetRatio()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
    }

    decimal IObservationValue.GetFrequency()
    {
        return IObservationValue.ThrowValueMismatchException<decimal>();
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

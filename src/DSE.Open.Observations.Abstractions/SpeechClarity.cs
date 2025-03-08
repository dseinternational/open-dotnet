// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A rating of speech clarity.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonByteValueConverter<SpeechClarity>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct SpeechClarity
    : IComparableValue<SpeechClarity, byte>,
      IUtf8SpanSerializable<SpeechClarity>,
      IRepeatableHash64,
      IObservationValue
{
    private const byte UnclearValue = 10;
    private const byte DevelopingValue = 50;
    private const byte ClearValue = 90;

    public static int MaxSerializedCharLength => 2;

    public static int MaxSerializedByteLength => 2;

    public MeasurementValueType ValueType => MeasurementValueType.Ordinal;

    public static bool IsValidValue(byte value)
    {
        return value is UnclearValue or DevelopingValue or ClearValue;
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    bool IObservationValue.GetBinary()
    {
        return IObservationValue.ThrowValueMismatchException<bool>();
    }

    byte IObservationValue.GetOrdinal()
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

    /// <summary>
    /// Sometimes recognizable to familiar listeners, but unclear and usually not
    /// recognised by unfamiliar listeners.
    /// </summary>
    public static SpeechClarity Unclear => new(UnclearValue);

    /// <summary>
    /// Clarity is improving, but still not ideal: mostly understood by familiar listeners, but
    /// only occassionally recognised by unfamiliar listeners.
    /// </summary>
    public static SpeechClarity Developing => new(DevelopingValue);

    /// <summary>
    /// Clear and mostly understood by familiar and unfamiliar listeners.
    /// </summary>
    public static SpeechClarity Clear => new(ClearValue);
}

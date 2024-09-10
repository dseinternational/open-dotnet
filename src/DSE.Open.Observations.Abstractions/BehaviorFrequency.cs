// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A rating of the frequency of an emerging behavior.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonUInt32ValueConverter<BehaviorFrequency>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct BehaviorFrequency
    : IComparableValue<BehaviorFrequency, uint>,
      IUtf8SpanSerializable<BehaviorFrequency>
{
    private const uint NeverValue = 0;
    private const uint EmergingValue = 10;
    private const uint DevelopingValue = 50;
    private const uint AchievedValue = 90;

    public static int MaxSerializedCharLength => 2;

    public static int MaxSerializedByteLength => 2;

    public static bool IsValidValue(uint value)
    {
        return value is NeverValue or EmergingValue or DevelopingValue or AchievedValue;
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

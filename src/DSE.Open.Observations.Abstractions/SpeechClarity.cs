// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A rating of speech clarity.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<SpeechClarity, uint>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct SpeechClarity
    : IComparableValue<SpeechClarity, uint>,
      IUtf8SpanSerializable<SpeechClarity>
{
    private const uint EmergingValue = 10;
    private const uint DevelopingValue = 50;
    private const uint ClearValue = 90;

    public static int MaxSerializedCharLength => 2;

    public static int MaxSerializedByteLength => 2;

    public static bool IsValidValue(uint value)
    {
        return value is EmergingValue or DevelopingValue or ClearValue;
    }

    /// <summary>
    /// Sometimes recognizable to familiar listeners, but unclear and usually not
    /// recognised by unfamiliar listeners.
    /// </summary>
    public static SpeechClarity Unclear => new(EmergingValue);

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

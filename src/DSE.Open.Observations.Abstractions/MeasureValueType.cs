// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.Text.Json.Serialization;
using DSE.Open.Values;
using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

[EquatableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<MeasureValueType, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct MeasureValueType : IEquatableValue<MeasureValueType, AsciiString>
{
    public static int MaxSerializedCharLength => 32;

    public static bool IsValidValue(AsciiString value)
    {
        return value.Length > 6 && Lookup.ContainsKey(value);
    }

    public static readonly MeasureValueType BehaviorFrequency = new((AsciiString)"behavior_frequency", true);

    public static readonly MeasureValueType Count = new((AsciiString)"count", true);

    public static readonly MeasureValueType SpeechClarity = new((AsciiString)"speech_clarity", true);

    // todo

    public static readonly IReadOnlyCollection<MeasureValueType> All =
    [
        BehaviorFrequency,
        Count,
        SpeechClarity
    ];

    public static readonly IReadOnlyDictionary<AsciiString, MeasureValueType> Lookup = All.ToFrozenDictionary(r => r._value);
}

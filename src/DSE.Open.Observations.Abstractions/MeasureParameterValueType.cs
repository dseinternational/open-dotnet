// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.Text.Json.Serialization;
using DSE.Open.Values;
using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

[EquatableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<MeasureParameterValueType, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct MeasureParameterValueType : IEquatableValue<MeasureParameterValueType, AsciiString>
{
    public static int MaxSerializedCharLength => 32;

    public static bool IsValidValue(AsciiString value)
    {
        return value.Length > 6 && Lookup.ContainsKey(value);
    }

    public static readonly MeasureParameterValueType SpeechSound = new((AsciiString)"speech_sound", true);

    public static readonly MeasureParameterValueType WordId = new((AsciiString)"word_id", true);

    public static readonly MeasureParameterValueType SentenceId = new((AsciiString)"sentence_id", true);

    public static readonly IReadOnlyCollection<MeasureParameterValueType> All =
    [
        SpeechSound,
        WordId,
        SentenceId
    ];

    public static readonly IReadOnlyDictionary<AsciiString, MeasureParameterValueType> Lookup = All.ToFrozenDictionary(r => r._value);
}

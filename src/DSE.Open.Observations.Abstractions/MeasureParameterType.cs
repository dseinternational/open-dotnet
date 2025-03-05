// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.Text.Json.Serialization;
using DSE.Open.Values;
using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

[EquatableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<MeasureParameterType, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct MeasureParameterType : IEquatableValue<MeasureParameterType, AsciiString>
{
    public static int MaxSerializedCharLength => 32;

    public static bool IsValidValue(AsciiString value)
    {
        return value.Length > 6 && Lookup.ContainsKey(value);
    }

    public static readonly MeasureParameterType SpeechSound = new((AsciiString)"speech_sound", true);

    public static readonly MeasureParameterType WordId = new((AsciiString)"word_id", true);

    public static readonly MeasureParameterType SentenceId = new((AsciiString)"sentence_id", true);

    public static readonly IReadOnlyCollection<MeasureParameterType> All =
    [
        SpeechSound,
        WordId,
        SentenceId
    ];

    public static readonly IReadOnlyDictionary<AsciiString, MeasureParameterType> Lookup = All.ToFrozenDictionary(r => r._value);
}

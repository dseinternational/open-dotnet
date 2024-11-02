// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// Represents the selection of a choice between "Yes", "No" and "Not sure"/"Unsure".
/// To represent no response, use nullable.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<YesNoUnsure, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct YesNoUnsure
    : IEquatableValue<YesNoUnsure, AsciiString>,
      IUtf8SpanSerializable<YesNoUnsure>
{
    public static int MaxSerializedCharLength => 6;

    public static int MaxSerializedByteLength => 6;

    public static bool IsValidValue(AsciiString value)
    {
        return value.Length is > 1 and <= 6 && Lookup.ContainsKey(value);
    }

    public static readonly YesNoUnsure Yes = new((AsciiString)"yes", true);

    public static readonly YesNoUnsure No = new((AsciiString)"no", true);

    public static readonly YesNoUnsure Unsure = new((AsciiString)"unsure", true);

    public static readonly IReadOnlyCollection<YesNoUnsure> All =
    [
        Yes,
        No,
        Unsure
    ];

    public static readonly IReadOnlyDictionary<AsciiString, YesNoUnsure> Lookup = All.ToDictionary(r => r._value);
}

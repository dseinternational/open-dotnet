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
    /// <summary>
    /// Gets the maximum length, in characters, of the serialized representation of a <see cref="YesNoUnsure"/>.
    /// </summary>
    public static int MaxSerializedCharLength => 6;

    /// <summary>
    /// Gets the maximum length, in bytes, of the serialized representation of a <see cref="YesNoUnsure"/>.
    /// </summary>
    public static int MaxSerializedByteLength => 6;

    /// <summary>
    /// Determines whether the specified value is a valid <see cref="YesNoUnsure"/> value.
    /// </summary>
    public static bool IsValidValue(AsciiString value)
    {
        return value.Length is > 1 and <= 6 && Lookup.ContainsKey(value);
    }

    /// <summary>
    /// Represents the "Yes" choice.
    /// </summary>
    public static readonly YesNoUnsure Yes = new((AsciiString)"yes", true);

    /// <summary>
    /// Represents the "No" choice.
    /// </summary>
    public static readonly YesNoUnsure No = new((AsciiString)"no", true);

    /// <summary>
    /// Represents the "Not sure"/"Unsure" choice.
    /// </summary>
    public static readonly YesNoUnsure Unsure = new((AsciiString)"unsure", true);

    /// <summary>
    /// Gets all defined <see cref="YesNoUnsure"/> values.
    /// </summary>
    public static readonly IReadOnlyCollection<YesNoUnsure> All =
    [
        Yes,
        No,
        Unsure
    ];

    /// <summary>
    /// A lookup of all defined <see cref="YesNoUnsure"/> values keyed by their underlying value.
    /// </summary>
    public static readonly IReadOnlyDictionary<AsciiString, YesNoUnsure> Lookup = All.ToDictionary(r => r._value);
}

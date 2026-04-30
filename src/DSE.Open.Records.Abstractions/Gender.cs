// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Records;

/// <summary>
/// Identifies a recorded gender.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<Gender, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Gender : IEquatableValue<Gender, AsciiString>, IUtf8SpanSerializable<Gender>
{
    /// <inheritdoc/>
    public static int MaxSerializedCharLength => 6;

    /// <inheritdoc/>
    public static int MaxSerializedByteLength => 6;

    /// <inheritdoc/>
    public static bool IsValidValue(AsciiString value)
    {
        return value.Length is > 1 and <= 6 && Lookup.ContainsKey(value);
    }

    /// <summary>
    /// The gender value representing female.
    /// </summary>
    public static readonly Gender Female = new((AsciiString)"female", true);

    /// <summary>
    /// The gender value representing male.
    /// </summary>
    public static readonly Gender Male = new((AsciiString)"male", true);

    /// <summary>
    /// The gender value representing other.
    /// </summary>
    public static readonly Gender Other = new((AsciiString)"other", true);

    /// <summary>
    /// Gets all defined <see cref="Gender"/> values.
    /// </summary>
    public static readonly IReadOnlyCollection<Gender> All =
    [
        Female,
        Male,
        Other
    ];

    /// <summary>
    /// Gets a lookup of defined <see cref="Gender"/> values keyed by their underlying value.
    /// </summary>
    public static readonly IReadOnlyDictionary<AsciiString, Gender> Lookup = All.ToDictionary(r => r._value);
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Records;

/// <summary>
/// Identifies a recorded biological sex.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<BiologicalSex, AsciiString>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct BiologicalSex : IEquatableValue<BiologicalSex, AsciiString>, IUtf8SpanSerializable<BiologicalSex>
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
    /// The biological sex value representing female.
    /// </summary>
    public static readonly BiologicalSex Female = new((AsciiString)"female", true);

    /// <summary>
    /// The biological sex value representing male.
    /// </summary>
    public static readonly BiologicalSex Male = new((AsciiString)"male", true);

    /// <summary>
    /// Gets all defined <see cref="BiologicalSex"/> values.
    /// </summary>
    public static readonly IReadOnlyCollection<BiologicalSex> All =
    [
        Female,
        Male,
    ];

    /// <summary>
    /// Gets a lookup of defined <see cref="BiologicalSex"/> values keyed by their underlying value.
    /// </summary>
    public static readonly IReadOnlyDictionary<AsciiString, BiologicalSex> Lookup = All.ToDictionary(r => r._value);
}

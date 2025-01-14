// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// Indicates the modality of a <see cref="Sign"/>.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<SignModality, AsciiString>))]
public readonly partial struct SignModality
    : IEquatableValue<SignModality, AsciiString>,
      IUtf8SpanSerializable<SignModality>,
      IRepeatableHash64
{
    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

    public SignModality(string value) : this((AsciiString)value)
    {
    }

    private SignModality(string value, bool skipValidation = false) : this((AsciiString)value, skipValidation)
    {
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

    /// <summary>
    /// A pictured sign.
    /// </summary>
    public static readonly SignModality Pictured = new("pictured", true);

    /// <summary>
    /// A spoken sign.
    /// </summary>
    public static readonly SignModality Spoken = new("spoken", true);

    /// <summary>
    /// A (visual) gesture, such as a sign language sign.
    /// </summary>
    public static readonly SignModality Gestured = new("gestured", true);

    /// <summary>
    /// A written sign.
    /// </summary>
    public static readonly SignModality Written = new("written", true);

    public static readonly IReadOnlySet<SignModality> All = FrozenSet.ToFrozenSet(
    [
        Pictured,
        Spoken,
        Gestured,
        Written,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues = FrozenSet.ToFrozenSet(All.Select(x => x._value));
}

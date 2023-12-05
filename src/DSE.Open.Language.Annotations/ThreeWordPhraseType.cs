// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

[EquatableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<ThreeWordPhraseType, AsciiString>))]
public readonly partial struct ThreeWordPhraseType
    : IEquatableValue<ThreeWordPhraseType, AsciiString>,
      IUtf8SpanSerializable<ThreeWordPhraseType>
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => 32;

    public ThreeWordPhraseType(string value) : this((AsciiString)value)
    {
    }

    private ThreeWordPhraseType(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator ThreeWordPhraseType(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new ThreeWordPhraseType(value);
    }

    /// <summary>
    /// Indicates a <b>noun + verb + noun</b> <c>[(NOUN|PROPN)+VERB+(NOUN|PROPN)]</c>
    /// (agent - action - object)
    /// construction - for example "dog bark", "baby eat".
    /// </summary>
    public static readonly ThreeWordPhraseType AgentActionObject = new("agent-action-object", true);

    /// <summary>
    /// TODO
    /// </summary>
    public static readonly ThreeWordPhraseType AgentActionLocative = new("agent-action-locative", true);

    /// <summary>
    /// TODO
    /// </summary>
    public static readonly ThreeWordPhraseType AgentObjectLocative = new("agent-object-locative", true);

    /// <summary>
    /// <c>[NOUN+ADP+NOUN|VERB+ADP+NOUN]</c> "toy in box", "hide under bed"
    /// </summary>
    public static readonly ThreeWordPhraseType Preposition = new("preposition", true);

    // TODO: complete

    public static readonly FrozenSet<ThreeWordPhraseType> All = FrozenSet.ToFrozenSet(
    [
        AgentActionObject,
        AgentActionLocative,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues =
        FrozenSet.ToFrozenSet(All.Select(x => x._value));
}

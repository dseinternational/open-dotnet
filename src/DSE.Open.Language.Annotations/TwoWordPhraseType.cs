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
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<TwoWordPhraseType, AsciiString>))]
public readonly partial struct TwoWordPhraseType : IEquatableValue<TwoWordPhraseType, AsciiString>, IUtf8SpanSerializable<TwoWordPhraseType>
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => 32;

    public TwoWordPhraseType(string value) : this((AsciiString)value)
    {
    }

    private TwoWordPhraseType(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator TwoWordPhraseType(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new TwoWordPhraseType(value);
    }

    /// <summary>
    /// Indicates a <b>noun + verb</b> <c>[(NOUN|PROPN)+VERB]</c> (agent + action)
    /// construction - for example "dog bark", "baby eat".
    /// </summary>
    public static readonly TwoWordPhraseType AgentAction = new("agent-action", true);

    /// <summary>
    /// Indicates a <b>verb + noun</b> <c>[VERB+(NOUN|PROPN)]</c> (action + object)
    /// construction - for example "throw ball", "drink juice", "washing cat".
    /// </summary>
    public static readonly TwoWordPhraseType ActionObject = new("action-object", true);

    /// <summary>
    /// Indicates a <b>verb + noun</b> <c>[(NOUN|PROPN)+NOUN]</c> (agent + object)
    /// construction - for example "daddy toy" (daddy get the toy),
    /// "baby ball" (give the baby the ball).
    /// </summary>
    public static readonly TwoWordPhraseType AgentObject = new("agent-object", true);

    /// <summary>
    /// Indicates a <b>noun + possessive + noun</b> <c>[NOUN+PART+NOUN]</c> (possessive)
    /// construction - for example "daddy car" (daddy's car), "dog bowl" (dog's bowl).
    /// </summary>
    public static readonly TwoWordPhraseType Possessive = new("possessive", true);

    /// <summary>
    /// Indicates an <b>adjective + noun</b> or <b>noun + adjective</b>
    /// <c>ADJ+NOUN|NOUN+ADJ</c> (descriptive) construction - for example
    /// "big truck", "blue ball", "dog wet", "shoe dirty".
    /// </summary>
    public static readonly TwoWordPhraseType Descriptive = new("descriptive", true);

    /// Indicates a <b>preposition + noun</b> or <b>noun + preposition</b>
    /// <c>[ADP+NOUN]</c> (locative, position/place) construction - for example
    /// "on chair", "in kitchen", "under table".
    public static readonly TwoWordPhraseType Locative = new("locative", true);

    /// <summary>
    /// Indicates an <b>verb + adverb</b> <c>[VERB+ADV]</c> (temporal)
    /// construction - for example "drink" (I want to drink now),
    /// "eat later".
    /// </summary>
    public static readonly TwoWordPhraseType Temporal = new("temporal", true);

    /// <summary>
    /// Indicates a <b>number + noun</b> <c>[NUM+NOUN]</c> (quantitative)
    /// construction - for example "two balls", "three bags".
    /// </summary>
    public static readonly TwoWordPhraseType Quantitative = new("temporal", true);

    /// <summary>
    /// Indicates a <b>noun + noun</b> <c>[NOUN+NOUN]</c> construction where the
    /// two nouns are not in a possessive relationship but are commonly associated
    /// with each other - for example "sock shoe", "bowl spoon".
    /// </summary>
    public static readonly TwoWordPhraseType Conjunctive = new("conjunctive", true);

    /// <summary>
    /// Indicates a <b>determiner + noun</b> <c>DET+NOUN</c> (existence)
    /// construction  - for example, "that ball", "this toy".
    /// </summary>
    public static readonly TwoWordPhraseType Existence = new("existence", true);

    /// <summary>
    /// Indicates a <b>adjective + noun</b> <c>[ADJ+NOUN]</c> (recurrence)
    /// construction that expresses the idea of repetition or something happening
    /// again - for example "another ball", "more juice".
    /// </summary>
    public static readonly TwoWordPhraseType Recurrence = new("recurrence", true);

    public static readonly TwoWordPhraseType NonExistence = new("non-existence", true);

    public static readonly TwoWordPhraseType Rejection = new("rejection", true);

    public static readonly TwoWordPhraseType Denial = new("denial", true);

    public static readonly FrozenSet<TwoWordPhraseType> All = FrozenSet.ToFrozenSet(
    [
        ActionObject,
        AgentAction,
        AgentObject,
        Conjunctive,
        Denial,
        Descriptive,
        Existence,
        Locative,
        NonExistence,
        Possessive,
        Quantitative,
        Recurrence,
        Rejection,
        Temporal,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues =
        FrozenSet.ToFrozenSet(All.Select(x => x._value));
}

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
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<SemanticClassificationTag, AsciiString>))]
public readonly partial struct SemanticClassificationTag
    : IEquatableValue<SemanticClassificationTag, AsciiString>,
      IUtf8SpanSerializable<SemanticClassificationTag>
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => 32;

    public SemanticClassificationTag(string value) : this((AsciiString)value)
    {
    }

    private SemanticClassificationTag(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator SemanticClassificationTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new SemanticClassificationTag(value);
    }


    /// <summary>
    /// Indicates a <b>noun + verb</b> <c>[(NOUN|PROPN)+VERB]</c> (agent + action)
    /// construction - for example "dog bark", "baby eat".
    /// </summary>
    public static readonly SemanticClassificationTag AgentAction = new("agent-action", true);

    /// <summary>
    /// Indicates a <b>verb + noun</b> <c>[VERB+(NOUN|PROPN)]</c> (action + object)
    /// construction - for example "throw ball", "drink juice", "washing cat".
    /// </summary>
    public static readonly SemanticClassificationTag ActionObject = new("action-object", true);

    /// <summary>
    /// Indicates a <b>verb + noun</b> <c>[(NOUN|PROPN)+NOUN]</c> (agent + object)
    /// construction - for example "daddy toy" (daddy get the toy),
    /// "baby ball" (give the baby the ball).
    /// </summary>
    public static readonly SemanticClassificationTag AgentObject = new("agent-object", true);

    /// <summary>
    /// Indicates a <b>noun + possessive + noun</b> <c>[NOUN+PART+NOUN]</c> (possessive)
    /// construction - for example "daddy car" (daddy's car), "dog bowl" (dog's bowl).
    /// </summary>
    public static readonly SemanticClassificationTag PossessorPossession = new("possessor-possession", true);

    /// <summary>
    /// Indicates an <b>adjective + noun</b> or <b>noun + adjective</b>
    /// <c>ADJ+NOUN|NOUN+ADJ</c> (descriptive) construction - for example
    /// "big truck", "blue ball", "dog wet", "shoe dirty".
    /// </summary>
    public static readonly SemanticClassificationTag AttributeEntity = new("attribute-entity", true);

    /// Indicates a <b>preposition + noun</b> or <b>noun + preposition</b>
    /// <c>[ADP+NOUN]</c> (location, position/place) construction - for example
    /// "on chair", "in kitchen", "under table".
    public static readonly SemanticClassificationTag Location = new("location", true);

    /// <summary>
    /// e.g. "sit chair"
    /// </summary>
    public static readonly SemanticClassificationTag ActionLocation = new("action-location", true);

    /// <summary>
    /// e.g. "bear bed"
    /// </summary>
    public static readonly SemanticClassificationTag EntityLocation = new("entity-location", true);

    /// <summary>
    /// Indicates an <b>verb + adverb</b> <c>[VERB+ADV]</c> (temporal)
    /// construction - for example "drink" (I want to drink now),
    /// "eat later".
    /// </summary>
    public static readonly SemanticClassificationTag Temporal = new("temporal", true);

    /// <summary>
    /// Indicates a <b>number + noun</b> <c>[NUM+NOUN]</c> (quantitative)
    /// construction - for example "two balls", "three bags".
    /// </summary>
    public static readonly SemanticClassificationTag Quantitative = new("quantitative", true);

    /// <summary>
    /// Indicates a <b>noun + noun</b> <c>[NOUN+NOUN]</c> construction where the
    /// two nouns are not in a possessive relationship but are commonly associated
    /// with each other - for example "sock shoe", "bowl spoon".
    /// </summary>
    public static readonly SemanticClassificationTag Conjunctive = new("conjunctive", true);


    public static readonly SemanticClassificationTag Declarative = new("declarative", true);


    public static readonly SemanticClassificationTag Imperative = new("imperative", true);

    /// <summary>
    /// Indicates a <b>determiner + noun</b> <c>DET+NOUN</c> (existence)
    /// construction  - for example, "that ball", "this toy".
    /// </summary>
    public static readonly SemanticClassificationTag DemonstrativeEntity = new("demonstrative-entity", true);

    /// <summary>
    /// Indicates a <b>adjective + noun</b> <c>[ADJ+NOUN]</c> (recurrence)
    /// construction that expresses the idea of repetition or something happening
    /// again - for example "another ball", "more juice".
    /// </summary>
    public static readonly SemanticClassificationTag Recurrence = new("recurrence", true);

    public static readonly SemanticClassificationTag NonExistence = new("non-existence", true);

    public static readonly SemanticClassificationTag Rejection = new("rejection", true);

    public static readonly SemanticClassificationTag Denial = new("denial", true);

    /// <summary>
    /// Indicates a <b>noun + verb + noun</b> <c>[(NOUN|PROPN)+VERB+(NOUN|PROPN)]</c>
    /// (agent - action - object)
    /// construction - for example "dog bark", "baby eat".
    /// </summary>
    public static readonly SemanticClassificationTag AgentActionObject = new("agent-action-object", true);

    /// <summary>
    /// TODO
    /// </summary>
    public static readonly SemanticClassificationTag AgentActionLocation = new("agent-action-location", true);

    /// <summary>
    /// TODO
    /// </summary>
    public static readonly SemanticClassificationTag AgentObjectLocation = new("agent-object-location", true);

    /// <summary>
    /// <c>[NOUN+ADP+NOUN|VERB+ADP+NOUN]</c> "toy in box", "hide under bed"
    /// </summary>
    public static readonly SemanticClassificationTag Preposition = new("preposition", true);

    // TODO: complete

    public static readonly FrozenSet<SemanticClassificationTag> All = FrozenSet.ToFrozenSet(
    [
        ActionLocation,
        ActionObject,
        AgentAction,
        AgentActionLocation,
        AgentActionObject,
        AgentObject,
        AgentObjectLocation,
        AttributeEntity,
        Conjunctive,
        Declarative,
        DemonstrativeEntity,
        Denial,
        EntityLocation,
        Imperative,
        Location,
        NonExistence,
        PossessorPossession,
        Preposition,
        Quantitative,
        Recurrence,
        Rejection,
        Temporal,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues =
        FrozenSet.ToFrozenSet(All.Select(x => x._value));
}

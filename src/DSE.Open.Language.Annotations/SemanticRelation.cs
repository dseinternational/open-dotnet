// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<SemanticRelation, AsciiString>))]
public readonly partial struct SemanticRelation
    : IEquatableValue<SemanticRelation, AsciiString>,
      IUtf8SpanSerializable<SemanticRelation>
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => 32;

    public SemanticRelation(string value) : this((AsciiString)value)
    {
    }

    private SemanticRelation(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator SemanticRelation(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

    /// <summary>
    /// Indicates a <b>noun + verb</b> <c>[(NOUN|PROPN)+VERB]</c> (agent + action)
    /// construction - for example "dog bark", "baby eat".
    /// </summary>
    public static readonly SemanticRelation AgentAction = new("agent-action", true);

    /// <summary>
    /// Indicates a <b>verb + noun</b> <c>[VERB+(NOUN|PROPN)]</c> (action + object)
    /// construction - for example "throw ball", "drink juice", "washing cat".
    /// </summary>
    public static readonly SemanticRelation ActionObject = new("action-object", true);

    /// <summary>
    /// Indicates a <b>noun or pronoun + noun</b> <c>[(NOUN|PROPN)+NOUN]</c> (agent + object)
    /// construction - for example "daddy toy" (daddy get the toy),
    /// "baby ball" (give the baby the ball).
    /// </summary>
    public static readonly SemanticRelation AgentObject = new("agent-object", true);

    /// <summary>
    /// Indicates a <b>noun + possessive + noun</b> <c>[NOUN+PART+NOUN]</c> (possessive)
    /// construction - for example "daddy car" (daddy's car), "dog bowl" (dog's bowl).
    /// </summary>
    public static readonly SemanticRelation PossessorPossession = new("possessor-possession", true);

    /// <summary>
    /// Indicates an <b>adjective + noun</b> or <b>noun + adjective</b>
    /// <c>ADJ+NOUN|NOUN+ADJ</c> (descriptive) construction - for example
    /// "big truck", "blue ball", "dog wet", "shoe dirty".
    /// </summary>
    public static readonly SemanticRelation AttributeEntity = new("attribute-entity", true);

    /// Indicates a <b>preposition + noun</b> or <b>noun + preposition</b>
    /// <c>[ADP+NOUN]</c> (location, position/place) construction - for example
    /// "on chair", "in kitchen", "under table".
    /// <c>[NOUN+ADP+NOUN|VERB+ADP+NOUN]</c> "toy in box", "hide under bed"
    public static readonly SemanticRelation Location = new("location", true);

    /// <summary>
    /// e.g. "sit chair"
    /// </summary>
    public static readonly SemanticRelation ActionLocation = new("action-location", true);

    /// <summary>
    /// e.g. "bear bed"
    /// </summary>
    public static readonly SemanticRelation EntityLocation = new("entity-location", true);

    /// <summary>
    /// Indicates an <b>verb + adverb</b> <c>[VERB+ADV]</c> (temporal)
    /// construction - for example "drink" (I want to drink now),
    /// "eat later".
    /// </summary>
    public static readonly SemanticRelation Temporal = new("temporal", true);

    /// <summary>
    /// Indicates a <b>number + noun</b> <c>[NUM+NOUN]</c> (quantitative)
    /// construction - for example "two balls", "three bags".
    /// </summary>
    public static readonly SemanticRelation Quantitative = new("quantitative", true);

    /// <summary>
    /// Indicates a <b>noun + noun</b> <c>[NOUN+NOUN]</c> construction where the
    /// two nouns are not in a possessive relationship but are commonly associated
    /// with each other - for example "sock shoe", "bowl spoon".
    /// </summary>
    public static readonly SemanticRelation Conjunctive = new("conjunctive", true);

    /// <summary>
    /// Indicates a <b>determiner + noun</b> <c>DET+NOUN</c> (existence)
    /// construction  - for example, "that ball", "this toy".
    /// </summary>
    public static readonly SemanticRelation DemonstrativeEntity = new("demonstrative-entity", true);

    /// <summary>
    /// Indicates a <b>adjective + noun</b> <c>[ADJ+NOUN]</c> (recurrence)
    /// construction that expresses the idea of repetition or something happening
    /// again - for example "another ball", "more juice".
    /// </summary>
    public static readonly SemanticRelation Recurrence = new("recurrence", true);

    /// <summary>
    /// Non-existence, denial, or rejection of an object or action. "no ball", "no juice", "no eat", "all gone", "no more".
    /// </summary>
    public static readonly SemanticRelation NonExistence = new("non-existence", true);

    /// <summary>
    /// Indicates a <b>noun + verb + noun</b> <c>[(NOUN|PROPN)+VERB+(NOUN|PROPN)]</c>
    /// (agent - action - object)
    /// construction - for example "dog bark", "baby eat".
    /// </summary>
    public static readonly SemanticRelation AgentActionObject = new("agent-action-object", true);

    /// <summary>
    /// TODO
    /// </summary>
    public static readonly SemanticRelation AgentActionLocation = new("agent-action-location", true);

    /// <summary>
    /// TODO
    /// </summary>
    public static readonly SemanticRelation ActionObjectLocation = new("action-object-location", true);

    // TODO: complete

    public static readonly FrozenSet<SemanticRelation> All = FrozenSet.ToFrozenSet(
    [
        ActionLocation,
        ActionObject,
        AgentAction,
        AgentActionLocation,
        AgentActionObject,
        AgentObject,
        ActionObjectLocation,
        AttributeEntity,
        Conjunctive,
        DemonstrativeEntity,
        EntityLocation,
        Location,
        NonExistence,
        PossessorPossession,
        Quantitative,
        Recurrence,
        Temporal,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues =
        FrozenSet.ToFrozenSet(All.Select(x => x._value));
}

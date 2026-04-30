// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// A universal dependency relation tag
/// (<see href="https://universaldependencies.org/u/dep/index.html"/>).
/// </summary>
[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UniversalRelationTag, AsciiString>))]
public readonly partial struct UniversalRelationTag
    : IEquatableValue<UniversalRelationTag, AsciiString>,
      IUtf8SpanSerializable<UniversalRelationTag>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters required to format a value as a string.
    /// </summary>
    public static int MaxSerializedCharLength => 32;

    /// <summary>
    /// The maximum number of bytes required to format a value as UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => 32;

    /// <summary>
    /// Initializes a new <see cref="UniversalRelationTag"/> from the specified value.
    /// </summary>
    public UniversalRelationTag(string value) : this((AsciiString)value)
    {
    }

    private UniversalRelationTag(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    /// <summary>
    /// Gets the length, in characters, of the tag value.
    /// </summary>
    public int Length => _value.Length;

    /// <summary>
    /// Gets the underlying tag value.
    /// </summary>
    public AsciiString Value => _value;

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a valid universal dependency relation tag.
    /// </summary>
    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    /// <summary>
    /// Explicitly converts a string value to a <see cref="UniversalRelationTag"/>.
    /// </summary>
    public static explicit operator UniversalRelationTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

    /// <summary><c>acl</c>: clausal modifier of noun.</summary>
    public static readonly UniversalRelationTag ClausalModifierOfNoun = new("acl", true);

    /// <summary><c>acl:relcl</c>: relative clause modifier.</summary>
    public static readonly UniversalRelationTag RelativeClauseModifier = new("acl:relcl", true);

    /// <summary><c>advcl</c>: adverbial clause modifier.</summary>
    public static readonly UniversalRelationTag AdverbialClauseModifier = new("advcl", true);

    /// <summary><c>advmod</c>: adverbial modifier.</summary>
    public static readonly UniversalRelationTag AdverbialModifier = new("advmod", true);

    /// <summary><c>advmod:emph</c>: emphasizing word.</summary>
    public static readonly UniversalRelationTag EmphasizingWord = new("advmod:emph", true);

    /// <summary><c>advmod:lmod</c>: locative adverbial modifier.</summary>
    public static readonly UniversalRelationTag LocativeAdverbialModifier = new("advmod:lmod", true);

    /// <summary><c>amod</c>: adjectival modifier.</summary>
    public static readonly UniversalRelationTag AdjectivalModifier = new("amod", true);

    /// <summary><c>appos</c>: appositional modifier.</summary>
    public static readonly UniversalRelationTag AppositionalModifier = new("appos", true);

    /// <summary><c>aux</c>: auxiliary.</summary>
    public static readonly UniversalRelationTag Auxiliary = new("aux", true);

    /// <summary><c>aux:pass</c>: passive auxiliary.</summary>
    public static readonly UniversalRelationTag PassiveAuxiliary = new("aux:pass", true);

    /// <summary><c>case</c>: case marking.</summary>
    public static readonly UniversalRelationTag CaseMarking = new("case", true);

    /// <summary><c>cc</c>: coordinating conjunction.</summary>
    public static readonly UniversalRelationTag CoordinatingConjunction = new("cc", true);

    /// <summary><c>cc:preconj</c>: preconjunct.</summary>
    public static readonly UniversalRelationTag Preconjunct = new("cc:preconj", true);

    /// <summary><c>ccomp</c>: clausal complement.</summary>
    public static readonly UniversalRelationTag ClausalComplement = new("ccomp", true);

    /// <summary><c>clf</c>: classifier.</summary>
    public static readonly UniversalRelationTag Classifier = new("clf", true);

    /// <summary><c>compound</c>: compound.</summary>
    public static readonly UniversalRelationTag Compound = new("compound", true);

    /// <summary><c>compound:lvc</c>: light verb construction.</summary>
    public static readonly UniversalRelationTag LightVerbConstruction = new("compound:lvc", true);

    /// <summary><c>compound:prt</c>: phrasal verb particle.</summary>
    public static readonly UniversalRelationTag PhrasalVerbParticle = new("compound:prt", true);

    /// <summary><c>compound:redup</c>: reduplicated compounds.</summary>
    public static readonly UniversalRelationTag ReduplicatedCompounds = new("compound:redup", true);

    /// <summary><c>compound:svc</c>: serial verb compounds.</summary>
    public static readonly UniversalRelationTag SerialVerbCompounds = new("compound:svc", true);

    /// <summary><c>conj</c>: conjunct.</summary>
    public static readonly UniversalRelationTag Conjunct = new("conj", true);

    /// <summary><c>cop</c>: copula.</summary>
    public static readonly UniversalRelationTag Copula = new("cop", true);

    /// <summary><c>csubj</c>: clausal subject.</summary>
    public static readonly UniversalRelationTag ClausalSubject = new("csubj", true);

    /// <summary><c>csubj:pass</c>: clausal passive subject.</summary>
    public static readonly UniversalRelationTag ClausalPassiveSubject = new("csubj:pass", true);

    /// <summary><c>dep</c>: unspecified dependency.</summary>
    public static readonly UniversalRelationTag UnspecifiedDependency = new("dep", true);

    /// <summary><c>det</c>: determiner.</summary>
    public static readonly UniversalRelationTag Determiner = new("det", true);

    /// <summary><c>det:numgov</c>: pronominal quantifier governing.</summary>
    public static readonly UniversalRelationTag PronominalQuantifierGoverning = new("det:numgov", true);

    /// <summary><c>det:nummod</c>: pronominal quantifier agreeing.</summary>
    public static readonly UniversalRelationTag PronominalQuantifierAgreeing = new("det:nummod", true);

    /// <summary><c>det:poss</c>: possessive determiner.</summary>
    public static readonly UniversalRelationTag PossessiveDeterminer = new("det:poss", true);

    /// <summary><c>discourse</c>: discourse element.</summary>
    public static readonly UniversalRelationTag DiscourseElement = new("discourse", true);

    /// <summary><c>dislocated</c>: dislocated elements.</summary>
    public static readonly UniversalRelationTag DislocatedElements = new("dislocated", true);

    /// <summary><c>dobj</c>: direct object.</summary>
    public static readonly UniversalRelationTag DirectObject = new("dobj", true);

    /// <summary><c>expl</c>: expletive.</summary>
    public static readonly UniversalRelationTag Expletive = new("expl", true);

    /// <summary><c>expl:impers</c>: impersonal expletive.</summary>
    public static readonly UniversalRelationTag ImpersonalExpletive = new("expl:impers", true);

    /// <summary><c>expl:pass</c>: reflexive pronoun used as a reflexive passive.</summary>
    public static readonly UniversalRelationTag ReflexivePronounReflexivePassive = new("expl:pass", true);

    /// <summary><c>expl:pv</c>: reflexive clitic with a reflexive verb.</summary>
    public static readonly UniversalRelationTag ReflexiveCliticWithReflexiveverb = new("expl:pv", true);

    /// <summary><c>fixed</c>: fixed multi-word expression.</summary>
    public static readonly UniversalRelationTag FixedMultiwordExpression = new("fixed", true);

    /// <summary><c>flat</c>: flat multi-word expression.</summary>
    public static readonly UniversalRelationTag FlatMultiwordExpression = new("flat", true);

    /// <summary><c>flat:foreign</c>: foreign words.</summary>
    public static readonly UniversalRelationTag ForeignWords = new("flat:foreign", true);

    /// <summary><c>flat:name</c>: names.</summary>
    public static readonly UniversalRelationTag Names = new("flat:name", true);

    /// <summary><c>goeswith</c>: goes with.</summary>
    public static readonly UniversalRelationTag GoesWith = new("goeswith", true);

    /// <summary><c>iobj</c>: indirect object.</summary>
    public static readonly UniversalRelationTag IndirectObject = new("iobj", true);

    /// <summary><c>list</c>: list.</summary>
    public static readonly UniversalRelationTag List = new("list", true);

    /// <summary><c>mark</c>: marker.</summary>
    public static readonly UniversalRelationTag Marker = new("mark", true);

    /// <summary><c>nmod</c>: nominal modifier.</summary>
    public static readonly UniversalRelationTag NominalModifier = new("nmod", true);

    /// <summary><c>nmod:poss</c>: possessive nominal modifier.</summary>
    public static readonly UniversalRelationTag PossessiveNominalModifier = new("nmod:poss", true);

    /// <summary><c>nmod:tmod</c>: temporal modifier.</summary>
    public static readonly UniversalRelationTag TemporalModifier = new("nmod:tmod", true);

    /// <summary>
    /// <c>nsubj</c>
    /// </summary>
    public static readonly UniversalRelationTag NominalSubject = new("nsubj", true);

    /// <summary>
    /// <c>nsubj:pass</c>
    /// </summary>
    public static readonly UniversalRelationTag PassiveNominalSubject = new("nsubj:pass", true);

    /// <summary><c>nummod</c>: numeric modifier.</summary>
    public static readonly UniversalRelationTag NumericModifier = new("nummod", true);

    /// <summary><c>nummod:gov</c>: numeric modifier governing the noun.</summary>
    public static readonly UniversalRelationTag NumericModifierGoverningNoun = new("nummod:gov", true);

    /// <summary>
    /// <c>obj</c>
    /// </summary>
#pragma warning disable CA1720 // Identifier contains type name
    public static readonly UniversalRelationTag Object = new("obj", true);
#pragma warning restore CA1720 // Identifier contains type name

    /// <summary><c>obl</c>: oblique nominal.</summary>
    public static readonly UniversalRelationTag ObliqueNominal = new("obl", true);

    /// <summary><c>obl:agent</c>: agent modifier.</summary>
    public static readonly UniversalRelationTag AgentModifier = new("obl:agent", true);

    /// <summary>
    /// <see href="https://universaldependencies.org/en/dep/obl-arg.html"> <c>obl:tmod</c></see>:
    /// The relation <c>obl:arg</c> is used for oblique arguments and distinguishes them from adjuncts,
    /// which use the plain <c>obl</c> relation
    /// </summary>
    public static readonly UniversalRelationTag ObliqueArgument = new("obl:arg", true);

    /// <summary><c>obl:lmod</c>: locative modifier.</summary>
    public static readonly UniversalRelationTag LocativeModifier = new("obl:lmod", true);

    /// <summary>
    /// <see href="https://universaldependencies.org/en/dep/obl-tmod.html"> <c>obl:tmod</c></see>:
    /// A temporal modifier is a subtype of the obl relation: if the modifier is specifying a time,
    /// it is labeled as <c>tmod</c>.
    /// </summary>
    [Obsolete("Beginning with the version 2.15 release, most English corpora will use the new obl:unmarked relation instead.")]
    public static readonly UniversalRelationTag ObliqueTemporalModifier = new("obl:tmod", true);

    /// <summary>
    /// <see href="https://universaldependencies.org/en/dep/obl-npmod.html"> <c>obl:tmod</c></see>
    /// </summary>
    [Obsolete("Beginning with the version 2.15 release, most English corpora will use the new obl:unmarked relation instead.")]
    public static readonly UniversalRelationTag ObliqueNounPhraseAdverbialModifier = new("obl:npmod", true);

    /// <summary>
    /// <see href="https://universaldependencies.org/en/dep/obl-unmarked.html"> <c>obl:unmarked</c></see>:
    /// A subtype of the obl relation that applies when an oblique takes the form of a nominal lacking a
    /// preposition (a.k.a. a noun phrase). It is 'unmarked' in that, unlike most obliques, it has no
    /// adposition.
    /// </summary>
    /// <remarks>
    /// Prior to release 2.15, temporal adverbials (see (i)) had a separate subtype called obl:tmod,
    /// and obl:npmod was used for the non-temporal ones.
    /// </remarks>
    public static readonly UniversalRelationTag ObliqueAdpositionless = new("obl:unmarked", true);

    /// <summary><c>orphan</c>: orphan.</summary>
    public static readonly UniversalRelationTag Orphan = new("orphan", true);

    /// <summary><c>parataxis</c>: parataxis.</summary>
    public static readonly UniversalRelationTag Parataxis = new("parataxis", true);

    /// <summary><c>punct</c>: punctuation.</summary>
    public static readonly UniversalRelationTag Punctuation = new("punct", true);

    /// <summary><c>reparandum</c>: overridden disfluency.</summary>
    public static readonly UniversalRelationTag OverriddenDisfluency = new("reparandum", true);

    /// <summary><c>root</c>: root.</summary>
    public static readonly UniversalRelationTag Root = new("root", true);

    /// <summary><c>vocative</c>: vocative.</summary>
    public static readonly UniversalRelationTag Vocative = new("vocative", true);

    /// <summary><c>xcomp</c>: open clausal complement.</summary>
    public static readonly UniversalRelationTag OpenClausalComplement = new("xcomp", true);

    /// <summary>
    /// The set of all defined universal dependency relation tags.
    /// </summary>
    public static readonly FrozenSet<UniversalRelationTag> All = FrozenSet.ToFrozenSet(
    [
        AdjectivalModifier,
        AdverbialModifier,
        AdverbialClauseModifier,
        AgentModifier,
        AppositionalModifier,
        Auxiliary,
        CaseMarking,
        Classifier,
        ClausalComplement,
        ClausalModifierOfNoun,
        ClausalPassiveSubject,
        ClausalSubject,
        Compound,
        Conjunct,
        CoordinatingConjunction,
        Copula,
        Determiner,
        DiscourseElement,
        DislocatedElements,
        DirectObject,
        EmphasizingWord,
        Expletive,
        FixedMultiwordExpression,
        FlatMultiwordExpression,
        ForeignWords,
        GoesWith,
        ImpersonalExpletive,
        IndirectObject,
        LightVerbConstruction,
        List,
        LocativeAdverbialModifier,
        LocativeModifier,
        Marker,
        Names,
        NominalModifier,
        NominalSubject,
        NumericModifier,
        NumericModifierGoverningNoun,
        Object,
        ObliqueAdpositionless,
        ObliqueArgument,
        ObliqueNominal,
#pragma warning disable CS0618 // Type or member is obsolete
        ObliqueNounPhraseAdverbialModifier,
        ObliqueTemporalModifier,
#pragma warning restore CS0618 // Type or member is obsolete
        OpenClausalComplement,
        Orphan,
        OverriddenDisfluency,
        Parataxis,
        PassiveAuxiliary,
        PassiveNominalSubject,
        PhrasalVerbParticle,
        PossessiveDeterminer,
        PossessiveNominalModifier,
        Preconjunct,
        PronominalQuantifierAgreeing,
        PronominalQuantifierGoverning,
        Punctuation,
        ReduplicatedCompounds,
        ReflexiveCliticWithReflexiveverb,
        ReflexivePronounReflexivePassive,
        RelativeClauseModifier,
        Root,
        SerialVerbCompounds,
        TemporalModifier,
        UnspecifiedDependency,
        Vocative,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues = FrozenSet.ToFrozenSet(All.Select(r => r._value));
}


// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UniversalRelationTag, AsciiString>))]
public readonly partial struct UniversalRelationTag
    : IEquatableValue<UniversalRelationTag, AsciiString>,
      IUtf8SpanSerializable<UniversalRelationTag>,
      IRepeatableHash64
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => 32;

    public UniversalRelationTag(string value) : this((AsciiString)value)
    {
    }

    private UniversalRelationTag(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public int Length => _value.Length;

    public AsciiString Value => _value;

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator UniversalRelationTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

    public static readonly UniversalRelationTag ClausalModifierOfNoun = new("acl", true);

    public static readonly UniversalRelationTag RelativeClauseModifier = new("acl:relcl", true);

    public static readonly UniversalRelationTag AdverbialClauseModifier = new("advcl", true);

    public static readonly UniversalRelationTag AdverbialModifier = new("advmod", true);

    public static readonly UniversalRelationTag EmphasizingWord = new("advmod:emph", true);

    public static readonly UniversalRelationTag LocativeAdverbialModifier = new("advmod:lmod", true);

    public static readonly UniversalRelationTag AdjectivalModifier = new("amod", true);

    public static readonly UniversalRelationTag AppositionalModifier = new("appos", true);

    public static readonly UniversalRelationTag Auxiliary = new("aux", true);

    public static readonly UniversalRelationTag PassiveAuxiliary = new("aux:pass", true);

    public static readonly UniversalRelationTag CaseMarking = new("case", true);

    public static readonly UniversalRelationTag CoordinatingConjunction = new("cc", true);

    public static readonly UniversalRelationTag Preconjunct = new("cc:preconj", true);

    public static readonly UniversalRelationTag ClausalComplement = new("ccomp", true);

    public static readonly UniversalRelationTag Classifier = new("clf", true);

    public static readonly UniversalRelationTag Compound = new("compound", true);

    public static readonly UniversalRelationTag LightVerbConstruction = new("compound:lvc", true);

    public static readonly UniversalRelationTag PhrasalVerbParticle = new("compound:prt", true);

    public static readonly UniversalRelationTag ReduplicatedCompounds = new("compound:redup", true);

    public static readonly UniversalRelationTag SerialVerbCompounds = new("compound:svc", true);

    public static readonly UniversalRelationTag Conjunct = new("conj", true);

    public static readonly UniversalRelationTag Copula = new("cop", true);

    public static readonly UniversalRelationTag ClausalSubject = new("csubj", true);

    public static readonly UniversalRelationTag ClausalPassiveSubject = new("csubj:pass", true);

    public static readonly UniversalRelationTag UnspecifiedDependency = new("dep", true);

    public static readonly UniversalRelationTag Determiner = new("det", true);

    public static readonly UniversalRelationTag PronominalQuantifierGoverning = new("det:numgov", true);

    public static readonly UniversalRelationTag PronominalQuantifierAgreeing = new("det:nummod", true);

    public static readonly UniversalRelationTag PossessiveDeterminer = new("det:poss", true);

    public static readonly UniversalRelationTag DiscourseElement = new("discourse", true);

    public static readonly UniversalRelationTag DislocatedElements = new("dislocated", true);

    public static readonly UniversalRelationTag DirectObject = new("dobj", true);

    public static readonly UniversalRelationTag Expletive = new("expl", true);

    public static readonly UniversalRelationTag ImpersonalExpletive = new("expl:impers", true);

    public static readonly UniversalRelationTag ReflexivePronounReflexivePassive = new("expl:pass", true);

    public static readonly UniversalRelationTag ReflexiveCliticWithReflexiveverb = new("expl:pv", true);

    public static readonly UniversalRelationTag FixedMultiwordExpression = new("fixed", true);

    public static readonly UniversalRelationTag FlatMultiwordExpression = new("flat", true);

    public static readonly UniversalRelationTag ForeignWords = new("flat:foreign", true);

    public static readonly UniversalRelationTag Names = new("flat:name", true);

    public static readonly UniversalRelationTag GoesWith = new("goeswith", true);

    public static readonly UniversalRelationTag IndirectObject = new("iobj", true);

    public static readonly UniversalRelationTag List = new("list", true);

    public static readonly UniversalRelationTag Marker = new("mark", true);

    public static readonly UniversalRelationTag NominalModifier = new("nmod", true);

    public static readonly UniversalRelationTag PossessiveNominalModifier = new("nmod:poss", true);

    public static readonly UniversalRelationTag TemporalModifier = new("nmod:tmod", true);

    /// <summary>
    /// <c>nsubj</c>
    /// </summary>
    public static readonly UniversalRelationTag NominalSubject = new("nsubj", true);

    /// <summary>
    /// <c>nsubj:pass</c>
    /// </summary>
    public static readonly UniversalRelationTag PassiveNominalSubject = new("nsubj:pass", true);
    public static readonly UniversalRelationTag NumericModifier = new("nummod", true);
    public static readonly UniversalRelationTag NumericModifierGoverningNoun = new("nummod:gov", true);

    /// <summary>
    /// <c>obj</c>
    /// </summary>
#pragma warning disable CA1720 // Identifier contains type name
    public static readonly UniversalRelationTag Object = new("obj", true);
#pragma warning restore CA1720 // Identifier contains type name

    public static readonly UniversalRelationTag ObliqueNominal = new("obl", true);

    public static readonly UniversalRelationTag AgentModifier = new("obl:agent", true);

    /// <summary>
    /// <see href="https://universaldependencies.org/en/dep/obl-arg.html"> <c>obl:tmod</c></see>:
    /// The relation <c>obl:arg</c> is used for oblique arguments and distinguishes them from adjuncts,
    /// which use the plain <c>obl</c> relation
    /// </summary>
    public static readonly UniversalRelationTag ObliqueArgument = new("obl:arg", true);

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

    public static readonly UniversalRelationTag Orphan = new("orphan", true);

    public static readonly UniversalRelationTag Parataxis = new("parataxis", true);

    public static readonly UniversalRelationTag Punctuation = new("punct", true);

    public static readonly UniversalRelationTag OverriddenDisfluency = new("reparandum", true);

    public static readonly UniversalRelationTag Root = new("root", true);

    public static readonly UniversalRelationTag Vocative = new("vocative", true);

    public static readonly UniversalRelationTag OpenClausalComplement = new("xcomp", true);

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


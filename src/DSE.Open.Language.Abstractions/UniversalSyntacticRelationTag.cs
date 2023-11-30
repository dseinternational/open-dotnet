// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

[EquatableValue]
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UniversalSyntacticRelationTag, AsciiString>))]
public readonly partial struct UniversalSyntacticRelationTag
    : IEquatableValue<UniversalSyntacticRelationTag, AsciiString>,
      IUtf8SpanSerializable<UniversalSyntacticRelationTag>
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => 32;

    public UniversalSyntacticRelationTag(string value) : this((AsciiString)value)
    {
    }

    private UniversalSyntacticRelationTag(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public AsciiString Value => _value;

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator UniversalSyntacticRelationTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new UniversalSyntacticRelationTag(value);
    }

    public static readonly UniversalSyntacticRelationTag ClausalModifierOfNoun = new("acl", true);
    public static readonly UniversalSyntacticRelationTag RelativeClauseModifier = new("acl:relcl", true);
    public static readonly UniversalSyntacticRelationTag AdverbialClauseModifier = new("advcl", true);
    public static readonly UniversalSyntacticRelationTag AdverbialModifier = new("advmod", true);
    public static readonly UniversalSyntacticRelationTag EmphasizingWord = new("advmod:emph", true);
    public static readonly UniversalSyntacticRelationTag LocativeAdverbialModifier = new("advmod:lmod", true);
    public static readonly UniversalSyntacticRelationTag AdjectivalModifier = new("amod", true);
    public static readonly UniversalSyntacticRelationTag AppositionalModifier = new("appos", true);
    public static readonly UniversalSyntacticRelationTag Auxiliary = new("aux", true);
    public static readonly UniversalSyntacticRelationTag PassiveAuxiliary = new("aux:pass", true);
    public static readonly UniversalSyntacticRelationTag CaseMarking = new("case", true);
    public static readonly UniversalSyntacticRelationTag CoordinatingConjunction = new("cc", true);
    public static readonly UniversalSyntacticRelationTag Preconjunct = new("cc:preconj", true);
    public static readonly UniversalSyntacticRelationTag ClausalComplement = new("ccomp", true);
    public static readonly UniversalSyntacticRelationTag Classifier = new("clf", true);
    public static readonly UniversalSyntacticRelationTag Compound = new("compound", true);
    public static readonly UniversalSyntacticRelationTag LightVerbConstruction = new("compound:lvc", true);
    public static readonly UniversalSyntacticRelationTag PhrasalVerbParticle = new("compound:prt", true);
    public static readonly UniversalSyntacticRelationTag ReduplicatedCompounds = new("compound:redup", true);
    public static readonly UniversalSyntacticRelationTag SerialVerbCompounds = new("compound:svc", true);
    public static readonly UniversalSyntacticRelationTag Conjunct = new("conj", true);
    public static readonly UniversalSyntacticRelationTag Copula = new("cop", true);
    public static readonly UniversalSyntacticRelationTag ClausalSubject = new("csubj", true);
    public static readonly UniversalSyntacticRelationTag ClausalPassiveSubject = new("csubj:pass", true);
    public static readonly UniversalSyntacticRelationTag UnspecifiedDependency = new("dep", true);
    public static readonly UniversalSyntacticRelationTag Determiner = new("det", true);
    public static readonly UniversalSyntacticRelationTag PronominalQuantifierGoverning = new("det:numgov", true);
    public static readonly UniversalSyntacticRelationTag PronominalQuantifierAgreeing = new("det:nummod", true);
    public static readonly UniversalSyntacticRelationTag PossessiveDeterminer = new("det:poss", true);
    public static readonly UniversalSyntacticRelationTag DiscourseElement = new("discourse", true);
    public static readonly UniversalSyntacticRelationTag DislocatedElements = new("dislocated", true);
    public static readonly UniversalSyntacticRelationTag Expletive = new("expl", true);
    public static readonly UniversalSyntacticRelationTag ImpersonalExpletive = new("expl:impers", true);
    public static readonly UniversalSyntacticRelationTag ReflexivePronounReflexivePassive = new("expl:pass", true);
    public static readonly UniversalSyntacticRelationTag ReflexiveCliticWithReflexiveverb = new("expl:pv", true);
    public static readonly UniversalSyntacticRelationTag FixedMultiwordExpression = new("fixed", true);
    public static readonly UniversalSyntacticRelationTag FlatMultiwordExpression = new("flat", true);
    public static readonly UniversalSyntacticRelationTag ForeignWords = new("flat:foreign", true);
    public static readonly UniversalSyntacticRelationTag Names = new("flat:name", true);
    public static readonly UniversalSyntacticRelationTag GoesWith = new("goeswith", true);
    public static readonly UniversalSyntacticRelationTag IndirectObject = new("iobj", true);
    public static readonly UniversalSyntacticRelationTag List = new("list", true);
    public static readonly UniversalSyntacticRelationTag Marker = new("mark", true);
    public static readonly UniversalSyntacticRelationTag NominalModifier = new("nmod", true);
    public static readonly UniversalSyntacticRelationTag PossessiveNominalModifier = new("nmod:poss", true);
    public static readonly UniversalSyntacticRelationTag TemporalModifier = new("nmod:tmod", true);
    public static readonly UniversalSyntacticRelationTag NominalSubject = new("nsubj", true);
    public static readonly UniversalSyntacticRelationTag PassiveNominalSubject = new("nsubj:pass", true);
    public static readonly UniversalSyntacticRelationTag NumericModifier = new("nummod", true);
    public static readonly UniversalSyntacticRelationTag NumericModifierGoverningNoun = new("nummod:gov", true);
#pragma warning disable CA1720 // Identifier contains type name
    public static readonly UniversalSyntacticRelationTag Object = new("obj", true);
#pragma warning restore CA1720 // Identifier contains type name
    public static readonly UniversalSyntacticRelationTag ObliqueNominal = new("obl", true);
    public static readonly UniversalSyntacticRelationTag AgentModifier = new("obl:agent", true);
    public static readonly UniversalSyntacticRelationTag ObliqueArgument = new("obl:arg", true);
    public static readonly UniversalSyntacticRelationTag LocativeModifier = new("obl:lmod", true);
    public static readonly UniversalSyntacticRelationTag TemporalModifier2 = new("obl:tmod", true);
    public static readonly UniversalSyntacticRelationTag Orphan = new("orphan", true);
    public static readonly UniversalSyntacticRelationTag Parataxis = new("parataxis", true);
    public static readonly UniversalSyntacticRelationTag Punctuation = new("punct", true);
    public static readonly UniversalSyntacticRelationTag OverriddenDisfluency = new("reparandum", true);
    public static readonly UniversalSyntacticRelationTag Root = new("root", true);
    public static readonly UniversalSyntacticRelationTag Vocative = new("vocative", true);
    public static readonly UniversalSyntacticRelationTag OpenClausalComplement = new("xcomp", true);

    public static readonly FrozenSet<UniversalSyntacticRelationTag> All = FrozenSet.ToFrozenSet(
    [
        AdjectivalModifier,
        AdverbialModifier,
        AgentModifier,
        AppositionalModifier,
        Auxiliary,
        CaseMarking,
        Classifier,
        ClausalComplement,
        ClausalPassiveSubject,
        ClausalSubject,
        Compound,
        Conjunct,
        CoordinatingConjunction,
        Copula,
        Determiner,
        DiscourseElement,
        DislocatedElements,
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
        ObliqueArgument,
        ObliqueNominal,
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
        Root,
        SerialVerbCompounds,
        TemporalModifier,
        TemporalModifier2,
        UnspecifiedDependency,
        Vocative,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues = FrozenSet.ToFrozenSet(All.Select(r => r._value));
}


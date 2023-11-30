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
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<UniversalSyntacticRelation, AsciiString>))]
public readonly partial struct UniversalSyntacticRelation
    : IEquatableValue<UniversalSyntacticRelation, AsciiString>,
      IUtf8SpanSerializable<UniversalSyntacticRelation>
{
    public static int MaxSerializedCharLength => 32;

    public static int MaxSerializedByteLength => 32;

    public UniversalSyntacticRelation(string value) : this((AsciiString)value)
    {
    }

    private UniversalSyntacticRelation(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
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
    public static explicit operator UniversalSyntacticRelation(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new UniversalSyntacticRelation(value);
    }

    public static readonly UniversalSyntacticRelation ClausalModifierOfNoun = new("acl", true);
    public static readonly UniversalSyntacticRelation RelativeClauseModifier = new("acl:relcl", true);
    public static readonly UniversalSyntacticRelation AdverbialClauseModifier = new("advcl", true);
    public static readonly UniversalSyntacticRelation AdverbialModifier = new("advmod", true);
    public static readonly UniversalSyntacticRelation EmphasizingWord = new("advmod:emph", true);
    public static readonly UniversalSyntacticRelation LocativeAdverbialModifier = new("advmod:lmod", true);
    public static readonly UniversalSyntacticRelation AdjectivalModifier = new("amod", true);
    public static readonly UniversalSyntacticRelation AppositionalModifier = new("appos", true);
    public static readonly UniversalSyntacticRelation Auxiliary = new("aux", true);
    public static readonly UniversalSyntacticRelation PassiveAuxiliary = new("aux:pass", true);
    public static readonly UniversalSyntacticRelation CaseMarking = new("case", true);
    public static readonly UniversalSyntacticRelation CoordinatingConjunction = new("cc", true);
    public static readonly UniversalSyntacticRelation Preconjunct = new("cc:preconj", true);
    public static readonly UniversalSyntacticRelation ClausalComplement = new("ccomp", true);
    public static readonly UniversalSyntacticRelation Classifier = new("clf", true);
    public static readonly UniversalSyntacticRelation Compound = new("compound", true);
    public static readonly UniversalSyntacticRelation LightVerbConstruction = new("compound:lvc", true);
    public static readonly UniversalSyntacticRelation PhrasalVerbParticle = new("compound:prt", true);
    public static readonly UniversalSyntacticRelation ReduplicatedCompounds = new("compound:redup", true);
    public static readonly UniversalSyntacticRelation SerialVerbCompounds = new("compound:svc", true);
    public static readonly UniversalSyntacticRelation Conjunct = new("conj", true);
    public static readonly UniversalSyntacticRelation Copula = new("cop", true);
    public static readonly UniversalSyntacticRelation ClausalSubject = new("csubj", true);
    public static readonly UniversalSyntacticRelation ClausalPassiveSubject = new("csubj:pass", true);
    public static readonly UniversalSyntacticRelation UnspecifiedDependency = new("dep", true);
    public static readonly UniversalSyntacticRelation Determiner = new("det", true);
    public static readonly UniversalSyntacticRelation PronominalQuantifierGoverning = new("det:numgov", true);
    public static readonly UniversalSyntacticRelation PronominalQuantifierAgreeing = new("det:nummod", true);
    public static readonly UniversalSyntacticRelation PossessiveDeterminer = new("det:poss", true);
    public static readonly UniversalSyntacticRelation DiscourseElement = new("discourse", true);
    public static readonly UniversalSyntacticRelation DislocatedElements = new("dislocated", true);
    public static readonly UniversalSyntacticRelation Expletive = new("expl", true);
    public static readonly UniversalSyntacticRelation ImpersonalExpletive = new("expl:impers", true);
    public static readonly UniversalSyntacticRelation ReflexivePronounReflexivePassive = new("expl:pass", true);
    public static readonly UniversalSyntacticRelation ReflexiveCliticWithReflexiveverb = new("expl:pv", true);
    public static readonly UniversalSyntacticRelation FixedMultiwordExpression = new("fixed", true);
    public static readonly UniversalSyntacticRelation FlatMultiwordExpression = new("flat", true);
    public static readonly UniversalSyntacticRelation ForeignWords = new("flat:foreign", true);
    public static readonly UniversalSyntacticRelation Names = new("flat:name", true);
    public static readonly UniversalSyntacticRelation GoesWith = new("goeswith", true);
    public static readonly UniversalSyntacticRelation IndirectObject = new("iobj", true);
    public static readonly UniversalSyntacticRelation List = new("list", true);
    public static readonly UniversalSyntacticRelation Marker = new("mark", true);
    public static readonly UniversalSyntacticRelation NominalModifier = new("nmod", true);
    public static readonly UniversalSyntacticRelation PossessiveNominalModifier = new("nmod:poss", true);
    public static readonly UniversalSyntacticRelation TemporalModifier = new("nmod:tmod", true);
    public static readonly UniversalSyntacticRelation NominalSubject = new("nsubj", true);
    public static readonly UniversalSyntacticRelation PassiveNominalSubject = new("nsubj:pass", true);
    public static readonly UniversalSyntacticRelation NumericModifier = new("nummod", true);
    public static readonly UniversalSyntacticRelation NumericModifierGoverningNoun = new("nummod:gov", true);
#pragma warning disable CA1720 // Identifier contains type name
    public static readonly UniversalSyntacticRelation Object = new("obj", true);
#pragma warning restore CA1720 // Identifier contains type name
    public static readonly UniversalSyntacticRelation ObliqueNominal = new("obl", true);
    public static readonly UniversalSyntacticRelation AgentModifier = new("obl:agent", true);
    public static readonly UniversalSyntacticRelation ObliqueArgument = new("obl:arg", true);
    public static readonly UniversalSyntacticRelation LocativeModifier = new("obl:lmod", true);
    public static readonly UniversalSyntacticRelation TemporalModifier2 = new("obl:tmod", true);
    public static readonly UniversalSyntacticRelation Orphan = new("orphan", true);
    public static readonly UniversalSyntacticRelation Parataxis = new("parataxis", true);
    public static readonly UniversalSyntacticRelation Punctuation = new("punct", true);
    public static readonly UniversalSyntacticRelation OverriddenDisfluency = new("reparandum", true);
    public static readonly UniversalSyntacticRelation Root = new("root", true);
    public static readonly UniversalSyntacticRelation Vocative = new("vocative", true);
    public static readonly UniversalSyntacticRelation OpenClausalComplement = new("xcomp", true);

    public static readonly FrozenSet<UniversalSyntacticRelation> All = FrozenSet.ToFrozenSet(
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


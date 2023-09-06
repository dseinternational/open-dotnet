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
[JsonConverter(typeof(JsonSpanSerializableValueConverter<UniversalSyntacticRelation, AsciiString>))]
public readonly partial struct UniversalSyntacticRelation : IEquatableValue<UniversalSyntacticRelation, AsciiString>
{
    public static int MaxSerializedCharLength => 32;

    public UniversalSyntacticRelation(string value) : this((AsciiString)value)
    {
    }

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

    public static readonly UniversalSyntacticRelation ClausalModifierOfNoun = (UniversalSyntacticRelation)"acl";
    public static readonly UniversalSyntacticRelation RelativeClauseModifier = (UniversalSyntacticRelation)"acl:relcl";
    public static readonly UniversalSyntacticRelation AdverbialClauseModifier = (UniversalSyntacticRelation)"advcl";
    public static readonly UniversalSyntacticRelation AdverbialModifier = (UniversalSyntacticRelation)"advmod";
    public static readonly UniversalSyntacticRelation EmphasizingWord = (UniversalSyntacticRelation)"advmod:emph";
    public static readonly UniversalSyntacticRelation LocativeAdverbialModifier = (UniversalSyntacticRelation)"advmod:lmod";
    public static readonly UniversalSyntacticRelation AdjectivalModifier = (UniversalSyntacticRelation)"amod";
    public static readonly UniversalSyntacticRelation AppositionalModifier = (UniversalSyntacticRelation)"appos";
    public static readonly UniversalSyntacticRelation Auxiliary = (UniversalSyntacticRelation)"aux";
    public static readonly UniversalSyntacticRelation PassiveAuxiliary = (UniversalSyntacticRelation)"aux:pass";
    public static readonly UniversalSyntacticRelation CaseMarking = (UniversalSyntacticRelation)"case";
    public static readonly UniversalSyntacticRelation CoordinatingConjunction = (UniversalSyntacticRelation)"cc";
    public static readonly UniversalSyntacticRelation Preconjunct = (UniversalSyntacticRelation)"cc:preconj";
    public static readonly UniversalSyntacticRelation ClausalComplement = (UniversalSyntacticRelation)"ccomp";
    public static readonly UniversalSyntacticRelation Classifier = (UniversalSyntacticRelation)"clf";
    public static readonly UniversalSyntacticRelation Compound = (UniversalSyntacticRelation)"compound";
    public static readonly UniversalSyntacticRelation LightVerbConstruction = (UniversalSyntacticRelation)"compound:lvc";
    public static readonly UniversalSyntacticRelation PhrasalVerbParticle = (UniversalSyntacticRelation)"compound:prt";
    public static readonly UniversalSyntacticRelation ReduplicatedCompounds = (UniversalSyntacticRelation)"compound:redup";
    public static readonly UniversalSyntacticRelation SerialVerbCompounds = (UniversalSyntacticRelation)"compound:svc";
    public static readonly UniversalSyntacticRelation Conjunct = (UniversalSyntacticRelation)"conj";
    public static readonly UniversalSyntacticRelation Copula = (UniversalSyntacticRelation)"cop";
    public static readonly UniversalSyntacticRelation ClausalSubject = (UniversalSyntacticRelation)"csubj";
    public static readonly UniversalSyntacticRelation ClausalPassiveSubject = (UniversalSyntacticRelation)"csubj:pass";
    public static readonly UniversalSyntacticRelation UnspecifiedDependency = (UniversalSyntacticRelation)"dep";
    public static readonly UniversalSyntacticRelation Determiner = (UniversalSyntacticRelation)"det";
    public static readonly UniversalSyntacticRelation PronominalQuantifierGoverning = (UniversalSyntacticRelation)"det:numgov";
    public static readonly UniversalSyntacticRelation PronominalQuantifierAgreeing = (UniversalSyntacticRelation)"det:nummod";
    public static readonly UniversalSyntacticRelation PossessiveDeterminer = (UniversalSyntacticRelation)"det:poss";
    public static readonly UniversalSyntacticRelation DiscourseElement = (UniversalSyntacticRelation)"discourse";
    public static readonly UniversalSyntacticRelation DislocatedElements = (UniversalSyntacticRelation)"dislocated";
    public static readonly UniversalSyntacticRelation Expletive = (UniversalSyntacticRelation)"expl";
    public static readonly UniversalSyntacticRelation ImpersonalExpletive = (UniversalSyntacticRelation)"expl:impers";
    public static readonly UniversalSyntacticRelation ReflexivePronounReflexivePassive = (UniversalSyntacticRelation)"expl:pass";
    public static readonly UniversalSyntacticRelation ReflexiveCliticWithReflexiveverb = (UniversalSyntacticRelation)"expl:pv";
    public static readonly UniversalSyntacticRelation FixedMultiwordExpression = (UniversalSyntacticRelation)"fixed";
    public static readonly UniversalSyntacticRelation FlatMultiwordExpression = (UniversalSyntacticRelation)"flat";
    public static readonly UniversalSyntacticRelation ForeignWords = (UniversalSyntacticRelation)"flat:foreign";
    public static readonly UniversalSyntacticRelation Names = (UniversalSyntacticRelation)"flat:name";
    public static readonly UniversalSyntacticRelation GoesWith = (UniversalSyntacticRelation)"goeswith";
    public static readonly UniversalSyntacticRelation IndirectObject = (UniversalSyntacticRelation)"iobj";
    public static readonly UniversalSyntacticRelation List = (UniversalSyntacticRelation)"list";
    public static readonly UniversalSyntacticRelation Marker = (UniversalSyntacticRelation)"mark";
    public static readonly UniversalSyntacticRelation NominalModifier = (UniversalSyntacticRelation)"nmod";
    public static readonly UniversalSyntacticRelation PossessiveNominalModifier = (UniversalSyntacticRelation)"nmod:poss";
    public static readonly UniversalSyntacticRelation TemporalModifier = (UniversalSyntacticRelation)"nmod:tmod";
    public static readonly UniversalSyntacticRelation NominalSubject = (UniversalSyntacticRelation)"nsubj";
    public static readonly UniversalSyntacticRelation PassiveNominalSubject = (UniversalSyntacticRelation)"nsubj:pass";
    public static readonly UniversalSyntacticRelation NumericModifier = (UniversalSyntacticRelation)"nummod";
    public static readonly UniversalSyntacticRelation NumericModifierGoverningNoun = (UniversalSyntacticRelation)"nummod:gov";
#pragma warning disable CA1720 // Identifier contains type name
    public static readonly UniversalSyntacticRelation Object = (UniversalSyntacticRelation)"obj";
#pragma warning restore CA1720 // Identifier contains type name
    public static readonly UniversalSyntacticRelation ObliqueNominal = (UniversalSyntacticRelation)"obl";
    public static readonly UniversalSyntacticRelation AgentModifier = (UniversalSyntacticRelation)"obl:agent";
    public static readonly UniversalSyntacticRelation ObliqueArgument = (UniversalSyntacticRelation)"obl:arg";
    public static readonly UniversalSyntacticRelation LocativeModifier = (UniversalSyntacticRelation)"obl:lmod";
    public static readonly UniversalSyntacticRelation TemporalModifier2 = (UniversalSyntacticRelation)"obl:tmod";
    public static readonly UniversalSyntacticRelation Orphan = (UniversalSyntacticRelation)"orphan";
    public static readonly UniversalSyntacticRelation Parataxis = (UniversalSyntacticRelation)"parataxis";
    public static readonly UniversalSyntacticRelation Punctuation = (UniversalSyntacticRelation)"punct";
    public static readonly UniversalSyntacticRelation OverriddenDisfluency = (UniversalSyntacticRelation)"reparandum";
    public static readonly UniversalSyntacticRelation Root = (UniversalSyntacticRelation)"root";
    public static readonly UniversalSyntacticRelation Vocative = (UniversalSyntacticRelation)"vocative";
    public static readonly UniversalSyntacticRelation OpenClausalComplement = (UniversalSyntacticRelation)"xcomp";

    public static readonly IReadOnlySet<UniversalSyntacticRelation> All = FrozenSet.ToFrozenSet(new[]
    {
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
    });

    private static readonly IReadOnlySet<AsciiString> s_validValues = FrozenSet.ToFrozenSet(All.Select(r => r._value));
}


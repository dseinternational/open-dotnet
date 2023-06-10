// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language;

// https://universaldependencies.org/u/dep/index.html
// also: https://nlp.stanford.edu/pubs/USD_LREC14_paper_camera_ready.pdf

public static class UniversalSyntacticRelations
{
    public static readonly PosTag ClausalModifierOfNoun = (PosTag)"acl";
    public static readonly PosTag RelativeClauseModifier = (PosTag)"acl:relcl";
    public static readonly PosTag AdverbialClauseModifier = (PosTag)"advcl";
    public static readonly PosTag AdverbialModifier = (PosTag)"advmod";
    public static readonly PosTag EmphasizingWord = (PosTag)"advmod:emph";
    public static readonly PosTag LocativeAdverbialModifier = (PosTag)"advmod:lmod";
    public static readonly PosTag AdjectivalModifier = (PosTag)"amod";
    public static readonly PosTag AppositionalModifier = (PosTag)"appos";
    public static readonly PosTag Auxiliary = (PosTag)"aux";
    public static readonly PosTag PassiveAuxiliary = (PosTag)"aux:pass";
    public static readonly PosTag CaseMarking = (PosTag)"case";
    public static readonly PosTag CoordinatingConjunction = (PosTag)"cc";
    public static readonly PosTag Preconjunct = (PosTag)"cc:preconj";
    public static readonly PosTag ClausalComplement = (PosTag)"ccomp";
    public static readonly PosTag Classifier = (PosTag)"clf";
    public static readonly PosTag Compound = (PosTag)"compound";
    public static readonly PosTag LightVerbConstruction = (PosTag)"compound:lvc";
    public static readonly PosTag PhrasalVerbParticle = (PosTag)"compound:prt";
    public static readonly PosTag ReduplicatedCompounds = (PosTag)"compound:redup";
    public static readonly PosTag SerialVerbCompounds = (PosTag)"compound:svc";
    public static readonly PosTag Conjunct = (PosTag)"conj";
    public static readonly PosTag Copula = (PosTag)"cop";
    public static readonly PosTag ClausalSubject = (PosTag)"csubj";
    public static readonly PosTag ClausalPassiveSubject = (PosTag)"csubj:pass";
    public static readonly PosTag UnspecifiedDependency = (PosTag)"dep";
    public static readonly PosTag Determiner = (PosTag)"det";
    public static readonly PosTag PronominalQuantifierGoverning = (PosTag)"det:numgov";
    public static readonly PosTag PronominalQuantifierAgreeing = (PosTag)"det:nummod";
    public static readonly PosTag PossessiveDeterminer = (PosTag)"det:poss";
    public static readonly PosTag DiscourseElement = (PosTag)"discourse";
    public static readonly PosTag DislocatedElements = (PosTag)"dislocated";
    public static readonly PosTag Expletive = (PosTag)"expl";
    public static readonly PosTag ImpersonalExpletive = (PosTag)"expl:impers";
    public static readonly PosTag ReflexivePronounReflexivePassive = (PosTag)"expl:pass";
    public static readonly PosTag ReflexiveCliticWithReflexiveverb = (PosTag)"expl:pv";
    public static readonly PosTag FixedMultiwordExpression = (PosTag)"fixed";
    public static readonly PosTag FlatMultiwordExpression = (PosTag)"flat";
    public static readonly PosTag ForeignWords = (PosTag)"flat:foreign";
    public static readonly PosTag Names = (PosTag)"flat:name";
    public static readonly PosTag GoesWith = (PosTag)"goeswith";
    public static readonly PosTag IndirectObject = (PosTag)"iobj";
    public static readonly PosTag List = (PosTag)"list";
    public static readonly PosTag Marker = (PosTag)"mark";
    public static readonly PosTag NominalModifier = (PosTag)"nmod";
    public static readonly PosTag PossessiveNominalModifier = (PosTag)"nmod:poss";
    public static readonly PosTag TemporalModifier = (PosTag)"nmod:tmod";
    public static readonly PosTag NominalSubject = (PosTag)"nsubj";
    public static readonly PosTag PassiveNominalSubject = (PosTag)"nsubj:pass";
    public static readonly PosTag NumericModifier = (PosTag)"nummod";
    public static readonly PosTag NumericModifierGoverningNoun = (PosTag)"nummod:gov";
#pragma warning disable CA1720 // Identifier contains type name
    public static readonly PosTag Object = (PosTag)"obj";
#pragma warning restore CA1720 // Identifier contains type name
    public static readonly PosTag ObliqueNominal = (PosTag)"obl";
    public static readonly PosTag AgentModifier = (PosTag)"obl:agent";
    public static readonly PosTag ObliqueArgument = (PosTag)"obl:arg";
    public static readonly PosTag LocativeModifier = (PosTag)"obl:lmod";
    public static readonly PosTag TemporalModifier2 = (PosTag)"obl:tmod";
    public static readonly PosTag Orphan = (PosTag)"orphan";
    public static readonly PosTag Parataxis = (PosTag)"parataxis";
    public static readonly PosTag Punctuation = (PosTag)"punct";
    public static readonly PosTag OverriddenDisfluency = (PosTag)"reparandum";
    public static readonly PosTag Root = (PosTag)"root";
    public static readonly PosTag Vocative = (PosTag)"vocative";
    public static readonly PosTag OpenClausalComplement = (PosTag)"xcomp";

}

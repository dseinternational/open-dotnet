// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language;

/// <summary>
/// <see href="https://universaldependencies.org/u/pos/">Universal POS tags</see> mark the core 
/// part-of-speech categories. 
/// </summary>
public static class UniversalPOSTags
{
    public static readonly PosTag Adjective = (PosTag)"ADJ";

    public static readonly PosTag Adposition = (PosTag)"ADP";

    public static readonly PosTag Adverb = (PosTag)"ADV";

    public static readonly PosTag Auxiliary = (PosTag)"AUX";

    public static readonly PosTag CoordinatingConjunction = (PosTag)"CCONJ";

    public static readonly PosTag Determiner = (PosTag)"DET";

    public static readonly PosTag Interjection = (PosTag)"INTJ";

    public static readonly PosTag Noun = (PosTag)"NOUN";

    public static readonly PosTag Numeral = (PosTag)"NUM";

    public static readonly PosTag Particle = (PosTag)"PART";

    public static readonly PosTag Pronoun = (PosTag)"PRON";

    public static readonly PosTag ProperNoun = (PosTag)"PROPN";

    public static readonly PosTag Punctuation = (PosTag)"PUNCT";

    public static readonly PosTag SubordinatingConjunction = (PosTag)"SCONJ";

    public static readonly PosTag Symbol = (PosTag)"SYM";

    public static readonly PosTag Verb = (PosTag)"VERB";

    public static readonly PosTag Other = (PosTag)"X";
}

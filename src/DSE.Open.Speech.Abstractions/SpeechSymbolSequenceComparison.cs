// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech;

public enum SpeechSymbolSequenceComparison
{
    /// <summary>
    /// The symbols are compared by character code value.
    /// </summary>
    Exact,

    /// <summary>
    /// Symbols sequences that contain equivalent sequences of consonants or
    /// vowel symbols are considered equal. In other words, suprasegmental symbols
    /// and diacritics are ignored for the comparison.
    /// </summary>
    ConsonantsAndVowels,
}

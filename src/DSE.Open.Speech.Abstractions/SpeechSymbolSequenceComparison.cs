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
    /// Symbols and symbol sequences that are equivalent are considered equal.
    /// For example, the symbols 'g' (U+0047) and 'ɡ' (U+0261) are considered equal;
    /// also, the symbol 'ɚ' (U+025A) is considered equal to the symbol sequence
    /// 'ə˞' (U+0259 U+02DE).
    /// </summary>
    Permissive
}

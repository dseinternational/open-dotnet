// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech;

public enum SpeechSymbolComparison
{
    /// <summary>
    /// The symbols are compared by character code value.
    /// </summary>
    Exact,

    /// <summary>
    /// Symbols that are equivalent are considered equal. For example, the symbols
    /// 'g' (U+0047) and 'ɡ' (U+0261) are considered equal.
    /// </summary>
    Permissive
}

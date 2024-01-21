// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech;

public enum PhonemeLabelScheme
{
    /// <summary>
    /// Labels using our internal phonetic spelling scheme. This is <see cref="OED"/>
    /// but with /er/ to contrast /er/ as in ⟨butter⟩ and /ur/ as in ⟨word⟩.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Labels using the Oxford English Dictionary phonetic spelling scheme.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Pronunciation_respelling_for_English"/></remarks>
    OED = 1,

    /// <summary>
    /// Labels using the BBC phonetic spelling scheme.
    /// </summary>
    /// <remarks>See <see href="https://en.wikipedia.org/wiki/Pronunciation_respelling_for_English"/></remarks>
    BBC = 2,

    /// <summary>
    /// Labels using in See and Learn Speech apps and kits.
    /// </summary>
    SeeAndLearnV2 = 1001,
}

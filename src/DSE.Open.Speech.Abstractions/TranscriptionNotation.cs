// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech;

/// <summary>
/// Indicates the type of a transcription notation.
/// </summary>
/// <remarks>
/// See <see href="https://en.wikipedia.org/wiki/International_Phonetic_Alphabet#Brackets_and_transcription_delimiters">
/// International Phonetic Alphabet on Wikipedia</see>.
/// </remarks>
public enum TranscriptionNotation
{
    /// <summary>
    /// Indicates that the notation is undefined or cannot be determined.
    /// </summary>
    Undefined,

    /// <summary>
    /// Indicates phonetic notation, typically delimited by square brackets <c>[...]</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://en.wikipedia.org/wiki/International_Phonetic_Alphabet#Brackets_and_transcription_delimiters">
    /// International Phonetic Alphabet on Wikipedia</see>.
    /// </remarks>
    Phonetic,

    /// <summary>
    /// Indicates phonemic notation, typically delimited by slashes <c>/.../</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://en.wikipedia.org/wiki/International_Phonetic_Alphabet#Brackets_and_transcription_delimiters">
    /// International Phonetic Alphabet on Wikipedia</see>.
    /// </remarks>
    Phonemic,

    /// <summary>
    /// Indicates prosodic notation, typically delimited by braces <c>{...}</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://en.wikipedia.org/wiki/International_Phonetic_Alphabet#Brackets_and_transcription_delimiters">
    /// International Phonetic Alphabet on Wikipedia</see>.
    /// </remarks>
    Prosodic,

    /// <summary>
    /// Indicates indistinguishable or unidentified utterances or silent articulation
    /// (mouthing), typically delimited by parenthesis <c>(...)</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://en.wikipedia.org/wiki/International_Phonetic_Alphabet#Brackets_and_transcription_delimiters">
    /// International Phonetic Alphabet on Wikipedia</see>.
    /// </remarks>
    IndistinguishableOrSlientArticulation,

    /// <summary>
    /// Indicates obscured speech or a description of the obscuring noise
    /// typically delimited by double parenthesis <c>⸨...⸩</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://en.wikipedia.org/wiki/International_Phonetic_Alphabet#Brackets_and_transcription_delimiters">
    /// International Phonetic Alphabet on Wikipedia</see>.
    /// </remarks>
    ObscuredSpeechOrObscuringNoise
}

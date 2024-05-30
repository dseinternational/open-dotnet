// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;
using DSE.Open.Text;

namespace DSE.Open.Speech;

/// <summary>
/// Defines a phoneme, the smallest unit of sound in a language that distinguishes one
/// word from another. A phoneme can be considered an abstraction for a sound or a group
/// of different sounds that are perceived to have the same function by speakers of the
/// language or dialect in question.
/// </summary>
public sealed class Phoneme : IEquatable<Phoneme>
{
    /// <summary>
    /// The language in which the phoneme occurs.
    /// </summary>
    [JsonPropertyName("language")]
    public LanguageCode2 Language { get; init; }

    /// <summary>
    /// The speech sound that identifies the (abstract) phoneme.
    /// </summary>
    [JsonPropertyName("abstraction")]
    public SpeechSound Abstraction { get; init; }

    /// <summary>
    /// Alternative speech sounds that can be used to represent the phoneme
    /// in the given <see cref="Language"/>.
    /// </summary>
    [JsonPropertyName("allophones")]
    public ReadOnlyValueSet<SpeechSound> Allophones { get; init; } = [];

    [JsonIgnore]
    public bool IsConsonant => SpeechSound.IsConsonant(Abstraction);

    [JsonIgnore]
    public bool IsVowel => SpeechSound.IsVowel(Abstraction);

    public override bool Equals(object? obj)
    {
        return obj is Phoneme phoneme && Equals(phoneme);
    }

    public bool Equals(Phoneme? other)
    {
        return other is not null
            && Language == other.Language
            && Abstraction == other.Abstraction
            && Allophones.SetEquals(other.Allophones);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Language, Abstraction, Allophones);
    }

    /// <summary>
    /// Determines if a given speech sound matches the phoneme definition -
    /// that is equals <see cref="Abstraction"/> or is contained in
    /// <see cref="Allophones"/>.
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    public bool IsMatch(SpeechSound sound)
    {
        return Abstraction == sound || Allophones.Contains(sound);
    }

    /// <summary>
    /// Returns a string representation of the phoneme.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Allophones.Count == 0
            ? $"/{Abstraction}/ ({Language})"
            : $"/{Abstraction}/ ({Language}) [ {string.Join(", ", StringHelper.WrapRange('[', ']', Allophones))} ]";
    }

    /// <summary>
    /// Returns a string representation of the phoneme using the given <see cref="PhonemeLabelScheme"/>.
    /// If the scheme does not define a label for the phoneme, returns <see cref="Abstraction"/> as a string.
    /// </summary>
    /// <param name="scheme"></param>
    /// <returns></returns>
    public string ToString(PhonemeLabelScheme scheme)
    {
        if (PhonemeLabel.TryGetLabel(scheme, this, out var label))
        {
            return label;
        }

        return Abstraction.ToString();
    }

    public SpeechTranscription ToTranscription()
    {
        return new(Abstraction, TranscriptionNotation.Phonemic);
    }

    public static bool Equals(Phoneme? left, Phoneme? right)
    {
        if (left is null)
        {
            return right is null;
        }

        if (right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator ==(Phoneme? left, Phoneme? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Phoneme? left, Phoneme? right)
    {
        return !(left == right);
    }
}

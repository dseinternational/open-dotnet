// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using static DSE.Open.Speech.Phonemes;

namespace DSE.Open.Speech.Abstractions.Tests;

public class PhonemesTests
{
    [Fact]
    public void CanLoadEnglishAll()
    {
        var all = English.All;
        Assert.NotNull(all);
    }

    [Fact]
    public void EnglishAllAbstractionsAreUnique()
    {
        var all = English.All.ToDictionary(p => p.Abstraction);
        Assert.NotNull(all);
    }

    [Fact]
    public void EnglishConsonantsCount()
    {
        Assert.Equal(26, English.Consonants.Count);
    }

    [Fact]
    public void EnglishConsonantsAreConsonants()
    {
        foreach (var p in English.Consonants)
        {
            Assert.True(p.IsConsonant, $"{p.Abstraction} not classified as consonant");
        }
    }

    [Fact]
    public void EnglishVowelsCount()
    {
        Assert.Equal(24, English.Vowels.Count);
    }

    [Fact]
    public void EnglishVowelsAreVowels()
    {
        foreach (var p in English.Vowels)
        {
            Assert.True(p.IsVowel, $"{p.Abstraction} not classified as vowel");
        }
    }

    [Theory]
    [InlineData("ˈælɪɡˌeɪtɐ")]
    [InlineData("ˈænɪməlz")]
    public void WordsStartingWithA(string word)
    {
        var transcription = SpeechSymbolSequence.ParseInvariant(word);
        Assert.True(transcription.StartsWith(English.a.Abstraction, SpeechSymbolSequenceComparison.ConsonantsAndVowels));
    }

    [Theory]
    [InlineData("bˈɑːθɹuːm")]
    [InlineData("blˈækbəɹi")]
    public void WordsStartingWithB(string word)
    {
        var transcription = SpeechSymbolSequence.ParseInvariant(word);
        Assert.True(transcription.StartsWith(English.b.Abstraction));
    }

    [Theory]
    [InlineData("kˈæbɪdʒ")]
    [InlineData("kˈeɪks")]
    public void WordsStartingWithK(string word)
    {
        var transcription = SpeechSymbolSequence.ParseInvariant(word);
        Assert.True(transcription.StartsWith(English.k.Abstraction));
    }

    [Theory]
    [MemberData(nameof(WordTranscriptions))]
    public void InitialSoundsArePhonemes(string word)
    {
        var sequence = SpeechSymbolSequence.ParseInvariant(word);
        var initialSound = sequence.GetInitialSound();
        Assert.Contains(English.All, p => p.Abstraction == initialSound || p.Allophones.Contains(initialSound));
    }

#pragma warning disable CA5394 // Do not use insecure randomness
    public static TheoryData<string> WordTranscriptions =>
        new(TranscriptionData.Transcriptions.ToArray());
#pragma warning restore CA5394 // Do not use insecure randomness

}

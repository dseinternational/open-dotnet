// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech.Abstractions.Tests;

public class SpeechSymbolTests
{
    [Theory]
    [MemberData(nameof(Consonants))]
    public void Consonants_IsStrictIpaSymbol(char symbol)
    {
        Assert.True(SpeechSymbol.IsStrictIpaSymbol(symbol));
    }

    [Theory]
    [MemberData(nameof(Vowels))]
    public void Vowels_IsStrictIpaSymbol(char symbol)
    {
        Assert.True(SpeechSymbol.IsStrictIpaSymbol(symbol));
    }

    [Theory]
    [MemberData(nameof(Diacritics))]
    public void Diacritics_IsStrictIpaSymbol(char symbol)
    {
        Assert.True(SpeechSymbol.IsStrictIpaSymbol(symbol));
    }

    [Theory]
    [MemberData(nameof(Suprasegmentals))]
    public void Suprasegmentals_IsStrictIpaSymbol(char symbol)
    {
        Assert.True(SpeechSymbol.IsStrictIpaSymbol(symbol));
    }

    [Theory]
    [MemberData(nameof(OtherSymbols))]
    public void OtherSymbols_IsStrictIpaSymbol(char symbol)
    {
        Assert.True(SpeechSymbol.IsStrictIpaSymbol(symbol));
    }

    public static TheoryData<char> Consonants =>
        new(SpeechSymbol.Consonants.Select(c => (char)c).ToArray());

    public static TheoryData<char> Vowels =>
        new(SpeechSymbol.Vowels.Select(c => (char)c).ToArray());

    public static TheoryData<char> Diacritics =>
        new(SpeechSymbol.Diacritics.Select(c => (char)c).ToArray());

    public static TheoryData<char> Suprasegmentals =>
        new(SpeechSymbol.Suprasegmentals.Select(c => (char)c).ToArray());

    public static TheoryData<char> OtherSymbols =>
        new(SpeechSymbol.OtherSymbols.Select(c => (char)c).ToArray());
}

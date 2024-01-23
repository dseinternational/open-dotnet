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
    public void EnglishVowelsCount()
    {
        Assert.Equal(24, English.Vowels.Count);
    }
}

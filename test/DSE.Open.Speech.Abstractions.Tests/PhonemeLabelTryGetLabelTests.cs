// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech.Abstractions.Tests;

public class PhonemeLabelTryGetLabelTests
{
    [Fact]
    public void TryGetLabel_KnownPhonemeInDefaultScheme_ReturnsTrueAndLabel()
    {
        Assert.True(PhonemeLabel.TryGetLabel(PhonemeLabelScheme.Default, Phonemes.English.b, out var label));
        Assert.Equal("b", label);
    }

    [Fact]
    public void TryGetLabel_DifferingSchemesYieldDifferentLabels()
    {
        Assert.True(PhonemeLabel.TryGetLabel(PhonemeLabelScheme.Default, Phonemes.English.er, out var def));
        Assert.True(PhonemeLabel.TryGetLabel(PhonemeLabelScheme.OED, Phonemes.English.er, out var oed));
        Assert.Equal("er", def);
        Assert.Equal("ur", oed);
    }

    [Fact]
    public void TryGetLabel_UnknownScheme_ReturnsFalseAndNullLabel()
    {
        Assert.False(PhonemeLabel.TryGetLabel((PhonemeLabelScheme)9999, Phonemes.English.b, out var label));
        Assert.Null(label);
    }

    [Fact]
    public void TryGetLabel_PhonemeMissingFromSchemeMap_ReturnsFalseAndNullLabel()
    {
        // SeeAndLearnV2 does not map Phonemes.English.aw (it is commented out).
        Assert.False(PhonemeLabel.TryGetLabel(PhonemeLabelScheme.SeeAndLearnV2, Phonemes.English.aw, out var label));
        Assert.Null(label);
    }

    [Fact]
    public void TryGetLabel_NullPhoneme_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => PhonemeLabel.TryGetLabel(PhonemeLabelScheme.Default, null!, out _));
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech.Abstractions.Tests;

public class SpeechSoundValidationTests
{
    [Fact]
    public void IsValidValue_EmptySequence_ReturnsFalse()
    {
        Assert.False(SpeechSound.IsValidValue(default));
    }

    [Fact]
    public void IsValidValue_SingleSymbol_ReturnsTrue()
    {
        var seq = SpeechSymbolSequence.ParseInvariant("a");
        Assert.True(SpeechSound.IsValidValue(seq));
    }

    [Fact]
    public void IsValidValue_MaxLengthSequence_ReturnsTrue()
    {
        var seq = SpeechSymbolSequence.ParseInvariant(new string('a', SpeechSound.MaxLength));
        Assert.Equal(SpeechSound.MaxLength, seq.Length);
        Assert.True(SpeechSound.IsValidValue(seq));
    }

    [Fact]
    public void IsValidValue_LongerThanMax_ReturnsFalse()
    {
        var seq = SpeechSymbolSequence.ParseInvariant(new string('a', SpeechSound.MaxLength + 1));
        Assert.False(SpeechSound.IsValidValue(seq));
    }

    [Fact]
    public void Ctor_AcceptsSequenceOfMaxLength()
    {
        var seq = SpeechSymbolSequence.ParseInvariant(new string('a', SpeechSound.MaxLength));
        var sound = new SpeechSound(seq);
        Assert.Equal(SpeechSound.MaxLength, sound.Length);
    }

    [Fact]
    public void Ctor_RejectsSequenceLongerThanMax()
    {
        var seq = SpeechSymbolSequence.ParseInvariant(new string('a', SpeechSound.MaxLength + 1));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new SpeechSound(seq));
    }

    [Fact]
    public void Ctor_RejectsEmptySequence()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new SpeechSound(default));
    }
}

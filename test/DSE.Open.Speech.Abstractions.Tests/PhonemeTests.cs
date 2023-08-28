// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Speech.Abstractions.Tests;

public class PhonemeTests
{

    [Theory]
    [MemberData(nameof(Phonemes))]
    public void Equality(Phoneme p)
    {
        var p2 = new Phoneme(p.ToString());
        Assert.Equal(p, p2);
    }

    [Theory]
    [MemberData(nameof(Phonemes))]
    public void Serialize(Phoneme p)
    {
        var json = JsonSerializer.Serialize(p);
        var expected = JsonSerializer.Serialize(p.ToString());

        Assert.Equal(expected, json);
    }

    [Theory]
    [MemberData(nameof(Phonemes))]
    public void SerializeWithRelaxedJsonEscaping(Phoneme p)
    {
        var json = JsonSerializer.Serialize(p, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal($"\"{p}\"", json);
    }

    public static TheoryData<Phoneme> Phonemes
    {
        get
        {
            var data = new TheoryData<Phoneme>()
            {
                new Phoneme("b"),
                new Phoneme("d"),
                new Phoneme("ð"),
                new Phoneme("dʒ"),
                new Phoneme("f"),
                new Phoneme("ɡ"),
                new Phoneme("h"),
                new Phoneme("j"),
                new Phoneme("k"),
                new Phoneme("l"),
                new Phoneme("m"),
                new Phoneme("n"),
                new Phoneme("ŋ"),
                new Phoneme("p"),
                new Phoneme("ɹ"),
                new Phoneme("s"),
                new Phoneme("ʃ"),
                new Phoneme("t"),
                new Phoneme("tʃ"),
                new Phoneme("v"),
                new Phoneme("w"),
                new Phoneme("z"),
                new Phoneme("ʒ"),
                new Phoneme("θ"),
                new Phoneme("ɒ"),
                new Phoneme("ɑː"),
                new Phoneme("æ"),
                new Phoneme("aɪ"),
                new Phoneme("aʊ"),
                new Phoneme("ɔː"),
                new Phoneme("ɔɪ"),
                new Phoneme("ə"),
                new Phoneme("eə"),
                new Phoneme("eɪ"),
                new Phoneme("əʊ"),
                new Phoneme("e"),
                new Phoneme("ɜː"),
                new Phoneme("ɪ"),
                new Phoneme("iː"),
                new Phoneme("ɪə"),
                new Phoneme("ʊ"),
                new Phoneme("uː"),
                new Phoneme("ʊə"),
                new Phoneme("ʌ"),
            };
            return data;
        }
    }

}

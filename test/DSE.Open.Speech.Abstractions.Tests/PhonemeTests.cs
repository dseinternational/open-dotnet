// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Speech.Tests;

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

    [Theory]
    [MemberData(nameof(Phonemes))]
    public void SerializeDeserialize(Phoneme p)
    {
        var json = JsonSerializer.Serialize(p);
        var deserialized = JsonSerializer.Deserialize<Phoneme>(json);
        Assert.Equal(p, deserialized);
    }

    [Theory]
    [MemberData(nameof(Phonemes))]
    public void SerializeDeserializeWithRelaxedJsonEscaping(Phoneme p)
    {
        var json = JsonSerializer.Serialize(p, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<Phoneme>(json);
        Assert.Equal(p, deserialized);
    }

    public static TheoryData<Phoneme> Phonemes
    {
        get
        {
            var data = new TheoryData<Phoneme>()
            {
                new("b"),
                new("d"),
                new("ð"),
                new("dʒ"),
                new("f"),
                new("ɡ"),
                new("h"),
                new("j"),
                new("k"),
                new("l"),
                new("m"),
                new("n"),
                new("ŋ"),
                new("p"),
                new("ɹ"),
                new("s"),
                new("ʃ"),
                new("t"),
                new("tʃ"),
                new("v"),
                new("w"),
                new("z"),
                new("ʒ"),
                new("θ"),
                new("ɒ"),
                new("ɑː"),
                new("æ"),
                new("aɪ"),
                new("aʊ"),
                new("ɔː"),
                new("ɔɪ"),
                new("ə"),
                new("eə"),
                new("eɪ"),
                new("əʊ"),
                new("e"),
                new("ɜː"),
                new("ɪ"),
                new("iː"),
                new("ɪə"),
                new("ʊ"),
                new("uː"),
                new("ʊə"),
                new("ʌ"),
            };
            return data;
        }
    }

}

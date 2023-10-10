// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using System.Text.Json;

namespace DSE.Open.Language.Tests;

public class UniversalPosTagTests
{
    [Theory]
    [MemberData(nameof(Tags))]
    public void Parse_valid_tags(string tagStr)
    {
        var tag = UniversalPosTag.Parse(tagStr, CultureInfo.InvariantCulture);
        Assert.NotEqual(default, tag);
    }

    [Theory]
    [MemberData(nameof(Tags))]
    public void Serialize_deserialize(string tagStr)
    {
        var tag = UniversalPosTag.Parse(tagStr, CultureInfo.InvariantCulture);
        var json = JsonSerializer.Serialize(tag);
        var deserialized = JsonSerializer.Deserialize<UniversalPosTag>(json);
        Assert.Equal(tag, deserialized);
    }

    public static readonly TheoryData<string> Tags = new()
    {
        "ADJ",
        "ADP",
        "ADV",
        "AUX",
        "CCONJ",
        "DET",
        "INTJ",
        "NOUN",
        "NUM",
        "PART",
        "PRON",
        "PROPN",
        "PUNCT",
        "SCONJ",
        "SYM",
        "VERB",
        "X"
    };

}

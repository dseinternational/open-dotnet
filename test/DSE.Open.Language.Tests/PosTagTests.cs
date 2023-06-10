// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Tests;

public class PosTagTests
{
    [Theory]
    [MemberData(nameof(Tags))]
    public void Parse_valid_tags(string tagStr)
    {
        var tag = PosTag.Parse(tagStr);
        Assert.NotEqual(default, tag);
    }

    public static readonly TheoryData<string> Tags = new()
    {
        "JJ",
        "CC",
        "DT",
        "UH",
        "NN",
        "CD",
        "IN",
        "PRP",
        "VB",
        "RP",

        "NOUN",
        "VERB",
        "ADJ",
        "ADV",
        "PRON",
        "DET",
        "ADP",
        "CONJ",
        "PRT",
        "NUM",
        "X"
    };

}

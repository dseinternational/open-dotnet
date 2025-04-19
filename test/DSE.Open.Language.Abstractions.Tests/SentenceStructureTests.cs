// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Language;

public class SentenceStructureTests
{
    [Theory]
    [MemberData(nameof(Tags))]
    public void Parse_valid_tags(string tagStr)
    {
        var tag = SentenceStructure.ParseInvariant(tagStr);
        Assert.NotEqual(default, tag);
    }

    [Theory]
    [MemberData(nameof(Tags))]
    public void Serialize_deserialize(string tagStr)
    {
        var tag = SentenceStructure.ParseInvariant(tagStr);
        AssertJson.Roundtrip(tag);
    }

    public static readonly TheoryData<string> Tags =
    [
        "simple",
        "compound",
        "complex",
        "compound-complex",
    ];
}

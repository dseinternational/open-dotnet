// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Language;

public class SentenceFunctionTests
{
    [Theory]
    [MemberData(nameof(Tags))]
    public void Parse_valid_tags(string tagStr)
    {
        var tag = SentenceFunction.ParseInvariant(tagStr);
        Assert.NotEqual(default, tag);
    }

    [Theory]
    [MemberData(nameof(Tags))]
    public void Serialize_deserialize(string tagStr)
    {
        var tag = SentenceFunction.ParseInvariant(tagStr);
        AssertJson.Roundtrip(tag);
    }

    public static readonly TheoryData<string> Tags =
    [
        "declarative",
        "interrogative",
        "imperative",
        "exclamatory",
    ];
}

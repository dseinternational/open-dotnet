// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language.Annotations;

public class SemanticRelationTagTests
{
    [Theory]
    [MemberData(nameof(Tags))]
    public void Parse_valid_tags(string tagStr)
    {
        var tag = SemanticRelationTag.Parse(tagStr, CultureInfo.InvariantCulture);
        Assert.NotEqual(default, tag);
    }

    [Theory]
    [MemberData(nameof(Tags))]
    public void Serialize_deserialize(string tagStr)
    {
        var tag = SemanticRelationTag.Parse(tagStr, CultureInfo.InvariantCulture);
        var json = JsonSerializer.Serialize(tag);
        var deserialized = JsonSerializer.Deserialize<SemanticRelationTag>(json);
        Assert.Equal(tag, deserialized);
    }

    public static readonly TheoryData<string> Tags = new()
    {
        "agent-action",
        "action-object",
        "agent-object",
        "possessive",
        "descriptive",
        "locative",
        "temporal",
        "conjunctive",
        "existence",
        "recurrence",
        "non-existence",
        "rejection",
        "denial",
    };

}

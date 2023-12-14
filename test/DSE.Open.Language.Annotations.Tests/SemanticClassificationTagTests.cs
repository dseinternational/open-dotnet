// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language.Annotations;

public class SemanticClassificationTagTests
{
    [Theory]
    [MemberData(nameof(Tags))]
    public void Parse_valid_tags(string tagStr)
    {
        var tag = SemanticClassificationTag.Parse(tagStr, CultureInfo.InvariantCulture);
        Assert.NotEqual(default, tag);
    }

    [Theory]
    [MemberData(nameof(Tags))]
    public void Serialize_deserialize(string tagStr)
    {
        var tag = SemanticClassificationTag.Parse(tagStr, CultureInfo.InvariantCulture);
        var json = JsonSerializer.Serialize(tag);
        var deserialized = JsonSerializer.Deserialize<SemanticClassificationTag>(json);
        Assert.Equal(tag, deserialized);
    }

    public static readonly TheoryData<string> Tags = new()
    {
        "agent-action",
        "action-object",
        "agent-object",
        "possessor-possession",
        "attribute-entity",
        "location",
        "action-location",
        "entity-location",
        "temporal",
        "conjunctive",
        "demonstrative-entity",
        "recurrence",
        "non-existence",
        "rejection",
        "denial",
        "agent-action-object",
        "agent-action-location",
    };

}

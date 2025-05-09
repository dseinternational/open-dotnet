// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Language;

public class SemanticRelationTests
{
    [Theory]
    [MemberData(nameof(Tags))]
    public void Parse_valid_tags(string tagStr)
    {
        var tag = SemanticRelation.ParseInvariant(tagStr);
        Assert.NotEqual(default, tag);
    }

    [Theory]
    [MemberData(nameof(Tags))]
    public void Serialize_deserialize(string tagStr)
    {
        var tag = SemanticRelation.ParseInvariant(tagStr);
        AssertJson.Roundtrip(tag);
    }

    public static readonly TheoryData<string> Tags =
    [
        "agent-action",
        "action-object",
        "agent-object",
        "possessor-possession",
        "attribute-entity",
        "action-location",
        "entity-location",
        "temporal",
        "conjunctive",
        "demonstrative-entity",
        "recurrence",
        "non-existence",
        "agent-action-object",
        "agent-action-location",
    ];
}

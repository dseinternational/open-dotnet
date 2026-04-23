// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language.Annotations;

public sealed class UniversalRelationTagTests
{
    [Theory]
    [MemberData(nameof(Tags))]
    public void Parse_valid_tags(string tagStr)
    {
        var tag = UniversalRelationTag.ParseInvariant(tagStr);
        Assert.NotEqual(default, tag);
        Assert.Equal(tagStr, tag.ToStringInvariant());
    }

    [Theory]
    [MemberData(nameof(Tags))]
    public void Serialize_deserialize(string tagStr)
    {
        var tag = UniversalRelationTag.ParseInvariant(tagStr);
        var json = JsonSerializer.Serialize(tag);
        var deserialized = JsonSerializer.Deserialize<UniversalRelationTag>(json);
        Assert.Equal(tag, deserialized);
    }

    [Fact]
    public void TryParse_rejects_unknown_value()
    {
        Assert.False(UniversalRelationTag.TryParse("not-a-relation", out _));
    }

    [Fact]
    public void Repeatable_hash_is_stable_for_equal_values()
    {
        var a = UniversalRelationTag.NominalSubject;
        var b = UniversalRelationTag.ParseInvariant("nsubj");
        Assert.Equal(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }

    [Fact]
    public void Repeatable_hash_differs_between_distinct_relations()
    {
        Assert.NotEqual(
            UniversalRelationTag.NominalSubject.GetRepeatableHashCode(),
            UniversalRelationTag.Object.GetRepeatableHashCode());
    }

    public static readonly TheoryData<string> Tags =
    [
        "acl",
        "acl:relcl",
        "advcl",
        "advmod",
        "amod",
        "appos",
        "aux",
        "case",
        "cc",
        "ccomp",
        "conj",
        "cop",
        "det",
        "mark",
        "nmod",
        "nsubj",
        "nsubj:pass",
        "nummod",
        "obj",
        "obl",
        "punct",
        "root",
        "xcomp",
    ];
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language.Annotations;

public sealed class PosTagTests
{
    [Theory]
    [InlineData("NOUN")]
    [InlineData("VERB")]
    [InlineData("ADJ")]
    public void IsValidUniversalPosTag_recognises_universal_tags(string value)
    {
        var tag = PosTag.ParseInvariant(value);
        Assert.True(tag.IsValidUniversalPosTag);
    }

    [Theory]
    [InlineData("NN")]
    [InlineData("VBZ")]
    [InlineData("JJ")]
    public void IsValidTreebankTag_recognises_treebank_tags(string value)
    {
        var tag = PosTag.ParseInvariant(value);
        Assert.True(tag.IsValidTreebankTag);
    }

    [Fact]
    public void ToUniversalPosTag_converts_recognised_tag()
    {
        var tag = PosTag.ParseInvariant("NOUN");
        var upos = tag.ToUniversalPosTag();
        Assert.Equal(UniversalPosTag.Noun, upos);
    }

    [Fact]
    public void ToTreebankPosTag_converts_recognised_tag()
    {
        var tag = PosTag.ParseInvariant("NN");
        var xpos = tag.ToTreebankPosTag();
        Assert.Equal(TreebankPosTag.NounSingularOrMass, xpos);
    }

    [Fact]
    public void Length_returns_character_count()
    {
        var tag = PosTag.ParseInvariant("VERB");
        Assert.Equal(4, tag.Length);
    }

    [Fact]
    public void Serialize_deserialize_roundtrips()
    {
        var tag = PosTag.ParseInvariant("NOUN");
        var json = JsonSerializer.Serialize(tag);
        var deserialized = JsonSerializer.Deserialize<PosTag>(json);
        Assert.Equal(tag, deserialized);
    }

    [Fact]
    public void Explicit_conversion_from_string_preserves_value()
    {
        var tag = (PosTag)"NOUN";
        Assert.Equal("NOUN", tag.ToStringInvariant());
    }

    [Fact]
    public void Repeatable_hash_is_stable_for_equal_values()
    {
        var a = PosTag.ParseInvariant("NOUN");
        var b = PosTag.ParseInvariant("NOUN");
        Assert.Equal(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Language.Annotations;

public sealed class DocumentTests
{
    [Fact]
    public void Default_sentences_is_empty()
    {
        var doc = new Document { Sentences = [] };
        Assert.Empty(doc.Sentences);
        Assert.Null(doc.Id);
        Assert.Null(doc.Language);
    }

    [Fact]
    public void Serialize_roundtrip_with_no_sentences()
    {
        var doc = new Document
        {
            Id = "doc-1",
            Language = LanguageTag.EnglishUk,
            Sentences = [],
        };

        var json = JsonSerializer.Serialize(doc);
        var back = JsonSerializer.Deserialize<Document>(json);

        Assert.NotNull(back);
        Assert.Equal(doc.Id, back.Id);
        Assert.Equal(doc.Language, back.Language);
        Assert.Empty(back.Sentences);
    }

    [Fact]
    public void Repeatable_hash_is_stable_for_equal_documents()
    {
        var a = new Document
        {
            Id = "doc-1",
            Language = LanguageTag.EnglishUk,
            Sentences = new ReadOnlyValueCollection<Sentence>([]),
        };

        var b = new Document
        {
            Id = "doc-1",
            Language = LanguageTag.EnglishUk,
            Sentences = new ReadOnlyValueCollection<Sentence>([]),
        };

        Assert.Equal(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }

    [Fact]
    public void Repeatable_hash_differs_when_id_changes()
    {
        var a = new Document { Id = "doc-1", Sentences = [] };
        var b = new Document { Id = "doc-2", Sentences = [] };
        Assert.NotEqual(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }

    [Fact]
    public void Repeatable_hash_differs_when_language_changes()
    {
        var a = new Document { Language = LanguageTag.EnglishUk, Sentences = [] };
        var b = new Document { Language = LanguageTag.EnglishUs, Sentences = [] };
        Assert.NotEqual(a.GetRepeatableHashCode(), b.GetRepeatableHashCode());
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Transformers;

[Collection("SentenceTransformersService")]
public class SentenceTransformerTests : SentenceTransformerTestsBase
{
    public SentenceTransformerTests(SentenceTransformersServiceFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact]
    public void Encode_MultipleSentences_ReturnsEmbeddings()
    {
        string[] chunks =
        [
            "Emma is playing the park with her mother.",
            "Tom is at school with his friends.",
            "Emma and her mother are playing on the swing."
        ];

        var embeddings = NomicTextEmbed.Encode(chunks, "search_document: ");

        Assert.Equal(2, embeddings.Rank);
        Assert.Equal(3, embeddings.Lengths[0]);
        Assert.Equal(768, embeddings.Lengths[1]);
    }

    [Fact]
    public void SingleSentence_ReturnsEmbeddings()
    {
        string[] chunks =
        [
            "Emma is playing the park with her mother.",
        ];

        var embeddings = NomicTextEmbed.Encode(chunks, "search_document: ");

        Assert.Equal(2, embeddings.Rank);
        Assert.Equal(1, embeddings.Lengths[0]);
        Assert.Equal(768, embeddings.Lengths[1]);
    }
}

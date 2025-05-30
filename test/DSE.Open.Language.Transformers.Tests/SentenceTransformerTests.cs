// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Language.Transformers;

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

[Collection("SentenceTransformersService")]
public class SentenceTransformerTests : SentenceTransformerTestsBase
{
    public SentenceTransformerTests(SentenceTransformersServiceFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact]
    public void Encode_EmptyCollection_ReturnsEmptyTensor()
    {
        string[] chunks = [];

        var embeddings = NomicTextEmbed.GetEmbeddings(chunks, "search_document: ");

        Assert.Equal(2, embeddings.Rank);
        Assert.Equal(0, embeddings.Lengths[0]);
        Assert.Equal(0, embeddings.Lengths[1]);
        Assert.True(embeddings.IsEmpty);
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

        var embeddings = NomicTextEmbed.GetEmbeddings(chunks, "search_document: ");

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

        var embeddings = NomicTextEmbed.GetEmbeddings(chunks, "search_document: ");

        Assert.Equal(2, embeddings.Rank);
        Assert.Equal(1, embeddings.Lengths[0]);
        Assert.Equal(768, embeddings.Lengths[1]);
    }

    [Fact]
    public void CosineSimilarity_ScoresSentencesSuccessfully()
    {
        string[] chunks =
        [
            "search_document: Emma is playing the park with her mother.",
            "search_document: Tom is at school with his friends.",
            "search_document: Emma and her mother are playing on the swing.",
            "search_query: Where is Emma?"
        ];

        var embeddings = NomicTextEmbed.GetEmbeddings(chunks, "search_document: ");

        Output.WriteLine($"embeddings: {embeddings.GetDescription()}");

        Assert.Equal(2, embeddings.Rank);
        Assert.Equal(4, embeddings.Lengths[0]);
        Assert.Equal(768, embeddings.Lengths[1]);

        var s0 = embeddings.Slice([0..1, 0..768]); // "Emma is playing in the park with her mother."
        var s1 = embeddings.Slice([1..2, 0..768]); // "Tom is at school with his friends."
        var s2 = embeddings.Slice([2..3, 0..768]); // "Emma and her mother are playing on the swing."
        var s3 = embeddings.Slice([3..4, 0..768]); // "Where is Emma?"

        // "Tom is at school with his friends." is less similar to "Emma is playing in the park with her mother."
        // than "Emma and her mother are playing on the swing."
        Assert.True(Tensor.CosineSimilarity(s0, s1)[0, 0] < Tensor.CosineSimilarity(s0, s2)[0, 0]);

        // "Emma is playing in the park with her mother." is a closer answer to "Where is Emma?"
        // than "Tom is at school with his friends."
        Assert.True(Tensor.CosineSimilarity(s3, s0)[0, 0] > Tensor.CosineSimilarity(s3, s1)[0, 0]);

        // "Emma and her mother are playing on the swing." is a closer answer to "Where is Emma?"
        // than "Tom is at school with his friends."
        Assert.True(Tensor.CosineSimilarity(s3, s2)[0, 0] > Tensor.CosineSimilarity(s0, s1)[0, 0]);
    }
}

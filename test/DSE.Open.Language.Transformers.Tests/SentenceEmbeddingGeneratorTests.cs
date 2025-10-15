// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Language.Transformers;


[Collection("SentenceTransformersService")]
public class SentenceEmbeddingGeneratorTests : SentenceTransformerTestsBase
{
    public SentenceEmbeddingGeneratorTests(SentenceTransformersServiceFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        EmbeddingGenerator = new SentenceEmbeddingGenerator(NomicTextEmbed);
    }

    public SentenceEmbeddingGenerator EmbeddingGenerator { get; }

    [Fact]
    public async Task GenerateAsync_ReturnsEmbeddings()
    {
        string[] chunks =
        [
            "Emma is playing the park with her mother.",
            "Tom is at school with his friends.",
            "Emma and her mother are playing on the swing."
        ];

        var embeddings = await EmbeddingGenerator.GenerateAsync(
            chunks,
            new Microsoft.Extensions.AI.EmbeddingGenerationOptions
            {
                AdditionalProperties = new Microsoft.Extensions.AI.AdditionalPropertiesDictionary
                {
                    { "prompt", "search_document" }
                }
            },
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(3, embeddings.Count);

        for (var i = 0; i < embeddings.Count; i++)
        {
            Assert.Equal(768, embeddings[i].Dimensions);
            Assert.Equal("nomic-ai/nomic-embed-text-v1.5", embeddings[i].ModelId);
            _ = Assert.NotNull(embeddings[i].CreatedAt);
        }

        var cs_0_1 = TensorPrimitives.CosineSimilarity(embeddings[0].Vector.Span, embeddings[1].Vector.Span);
        var cs_0_2 = TensorPrimitives.CosineSimilarity(embeddings[0].Vector.Span, embeddings[2].Vector.Span);
        Assert.True(cs_0_1 < cs_0_2);
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.AI;

namespace DSE.Open.Language.Transformers;

public sealed class SentenceEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
{
    private readonly SentenceTransformer _sentenceTransformer;

    public SentenceEmbeddingGenerator(SentenceTransformer sentenceTransformer)
    {
        ArgumentNullException.ThrowIfNull(sentenceTransformer);
        _sentenceTransformer = sentenceTransformer;
    }

    public Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(
        IEnumerable<string> values,
        EmbeddingGenerationOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(values);

        return Task.Run(() =>
        {
            string? prompt = null;

            _ = options?.AdditionalProperties?.TryGetValue("prompt", out prompt);

            var tensor = _sentenceTransformer.GetEmbeddings([.. values], prompt: prompt);

            // expect 2d tensor with shape [values.Count, embedding_length]

            if (tensor.Rank != 2)
            {
                throw new InvalidOperationException("Expected a 2D tensor.");
            }

            var embeddingCount = (int)tensor.Lengths[0];
            var embeddingLength = (int)tensor.Lengths[1];

            var embeddings = new Embedding<float>[embeddingCount];

            for (var i = 0; i < embeddingCount; i++)
            {
                var data = new float[embeddingLength];
                tensor.Slice([i..(i + 1), 0..embeddingLength]).FlattenTo(data);
                embeddings[i] = new Embedding<float>(data)
                {
                    ModelId = _sentenceTransformer.ModelName,
                    CreatedAt = DateTimeOffset.Now
                };
            }

            return new GeneratedEmbeddings<Embedding<float>>(embeddings);
        }, cancellationToken);
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        return serviceType is null ? throw new ArgumentNullException(nameof(serviceType)) :
        serviceKey is not null ? null :
        serviceType.IsInstanceOfType(this) ? this :
        serviceType == typeof(SentenceEmbeddingGenerator) ? this :
        null;
    }

    public void Dispose()
    {
        _sentenceTransformer.Dispose();
    }
}

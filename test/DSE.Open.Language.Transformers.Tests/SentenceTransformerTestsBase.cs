// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CSnakes.Runtime;

namespace DSE.Open.Language.Transformers;

public abstract class SentenceTransformerTestsBase
{
    private SentenceTransformer? _nomicTextEmbed;
    private readonly Lock _nomicTextEmbedLock = new();

    protected SentenceTransformerTestsBase(SentenceTransformersServiceFixture fixture, ITestOutputHelper output)
    {
        Fixture = fixture;
        Output = output;
    }

    public SentenceTransformersServiceFixture Fixture { get; }

    public ISentenceTransformersService SentenceTransformersService => Fixture.SentenceTransformersService;

    public SentenceTransformer NomicTextEmbed
    {
        get
        {
            if (_nomicTextEmbed is null)
            {
                lock (_nomicTextEmbedLock)
                {
#pragma warning disable CA1508 // Avoid dead conditional code
                    _nomicTextEmbed ??= SentenceTransformer.Create(
                        Fixture.PythonEnvironment,
                        "nomic-ai/nomic-embed-text-v1.5",
                        trustExternalCode: true);
#pragma warning restore CA1508 // Avoid dead conditional code
                }
            }

            return _nomicTextEmbed;
        }
    }

    public ITestOutputHelper Output { get; }
}

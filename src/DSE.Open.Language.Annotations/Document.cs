// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;
using DSE.Open.Hashing;

namespace DSE.Open.Language.Annotations;

public record Document : IRepeatableHash64
{
    [JsonPropertyName("doc_id")]
    public string? Id { get; init; }

    [JsonPropertyName("language")]
    public LanguageTag? Language { get; init; }

    [JsonPropertyName("sentences")]
    public required ReadOnlyValueCollection<Sentence> Sentences { get; init; } = [];

    public ulong GetRepeatableHashCode()
    {
        var hash = RepeatableHash64Provider.Default.CombineHashCodes(
            RepeatableHash64Provider.Default.GetRepeatableHashCode(Id.AsSpan()),
            Language?.GetRepeatableHashCode() ?? 0u);

        foreach (var sentence in Sentences)
        {
            hash = RepeatableHash64Provider.Default.CombineHashCodes(hash, sentence.GetRepeatableHashCode());
        }

        return hash;
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open;
using Xunit;

namespace DSE.Open.Testing.Xunit.Stanza;

public sealed class StanzaNlpFactAttribute : FactAttribute
{
    public StanzaNlpFactAttribute(params string[] languages)
    {
        Languages = languages;

        var stanzaNlp = Environment.GetEnvironmentVariable("STANZA_NLP");

        if (!string.IsNullOrWhiteSpace(stanzaNlp))
        {
            var languagesProvided = stanzaNlp.Split(',').ToArray();

            // No languages specified, means we just need stanza installed with any languages
            // Languages specified, means we need stanza installed and the languages specified

            if (!languages.All(lr => languagesProvided.Contains(lr, StringComparer.InvariantCultureIgnoreCase)))
            {
                Skip = "Stanza NLP is not installed with the required languages: " +
                    $"{string.Join(", ", languages)} (STANZA_NLP: {stanzaNlp})";
            }
        }
        else
        {
            Skip = "Stanza NLP is not installed. Should the STANZA_NLP environment variable be set?";
        }
    }

    public IReadOnlyList<string> Languages { get; }
}

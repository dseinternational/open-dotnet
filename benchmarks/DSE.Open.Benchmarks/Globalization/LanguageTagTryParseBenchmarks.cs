// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;
using DSE.Open.Globalization;

namespace DSE.Open.Benchmarks.Globalization;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class LanguageTagTryParseBenchmarks
{
    // Best guess at a set of valid language tags which hit the regex
    private static readonly string[] s_validRegexHits =
    {
        "az-Cyrl", "az-Cyrl-AZ", "az-Latn", "az-Latn-AZ", "bs-Cyrl", "bs-Cyrl-BA", "bs-Latn", "bs-Latn-BA",
        "zh-Hans", "zh-Hant", "en-029", "ha-Latn", "ha-Latn-NG", "iu-Latn", "iu-Latn-CA", "iu-Cans", "iu-Cans-CA",
        "kok-IN", "dsb-DE", "arn-CL", "mn-Cyrl", "mn-Mong-CN", "quz-BO", "quz-EC", "quz-PE", "smn-FI", "smj-NO",
        "smj-SE", "se-FI", "se-NO", "se-SE", "sms-FI", "sma-NO", "sma-SE", "sr-Cyrl", "sr-Cyrl-BA", "sr-Cyrl-ME",
        "sr-Cyrl-CS", "sr-Cyrl-RS", "sr-Latn", "sr-Latn-BA", "sr-Latn-ME", "sr-Latn-CS", "sr-Latn-RS", "nso-ZA",
        "tg-Cyrl-TJ", "tzm-Latn", "tzm-Latn-DZ", "uz-Cyrl", "uz-Cyrl-UZ", "uz-Latn", "uz-Latn-UZ", "sah-RU",
        "az-Arab", "az-Cyrl", "az-Latn", "sr-Cyrl", "sr-Latn", "uz-Cyrl", "uz-Latn", "zh-Hans", "zh-Hant"
    };

    // Best guess at a set of invalid language tags which hit the regex
    private static readonly string[] s_invalidRegexHits =
    {
        "az-Cyrl-AZ-BA", "az-Latn-AZ-BA", "bs-Cyrl-BA-BA", "bs-Latn-BA-BA", "zh-Hans-Hans", "zh-Hant-Hant",
        "en-029-029", "ha-Latn-NG-NG", "iu-Latn-CA-CA", "iu-Cans-CA-CA", "kok-IN-IN", "dsb-DE-DE", "arn-CL-CL",
        "mn-Cyrl-CN-CN", "mn-Mong-CN-CN", "quz-BO-BO", "quz-EC-EC", "quz-PE-PE", "smn-FI-FI", "smj-NO-NO",
        "smj-SE-SE", "se-FI-FI", "se-NO-NO", "se-SE-SE", "sms-FI-FI", "sma-NO-NO", "sma-SE-SE", "sr-Cyrl-BA-BA",
        "sr-Cyrl-ME-ME", "sr-Cyrl-CS-CS", "sr-Cyrl-RS-RS", "sr-Latn-BA-BA", "sr-Latn-ME-ME", "sr-Latn-CS-CS",
        "sr-Latn-RS-RS", "nso-ZA-ZA", "tg-Cyrl-TJ-TJ", "tzm-Latn-DZ-DZ", "uz-Cyrl-UZ-UZ", "uz-Latn-UZ-UZ",
        "sah-RU-RU", "az-Arab-Arab", "az-Cyrl-Cyrl", "az-Latn-Latn", "sr-Cyrl-Cyrl", "sr-Latn-Latn", "uz-Cyrl-Cyrl",
        "uz-Latn-Latn", "zh-Hans-Hans", "zh-Hant-Hant"
    };

    [Benchmark]
    public void TryParse_Valid()
    {
        foreach (var tag in s_validRegexHits)
        {
            _ = LanguageTag.TryParse(tag, out _);
        }
    }

    [Benchmark]
    public void TryParse_Invalid()
    {
        foreach (var tag in s_invalidRegexHits)
        {
            _ = LanguageTag.TryParse(tag, out _);
        }
    }
}

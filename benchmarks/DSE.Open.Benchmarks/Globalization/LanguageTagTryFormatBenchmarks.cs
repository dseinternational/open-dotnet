// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Benchmarks.Globalization;

[MemoryDiagnoser]
public class LanguageTagTryFormatBenchmarks
{
    private static readonly LanguageTag[] s_tags = new[]
    {
        LanguageTag.EnglishAustralia,
        LanguageTag.EnglishCanada,
        LanguageTag.EnglishUk,
        LanguageTag.EnglishUs,
        LanguageTag.EnglishIndia,
        LanguageTag.EnglishIreland,
        LanguageTag.EnglishNewZealand,
        LanguageTag.EnglishSouthAfrica,
        LanguageTag.Parse("fr-FR", null),
        LanguageTag.Parse("en-CA-x-ca", null),
    };

    [Benchmark]
    public void TryFormatNormalized()
    {
        s_tags.ForEach(t =>
        {
            Span<char> b = stackalloc char[t.Length];
            _ = t.TryFormat(b, out var cw, "N".AsSpan(), CultureInfo.InvariantCulture);
        });
    }

    [Benchmark]
    public void TryFormatLowercase()
    {
        s_tags.ForEach(t =>
        {
            Span<char> b = stackalloc char[t.Length];
            _ = t.TryFormat(b, out var cw, "L".AsSpan(), CultureInfo.InvariantCulture);
        });
    }

    [Benchmark]
    public void TryFormatUppercase()
    {
        s_tags.ForEach(t =>
        {
            Span<char> b = stackalloc char[t.Length];
            _ = t.TryFormat(b, out var cw, "U".AsSpan(), CultureInfo.InvariantCulture);
        });
    }

    [Benchmark(Baseline = true)]
    public void TryFormatDefault()
    {
        s_tags.ForEach(t =>
        {
            Span<char> b = stackalloc char[t.Length];
            _ = t.TryFormat(b, out var cw, default, CultureInfo.InvariantCulture);
        });
    }
}

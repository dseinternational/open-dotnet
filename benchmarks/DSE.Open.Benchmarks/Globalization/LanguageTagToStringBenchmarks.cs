// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;
using DSE.Open.Collections.Generic;
using DSE.Open.Globalization;

namespace DSE.Open.Benchmarks.Globalization;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
public class LanguageTagToStringBenchmarks
{
    private static readonly LanguageTag[] s_tags =
    [
        LanguageTag.EnglishAustralia,
        LanguageTag.EnglishCanada,
        LanguageTag.EnglishUk,
        LanguageTag.EnglishUs,
        LanguageTag.EnglishIndia,
        LanguageTag.EnglishIreland,
        LanguageTag.EnglishNewZealand,
        LanguageTag.EnglishSouthAfrica,
        LanguageTag.ParseInvariant("fr-FR"),
        LanguageTag.ParseInvariant("en-CA-x-ca"),
    ];

    [Benchmark]
    public void ToStringFormatted()
    {
        s_tags.ForEach(t => t.ToStringFormatted());
    }

    [Benchmark(Baseline = true)]
    public void ToStringUnformatted()
    {
        s_tags.ForEach(t => t.ToString());
    }

    [Benchmark]
    public void ToStringLower()
    {
        s_tags.ForEach(t => t.ToStringLower());
    }

    [Benchmark]
    public void ToStringUpper()
    {
        s_tags.ForEach(t => t.ToStringUpper());
    }
}

#pragma warning restore CA1822 // Mark members as static

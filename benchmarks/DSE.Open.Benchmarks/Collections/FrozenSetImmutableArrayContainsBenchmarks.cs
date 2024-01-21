// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;

namespace DSE.Open.Benchmarks.Collections;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser(false)]
public class FrozenSetImmutableArrayContainsBenchmarks
{
    [Benchmark]
    public bool ImmutableArray_Contains()
    {
        return AlphabetArray.Contains('a');
    }

    [Benchmark]
    public bool ImmutableArray_AsSpan_Contains()
    {
        return AlphabetArray.AsSpan().Contains('a');
    }

    [Benchmark(Baseline = true)]
    public bool FrozenSet_Contains()
    {
        return AlphabetSet.Contains('a');
    }

    public static readonly ImmutableArray<char> AlphabetArray =
    [
        // Plosives
        'p',
        'b',
        't',
        'd',
        'ʈ',
        'ɖ',
        'c',
        'ɟ',
        'k',
        'g',
        'q',
        'ɢ',

        // Nasals
        'm',
        'ɱ',
        'n',
        'ɳ',
        'ɲ',
        'ŋ',
        'ɴ',

        // Trills
        'ʙ',
        'r',
        'ʀ',

        // Taps or Flaps
        'ɾ',
        'ɽ',

        // Fricatives
        'ɸ',
        'β',
        'f',
        'v',
        'θ',
        'ð',
        's',
        'z',
        'ʃ',
        'ʒ',
        'ʂ',
        'ʐ',
        'ç',
        'ʝ',
        'x',
        'ɣ',
        'χ',
        'ʁ',
        'ħ',
        'ʕ',
        'h',
        'ɦ',

        // Lateral fricatives
        'ɬ',
        'ɮ',

        // Approximants
        'ʋ',
        'ɹ',
        'ɻ',
        'j',
        'ɰ',

        // Laterals
        'l',
        'ɭ',
        'ʎ',
        'ʟ',

        // Vowels
        'i',
        'y',
        'ɨ',
        'ʉ',
        'ɯ',
        'u',
        'ɪ',
        'ʏ',
        'ʊ',
        'e',
        'ø',
        'ɘ',
        'ɵ',
        'ɤ',
        'o',
        'ɛ',
        'œ',
        'ɜ',
        'ɞ',
        'ʌ',
        'ɔ',
        'æ',
        'ɐ',
        'a',
        'ɶ',
        'ä',
        'ɑ',
        'ɒ',

        'ə',

        // Diacritics and suprasegmentals
        'ˈ',
        'ˌ',
        'ː',
        'ˑ',
        'ʼ',
        'ʴ',
        'ʵ',
        'ʶ',
        'ʰ',
        'ʱ',
        'ʲ',
        'ʷ',
        'ˠ',
        'ˤ',
        'ˁ',

        // TODO: review these

        // Additional diacritics
        '̥',
        '̬',
        '̹',
        '̜',
        '̟',
        '̠',
        '̈',
        '̽',
        '̩',
        '̯',
        '̪',
        '̺',
        '̻',
        '̼',
        '̝',
        '̞',
        '̘',
        '̙',
        '̆',
        '̊',

        // Tone letters and other notations
        '˥',
        '˦',
        '˧',
        '˨',
        '˩',
        '↗',
        '↘'
    ];

    public static readonly FrozenSet<char> AlphabetSet = FrozenSet.ToFrozenSet(AlphabetArray);
}

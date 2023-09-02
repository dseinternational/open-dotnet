// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Speech.Abstractions;

/// <summary>
/// A set of <see cref="PhonemeLabel"/>s.
/// </summary>
public abstract class PhonemeLabelSet : IReadOnlyCollection<PhonemeLabel>
{
    private readonly Dictionary<Phoneme, PhonemeLabel> _phonemeLookup = new();
    private readonly Dictionary<string, PhonemeLabel> _labelLookup = new();

    protected PhonemeLabelSet(IEnumerable<PhonemeLabel> notations)
    {
        Guard.IsNotNull(notations);

        foreach (var ps in notations)
        {
            _phonemeLookup.Add(ps.Phoneme, ps);
            _labelLookup.Add(ps.Label, ps);
        }
    }

    public int Count => _phonemeLookup.Count;

    public bool ContainsPhoneme(Phoneme phoneme) => _phonemeLookup.ContainsKey(phoneme);

    public bool ContainsLabel(string label) => _labelLookup.ContainsKey(label);

    public IEnumerator<PhonemeLabel> GetEnumerator() => _phonemeLookup.Values.GetEnumerator();

    public bool TryGetForPhoneme(
        Phoneme key,
        [MaybeNullWhen(false)] out PhonemeLabel value)
        => _phonemeLookup.TryGetValue(key, out value);

    public bool TryGetForSymbol(
        string key,
        [MaybeNullWhen(false)] out PhonemeLabel value)
        => _labelLookup.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

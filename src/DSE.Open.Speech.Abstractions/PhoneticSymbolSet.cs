// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Speech.Abstractions;

/// <summary>
/// A set of <see cref="PhoneticSymbol"/>s.
/// </summary>
public abstract class PhoneticSymbolSet : IReadOnlyCollection<PhoneticSymbol>
{
    private readonly Dictionary<Phoneme, PhoneticSymbol> _phonemeLookup = new();
    private readonly Dictionary<string, PhoneticSymbol> _symbolLookup = new();

    protected PhoneticSymbolSet(IEnumerable<PhoneticSymbol> notations)
    {
        Guard.IsNotNull(notations);

        foreach (var ps in notations)
        {
            _phonemeLookup.Add(ps.Phoneme, ps);
            _symbolLookup.Add(ps.Symbol, ps);
        }
    }

    public int Count => _phonemeLookup.Count;

    public bool ContainsPhoneme(Phoneme phoneme) => _phonemeLookup.ContainsKey(phoneme);

    public bool ContainsSymbol(string symbol) => _symbolLookup.ContainsKey(symbol);

    public IEnumerator<PhoneticSymbol> GetEnumerator() => _phonemeLookup.Values.GetEnumerator();

    public bool TryGetForPhoneme(
        Phoneme key,
        [MaybeNullWhen(false)] out PhoneticSymbol value)
        => _phonemeLookup.TryGetValue(key, out value);

    public bool TryGetForSymbol(
        string key,
        [MaybeNullWhen(false)] out PhoneticSymbol value)
        => _symbolLookup.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

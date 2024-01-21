// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Globalization;

namespace DSE.Open.Speech;

/// <summary>
/// A set of <see cref="PhonemeSymbol"/>s.
/// </summary>
[Obsolete("To be removed.")]
public abstract class PhonemeSymbolSet : IReadOnlyCollection<PhonemeSymbol>
{
    private readonly List<PhonemeSymbol> _symbols = new();
    private readonly Dictionary<LanguageTag, Dictionary<string, PhonemeSymbol>> _languageLabelLookup = new();

    protected PhonemeSymbolSet(IEnumerable<PhonemeSymbol> symbols)
    {
        ArgumentNullException.ThrowIfNull(symbols);

        foreach (var ps in symbols)
        {
            _symbols.Add(ps);

            if (!_languageLabelLookup.TryGetValue(ps.Language, out var labelLookup))
            {
                labelLookup = new Dictionary<string, PhonemeSymbol>(StringComparer.InvariantCulture);

                _languageLabelLookup.Add(ps.Language, labelLookup);
            }

            if (!labelLookup.TryAdd(ps.Symbol, ps))
            {
                throw new InvalidOperationException($"Duplicate symbol '{ps.Symbol}' for language '{ps.Language}'");
            }
        }
    }

    public int Count => _symbols.Count;

    public bool ContainsSymbol(LanguageTag language, string Symbol)
    {
        return _languageLabelLookup.TryGetValue(language, out var lookup) && lookup.ContainsKey(Symbol);
    }

    public IEnumerable<PhonemeSymbol> SelectByLanguage(LanguageTag language)
    {
        if (_languageLabelLookup.TryGetValue(language, out var lookup))
        {
            return lookup.Values;
        }

        return Enumerable.Empty<PhonemeSymbol>();
    }

    public IEnumerator<PhonemeSymbol> GetEnumerator()
    {
        return _symbols.GetEnumerator();
    }

    public bool TryGetLookup(
        LanguageTag language,
        [MaybeNullWhen(false)] out Dictionary<string, PhonemeSymbol> lookup)
    {
        if (_languageLabelLookup.TryGetValue(language, out lookup))
        {
            return true;
        }

        lookup = default;
        return false;
    }

    public bool TryGetPhonemeSymbol(
        LanguageTag language,
        string Symbol,
        [MaybeNullWhen(false)] out PhonemeSymbol value)
    {
        if (_languageLabelLookup.TryGetValue(language, out var lookup)
            && lookup.TryGetValue(Symbol, out value))
        {
            return true;
        }

        value = default;
        return false;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

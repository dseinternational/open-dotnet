// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using DSE.Open.Globalization;

namespace DSE.Open.Speech;

/// <summary>
/// A set of <see cref="PhonemeLabel"/>s.
/// </summary>
public abstract class PhonemeLabelSet : IReadOnlyCollection<PhonemeLabel>
{
    private readonly List<PhonemeLabel> _lookup = new();
    private readonly Dictionary<LanguageTag, Dictionary<string, PhonemeLabel>> _lookup2 = new();

    protected PhonemeLabelSet(IEnumerable<PhonemeLabel> notations)
    {
        Guard.IsNotNull(notations);

        foreach (var ps in notations)
        {
            _lookup.Add(ps);

            if (!_lookup2.TryGetValue(ps.Language, out var value))
            {
                value = new Dictionary<string, PhonemeLabel>(StringComparer.InvariantCulture);
                _lookup2.Add(ps.Language, value);
            }

            value.Add(ps.Label, ps);
        }
    }

    public int Count => _lookup.Count;

    public bool ContainsLabel(LanguageTag language, string label) => _lookup2.TryGetValue(language, out var lookup) && lookup.ContainsKey(label);

    public IEnumerable<PhonemeLabel> SelectByLanguage(LanguageTag language)
    {
        if (_lookup2.TryGetValue(language, out var lookup))
        {
            return lookup.Values;
        }

        return Enumerable.Empty<PhonemeLabel>();
    }

    public IEnumerator<PhonemeLabel> GetEnumerator() => _lookup.GetEnumerator();

    public bool TryGetLookup(
        LanguageTag language,
        [MaybeNullWhen(false)] out Dictionary<string, PhonemeLabel> lookup)
    {
        if (_lookup2.TryGetValue(language, out lookup)) return true;

        lookup = default;
        return false;
    }

    public bool TryGetPhonemeLabel(
        LanguageTag language,
        string label,
        [MaybeNullWhen(false)] out PhonemeLabel value)
    {
        if (_lookup2.TryGetValue(language, out var lookup)
            && lookup.TryGetValue(label, out value)) return true;

        value = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

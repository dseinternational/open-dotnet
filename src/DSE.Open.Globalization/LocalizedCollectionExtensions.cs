// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Globalization;

public static class LocalizedCollectionExtensions
{
    /// <summary>
    /// Gets the item specified by the <paramref name="tag"/> or
    /// the nearest fallback item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="localizedCollection"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"><paramref name="tag"/> is
    /// not a valid <see cref="LanguageTag"/>.</exception>
    public static T GetLocalizedOrFallback<T>(
        this IEnumerable<KeyValuePair<string, T>> localizedCollection,
        LanguageTag tag)
        => GetLocalizedOrFallback(localizedCollection, tag, LocalizedCollectionOptions.DefaultFallbacks);

    public static T GetLocalizedOrFallback<T>(
        this Dictionary<string, T> dictionary,
        LanguageTag tag,
        IEnumerable<LanguageTag> fallbacks)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(fallbacks);

        var tagStr = tag.ToString();

        if (dictionary.Count == 0)
        {
            throw new InvalidOperationException("No localized items in collection.");
        }

        if (dictionary.TryGetValue(tagStr, out var exactMatch))
        {
            return exactMatch;
        }

        var tagLangPart = tag.GetLanguagePart();

        if (tagLangPart != tag && dictionary.TryGetValue(tagLangPart.ToString(), out var baseLangMatch))
        {
            return baseLangMatch;
        }

        var firstSameLang = dictionary
            .Where(i => FirstPartMatch(i.Key, tagLangPart))
            .Select(i => i.Key)
            .FirstOrDefault();

        if (firstSameLang is not null)
        {
            return dictionary[firstSameLang];
        }

        foreach (var fallback in fallbacks)
        {
            if (dictionary.TryGetValue(fallback.ToString(), out var fallbackMatch))
            {
                return fallbackMatch;
            }
        }

        return dictionary.First().Value;
    }

    public static T GetLocalizedOrFallback<T>(
        this IReadOnlyDictionary<string, T> dictionary,
        LanguageTag tag,
        IEnumerable<LanguageTag> fallbacks)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(fallbacks);

        var tagStr = tag.ToString();

        if (dictionary.Count == 0)
        {
            throw new InvalidOperationException("No localized items in collection.");
        }

        if (dictionary.TryGetValue(tagStr, out var exactMatch))
        {
            return exactMatch;
        }

        var tagLangPart = tag.GetLanguagePart();

        if (tagLangPart != tag && dictionary.TryGetValue(tagLangPart.ToString(), out var baseLangMatch))
        {
            return baseLangMatch;
        }

        var firstSameLang = dictionary
            .Where(i => FirstPartMatch(i.Key, tagLangPart))
            .Select(i => i.Key).FirstOrDefault();

        if (firstSameLang is not null)
        {
            return dictionary[firstSameLang];
        }

        foreach (var fallback in fallbacks)
        {
            if (dictionary.TryGetValue(fallback.ToString(), out var fallbackMatch))
            {
                return fallbackMatch;
            }
        }

        return dictionary.First().Value;
    }

    /// <summary>
    /// Gets the item specified by the <paramref name="tag"/> or
    /// the nearest fallback item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="localizedCollection"></param>
    /// <param name="tag"></param>
    /// <param name="fallbacks"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"><paramref name="tag"/> is
    /// not a valid <see cref="LanguageTag"/>.</exception>
    [SuppressMessage("Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "<Pending>")]
    public static T GetLocalizedOrFallback<T>(
        this IEnumerable<KeyValuePair<string, T>> localizedCollection,
        LanguageTag tag,
        IEnumerable<LanguageTag> fallbacks)
    {
        ArgumentNullException.ThrowIfNull(localizedCollection);
        ArgumentNullException.ThrowIfNull(fallbacks);

        var tagStr = tag.ToString();

        if (localizedCollection is Dictionary<string, T> dictionary)
        {
            return GetLocalizedOrFallback(dictionary, tag, fallbacks);
        }

        if (localizedCollection is IReadOnlyDictionary<string, T> readOnlyDictionary)
        {
            return GetLocalizedOrFallback(readOnlyDictionary, tag, fallbacks);
        }

        if (localizedCollection is not ICollection<KeyValuePair<string, T>> list)
        {
            list = localizedCollection.ToList();
        }

        if (list.Count == 0)
        {
            throw new InvalidOperationException("No localized items in collection.");
        }

        var exactMatch = list.SingleOrDefault(i => i.Key == tagStr);

        if (exactMatch.Key == tagStr)
        {
            return exactMatch.Value;
        }

        var tagLangPart = tag.GetLanguagePart();

        var baseLangMatch = list.SingleOrDefault(i => i.Key.AsSpan().SequenceEqual(tagLangPart.ToCharArray()));

        if (baseLangMatch.Key == tagStr)
        {
            return baseLangMatch.Value;
        }

        var firstSameLang = list.FirstOrDefault(i => FirstPartMatch(i.Key, tagLangPart));

        if (firstSameLang.Key == tagStr)
        {
            return firstSameLang.Value;
        }

        foreach (var fallback in fallbacks)
        {
            var fallbackMatch = list.SingleOrDefault(i => i.Key == fallback.ToString());

            if (fallbackMatch.Key == tagStr)
            {
                return fallbackMatch.Value;
            }
        }

        return localizedCollection.First().Value;
    }

    /// <summary>
    /// Gets the item specified by the <paramref name="tag"/> or
    /// the nearest fallback item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="localizedCollection"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static T GetLocalizedOrFallback<T>(
        this IEnumerable<KeyValuePair<LanguageTag, T>> localizedCollection,
        LanguageTag tag)
        => GetLocalizedOrFallback(localizedCollection, tag, LocalizedCollectionOptions.DefaultFallbacks);

    public static T GetLocalizedOrFallback<T>(
        this Dictionary<LanguageTag, T> dictionary,
        LanguageTag tag,
        IEnumerable<LanguageTag> fallbacks)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(fallbacks);

        if (dictionary.Count == 0)
        {
            throw new InvalidOperationException("No localized items in collection.");
        }

        if (dictionary.TryGetValue(tag, out var exactMatch))
        {
            return exactMatch;
        }

        var tagLangPart = tag.GetLanguagePart();

        if (tagLangPart != tag && dictionary.TryGetValue(tagLangPart, out var baseLangMatch))
        {
            return baseLangMatch;
        }

        LanguageTag? match = default;

        foreach (var k in dictionary.Keys)
        {
            if (k.LanguagePartEquals(tagLangPart))
            {
                match = k;
                break;
            }
        }

        if (match.HasValue)
        {
            return dictionary[match.Value];
        }

        foreach (var fallback in fallbacks)
        {
            if (dictionary.TryGetValue(fallback, out var fallbackMatch))
            {
                return fallbackMatch;
            }
        }

        return dictionary.First().Value;
    }

    /// <summary>
    /// Gets the item specified by the <paramref name="tag"/> or
    /// the nearest fallback item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="localizedCollection"></param>
    /// <param name="tag"></param>
    /// <param name="fallbacks"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [SuppressMessage("Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "<Pending>")]
    public static T GetLocalizedOrFallback<T>(
        this IEnumerable<KeyValuePair<LanguageTag, T>> localizedCollection,
        LanguageTag tag,
        IEnumerable<LanguageTag> fallbacks)
    {
        ArgumentNullException.ThrowIfNull(localizedCollection);
        ArgumentNullException.ThrowIfNull(fallbacks);

        if (localizedCollection is Dictionary<LanguageTag, T> dictionary)
        {
            return GetLocalizedOrFallback(dictionary, tag, fallbacks);
        }

        if (localizedCollection is IReadOnlyDictionary<LanguageTag, T> readOnlyDictionary)
        {
            return GetLocalizedOrFallback(readOnlyDictionary, tag, fallbacks);
        }

        if (localizedCollection is not ICollection<KeyValuePair<LanguageTag, T>> list)
        {
            list = localizedCollection.ToList();
        }

        if (list.Count == 0)
        {
            throw new InvalidOperationException("No localized items in collection.");
        }

        var exactMatch = list.SingleOrDefault(i => i.Key == tag);

        if (exactMatch.Key == tag)
        {
            return exactMatch.Value;
        }

        var tagLangPart = tag.GetLanguagePart();

        var baseLangMatch = list.SingleOrDefault(i => i.Key == tagLangPart);

        if (baseLangMatch.Key == tag)
        {
            return baseLangMatch.Value;
        }

        var firstSameLang = list.FirstOrDefault(i => i.Key.LanguagePartEquals(tagLangPart));

        if (firstSameLang.Key == tag)
        {
            return firstSameLang.Value;
        }

        foreach (var fallback in fallbacks)
        {
            var fallbackMatch = list.SingleOrDefault(i => i.Key == fallback);

            if (fallbackMatch.Key == tag)
            {
                return fallbackMatch.Value;
            }
        }

        return localizedCollection.First().Value;
    }

    /// <summary>
    /// Gets the item specified by the <paramref name="tag"/> or
    /// the nearest fallback item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="localizedCollection"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static T GetLocalizedOrFallback<T>(
        this IEnumerable<T> localizedCollection,
        LanguageTag tag)
        where T : ILocalized
        => GetLocalizedOrFallback(localizedCollection, tag, LocalizedCollectionOptions.DefaultFallbacks);

    /// <summary>
    /// Gets the item specified by the <paramref name="tag"/> or
    /// the nearest fallback item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="localizedCollection"></param>
    /// <param name="tag"></param>
    /// <param name="fallbacks"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T GetLocalizedOrFallback<T>(
        this IEnumerable<T> localizedCollection,
        LanguageTag tag,
        IEnumerable<LanguageTag> fallbacks)
        where T : ILocalized
    {
        ArgumentNullException.ThrowIfNull(localizedCollection);
        ArgumentNullException.ThrowIfNull(fallbacks);

        var list = localizedCollection.ToList();

        if (list.Count == 0)
        {
            throw new InvalidOperationException("No localized items in collection.");
        }

        var exactMatch = list.SingleOrDefault(i => i.Language == tag);

        if (exactMatch is not null)
        {
            return exactMatch;
        }

        var tagLangPart = tag.GetLanguagePart();

        var baseLangMatch = list.SingleOrDefault(i => i.Language == tagLangPart);

        if (baseLangMatch is not null)
        {
            return baseLangMatch;
        }

        var firstSameLang = list.FirstOrDefault(i => i.Language.LanguagePartEquals(tagLangPart));

        if (firstSameLang is not null)
        {
            return firstSameLang;
        }

        foreach (var fallback in fallbacks)
        {
            var fallbackMatch = list.SingleOrDefault(i => i.Language == fallback);

            if (fallbackMatch is not null)
            {
                return fallbackMatch;
            }
        }

        return list.First();
    }

    private static bool FirstPartMatch(ReadOnlySpan<char> tag, LanguageTag match)
    {
        var ix = tag.IndexOf('-');

        if (ix < 0)
        {
            ix = tag.Length;
        }

        var firstPart = tag[..ix];
        var matchSpan = match.GetLanguagePartSpan();

        for (var i = 0; i < firstPart.Length; i++)
        {
            if (firstPart[i] != matchSpan[i])
            {
                return false;
            }
        }

        return true;
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Resources;

namespace DSE.Open.Localization.Resources;

/// <summary>
/// A <see cref="ResourceManager"/> used only by tests: returns strings from an
/// in-memory dictionary keyed by culture, skipping all of the usual assembly-based
/// resource-file loading.
/// </summary>
internal sealed class StubResourceManager : ResourceManager
{
    private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> _byCulture;
    private readonly string _defaultCulture;

    public StubResourceManager(
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> byCulture,
        string defaultCulture = "")
    {
        _byCulture = byCulture;
        _defaultCulture = defaultCulture;
    }

    public override string? GetString(string name, CultureInfo? culture)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        culture ??= CultureInfo.CurrentUICulture;

        // Walk the parent chain like ResourceManager does, then fall back to the
        // provided default culture.
        for (var c = culture; c is not null; c = c.Parent)
        {
            if (_byCulture.TryGetValue(c.Name, out var dict) && dict.TryGetValue(name, out var value))
            {
                return value;
            }

            if (c.Equals(CultureInfo.InvariantCulture))
            {
                break;
            }
        }

        if (_byCulture.TryGetValue(_defaultCulture, out var defaultDict)
            && defaultDict.TryGetValue(name, out var defaultValue))
        {
            return defaultValue;
        }

        return null;
    }

    public override string? GetString(string name)
    {
        return GetString(name, null);
    }
}

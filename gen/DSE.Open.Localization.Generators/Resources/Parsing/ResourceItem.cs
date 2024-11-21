// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace DSE.Open.Localization.Generators.Resources.Parsing;

/// <summary>
/// Represents a an item in a resource file.
/// </summary>
public sealed record ResourceItem
{
    /// <summary>
    /// The key of the resource item.
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// The formatting holes, if any, in the resource item.
    /// </summary>
    public ReadOnlyCollection<Hole> Holes { get; set; } = new([]);

    /// <summary>
    /// The length of the format string.
    /// </summary>
    public int FormatLength { get; set; }

    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }

    /// <summary>
    /// Creates a new <see cref="ResourceItem"/>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="holes"></param>
    /// <param name="formatLength"></param>
    public static ResourceItem Create(string key, ReadOnlyCollection<Hole> holes, int formatLength)
    {
        return new ResourceItem
        {
            Key = key,
            Holes = holes,
            FormatLength = formatLength
        };
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Generators.Resources.Parsing;

/// <summary>
/// Represents a formatting hole in a resource string.
/// </summary>
public sealed record Hole
{
    /// <summary>
    /// The index of the hole in the string, including the opening brace.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// The length of this hole in the string, including the opening and closing braces.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// The name of the hole.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The type of the hole, if constrained to a type.
    /// </summary>
    public string? Type { get; set; } = null!;
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Generators.Resources.Parsing;

/// <summary>
/// Models a parsed parameter definition.
/// </summary>
public readonly record struct ParameterDefinition
{
    public ParameterDefinition(string name, string type)
    {
        Name = name;
        Type = type;
    }

    /// <summary>
    /// The parameter name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The parameter type.
    /// </summary>
    public string Type { get; }

    public override string ToString()
    {
        return $"{Type} {Name}";
    }

    public bool Equals(ParameterDefinition? other)
    {
        // Parameters cannot be distinguished by type.
        return Name == other?.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    /// <summary>
    /// Constructs a parameter definition from a hole.
    /// </summary>
    /// <param name="hole"></param>
    public static ParameterDefinition FromHole(Hole hole)
    {
        if (hole is null)
        {
            throw new ArgumentNullException(nameof(hole));
        }

        if (hole.Type is null)
        {
            return new ParameterDefinition(hole.Name, GlobalTypes.ReadOnlySpanChar);
        }

        if (!TypeConstraints.Lookup.TryGetValue(hole.Type, out var type))
        {
            type = hole.Type;
        }

        return new ParameterDefinition(hole.Name, type);
    }
}

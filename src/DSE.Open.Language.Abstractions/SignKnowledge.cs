// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using DSE.Open.Values;

namespace DSE.Open.Language;

/// <summary>
/// Indicates how someone is able to use a <see cref="Sign"/>.
/// </summary>
[EquatableValue]
public readonly partial struct SignKnowledge : IEquatableValue<SignKnowledge, CharSequence>
{
    public static int MaxSerializedCharLength => 12;

    public static bool IsValidValue(CharSequence value)
    {
        return !value.IsEmpty
            && value.Length < MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

    /// <summary>
    /// Understands or comprehends the meaning of the <see cref="Sign"/>.
    /// </summary>
    /// <remarks>
    /// When recording understanding, the meaning of the sign that is understood
    /// should also be tracked to avoid ambiguity (such as, between "(a) drink" and "(to) drink").
    /// </remarks>
    public static readonly SignKnowledge Understands = new("understands", true);

    /// <summary>
    /// Is able to imitate the <see cref="Sign"/>, given a prompt.
    /// </summary>
    public static readonly SignKnowledge Imitates = new("imitates", true);

    /// <summary>
    /// Is able to produce the <see cref="Sign"/> independently.
    /// </summary>
    public static readonly SignKnowledge Produces = new("produces", true);

    public static readonly IReadOnlySet<SignKnowledge> All = FrozenSet.ToFrozenSet(new[]
    {
        Understands,
        Imitates,
        Produces
    });

    private static readonly FrozenSet<CharSequence> s_validValues
        = FrozenSet.ToFrozenSet(All.Select(x => x._value));
}

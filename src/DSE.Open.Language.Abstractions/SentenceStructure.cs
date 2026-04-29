// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// Identifies the clausal structure of a sentence - for example, simple,
/// compound, complex or compound-complex.
/// </summary>
[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<SentenceStructure, AsciiString>))]
public readonly partial struct SentenceStructure
    : IEquatableValue<SentenceStructure, AsciiString>,
      IUtf8SpanSerializable<SentenceStructure>
{
    /// <summary>
    /// The maximum number of characters used to serialize a
    /// <see cref="SentenceStructure"/> value.
    /// </summary>
    public static int MaxSerializedCharLength => 32;

    /// <summary>
    /// The maximum number of bytes used to serialize a
    /// <see cref="SentenceStructure"/> value in UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => 32;

    /// <summary>
    /// Initializes a new <see cref="SentenceStructure"/> from a <see cref="string"/>.
    /// </summary>
    /// <param name="value">The sentence structure label.</param>
    public SentenceStructure(string value) : this((AsciiString)value)
    {
    }

    private SentenceStructure(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is a defined
    /// <see cref="SentenceStructure"/> label.
    /// </summary>
    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    /// <summary>
    /// Converts a <see cref="string"/> to a <see cref="SentenceStructure"/>.
    /// </summary>
    public static explicit operator SentenceStructure(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

    /// <summary>
    /// A sentence consisting of a single independent clause.
    /// </summary>
    public static readonly SentenceStructure Simple = new("simple", true);

    /// <summary>
    /// A sentence consisting of two or more independent clauses joined by a
    /// coordinator or equivalent punctuation.
    /// </summary>
    public static readonly SentenceStructure Compound = new("compound", true);

    /// <summary>
    /// A sentence consisting of one independent clause and at least one
    /// dependent clause.
    /// </summary>
    public static readonly SentenceStructure Complex = new("complex", true);

    /// <summary>
    /// A sentence consisting of two or more independent clauses and at least
    /// one dependent clause.
    /// </summary>
    public static readonly SentenceStructure CompoundComplex = new("compound-complex", true);

    /// <summary>
    /// The set of all defined <see cref="SentenceStructure"/> values.
    /// </summary>
    public static readonly FrozenSet<SentenceStructure> All = FrozenSet.ToFrozenSet(
    [
        Simple,
        Compound,
        Complex,
        CompoundComplex,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues =
        FrozenSet.ToFrozenSet(All.Select(x => x._value));
}

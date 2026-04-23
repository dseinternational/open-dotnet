// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// Identifies the communicative function of a sentence - its speech-act
/// category, such as a statement, question, command or exclamation.
/// </summary>
[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<SentenceFunction, AsciiString>))]
public readonly partial struct SentenceFunction
    : IEquatableValue<SentenceFunction, AsciiString>,
      IUtf8SpanSerializable<SentenceFunction>
{
    /// <summary>
    /// The maximum number of characters used to serialize a
    /// <see cref="SentenceFunction"/> value.
    /// </summary>
    public static int MaxSerializedCharLength => 32;

    /// <summary>
    /// The maximum number of bytes used to serialize a
    /// <see cref="SentenceFunction"/> value in UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => 32;

    public SentenceFunction(string value) : this((AsciiString)value)
    {
    }

    private SentenceFunction(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength
            && s_validValues.Contains(value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator SentenceFunction(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }

    /// <summary>
    /// A sentence that makes a statement - for example, "The ball is red."
    /// </summary>
    public static readonly SentenceFunction Declarative = new("declarative", true);

    /// <summary>
    /// A sentence that asks a question - for example, "Where is the ball?"
    /// </summary>
    public static readonly SentenceFunction Interrogative = new("interrogative", true);

    /// <summary>
    /// A sentence that gives a command or direction - for example, "Find the ball."
    /// </summary>
    public static readonly SentenceFunction Imperative = new("imperative", true);

    /// <summary>
    /// A sentence that expresses strong emotion - for example, "What a ball!"
    /// </summary>
    public static readonly SentenceFunction Exclamatory = new("exclamatory", true);

    /// <summary>
    /// The set of all defined <see cref="SentenceFunction"/> values.
    /// </summary>
    public static readonly FrozenSet<SentenceFunction> All = FrozenSet.ToFrozenSet(
    [
        Declarative,
        Interrogative,
        Imperative,
        Exclamatory,
    ]);

    private static readonly FrozenSet<AsciiString> s_validValues =
        FrozenSet.ToFrozenSet(All.Select(x => x._value));
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language.Annotations;

/// <summary>
/// Unconstrained POS tag (placeholder for others).
/// </summary>
[EquatableValue]
[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<PosTag, AsciiString>))]
public readonly partial struct PosTag
    : IEquatableValue<PosTag, AsciiString>,
      IUtf8SpanSerializable<PosTag>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters used to serialize a <see cref="PosTag"/>.
    /// </summary>
    public static int MaxSerializedCharLength => 16;

    /// <summary>
    /// The maximum number of bytes used to serialize a <see cref="PosTag"/> in UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => 16;

    /// <summary>
    /// Initializes a new <see cref="PosTag"/> from the specified value.
    /// </summary>
    public PosTag(AsciiString value) : this(value, false)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="PosTag"/> from the specified value.
    /// </summary>
    public PosTag(string value) : this((AsciiString)value)
    {
    }

    private PosTag(string value, bool skipValidation) : this((AsciiString)value, skipValidation)
    {
    }

    /// <summary>
    /// The number of characters in the tag.
    /// </summary>
    public int Length => _value.Length;

    /// <summary>
    /// The underlying <see cref="AsciiString"/> holding the characters of the tag.
    /// </summary>
    public AsciiString Value => _value;

    /// <summary>
    /// <see langword="true"/> if the value is a recognised
    /// <see cref="UniversalPosTag"/>.
    /// </summary>
    public bool IsValidUniversalPosTag => UniversalPosTag.IsValidValue(_value);

    /// <summary>
    /// <see langword="true"/> if the value is a recognised
    /// <see cref="TreebankPosTag"/>.
    /// </summary>
    public bool IsValidTreebankTag => TreebankPosTag.IsValidValue(_value);

    public static bool IsValidValue(AsciiString value)
    {
        return !value.IsEmpty
            && value.Length <= MaxSerializedCharLength;
    }

    /// <summary>
    /// Converts this <see cref="PosTag"/> to a <see cref="UniversalPosTag"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not a
    /// valid <see cref="UniversalPosTag"/>.</exception>
    public UniversalPosTag ToUniversalPosTag()
    {
        return new(_value);
    }

    /// <summary>
    /// Converts this <see cref="PosTag"/> to a <see cref="TreebankPosTag"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is not a
    /// valid <see cref="TreebankPosTag"/>.</exception>
    public TreebankPosTag ToTreebankPosTag()
    {
        return new(_value);
    }

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates - Parse
    public static explicit operator PosTag(string value)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        return new(value);
    }
}

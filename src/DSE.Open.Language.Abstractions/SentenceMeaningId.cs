// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.IO.Hashing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// A value used to identify a sentence meaning.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<SentenceMeaningId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct SentenceMeaningId
    : IEquatableValue<SentenceMeaningId, ulong>,
      IUtf8SpanSerializable<SentenceMeaningId>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters used to serialize a
    /// <see cref="SentenceMeaningId"/> value.
    /// </summary>
    public static int MaxSerializedCharLength => 16;

    /// <summary>
    /// The maximum number of bytes used to serialize a
    /// <see cref="SentenceMeaningId"/> value in UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => 16;

    /// <summary>
    /// Initializes a new <see cref="SentenceMeaningId"/> from the given <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    public SentenceMeaningId(ulong value) : this(value, false)
    {
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is within the
    /// range of valid <see cref="SentenceMeaningId"/> values.
    /// </summary>
    public static bool IsValidValue(ulong value)
    {
        return value is <= LanguageIds.MaxIdValue and >= LanguageIds.MinIdValue;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is within the
    /// range of valid <see cref="SentenceMeaningId"/> values.
    /// </summary>
    public static bool IsValidValue(long value)
    {
        return value is <= ((long)LanguageIds.MaxIdValue) and >= ((long)LanguageIds.MinIdValue);
    }

    /// <summary>
    /// Attempts to create a <see cref="SentenceMeaningId"/> from a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <param name="id">When this method returns, contains the resulting
    /// <see cref="SentenceMeaningId"/> if the conversion succeeded; otherwise the default value.</param>
    /// <returns><see langword="true"/> if the value was within the valid range; otherwise <see langword="false"/>.</returns>
    public static bool TryFromInt64(long value, out SentenceMeaningId id)
    {
        if (IsValidValue(value))
        {
            id = new SentenceMeaningId((ulong)value);
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    /// <summary>
    /// Creates a <see cref="SentenceMeaningId"/> from a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is
    /// outside the range of valid <see cref="SentenceMeaningId"/> values.</exception>
    public static SentenceMeaningId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new SentenceMeaningId((ulong)value);
    }

    /// <summary>
    /// Creates a <see cref="SentenceMeaningId"/> from a <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is
    /// outside the range of valid <see cref="SentenceMeaningId"/> values.</exception>
    public static SentenceMeaningId FromUInt64(ulong value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new SentenceMeaningId(value);
    }

    /// <summary>
    /// Converts a <see cref="long"/> to a <see cref="SentenceMeaningId"/>.
    /// </summary>
    public static explicit operator SentenceMeaningId(long value)
    {
        return FromInt64(value);
    }

    /// <summary>
    /// Returns the <see cref="long"/> representation of this id.
    /// </summary>
    public long ToInt64()
    {
        unchecked
        {
            return (long)_value;
        }
    }

    /// <summary>
    /// Returns the <see cref="ulong"/> representation of this id.
    /// </summary>
    public ulong ToUInt64()
    {
        return _value;
    }

    /// <summary>
    /// Converts a <see cref="SentenceMeaningId"/> to a <see cref="long"/>.
    /// </summary>
    public static implicit operator long(SentenceMeaningId value)
    {
        return value.ToInt64();
    }

    /// <summary>
    /// Returns a randomly-generated <see cref="SentenceMeaningId"/>.
    /// </summary>
    public static SentenceMeaningId GetRandomId()
    {
        return (SentenceMeaningId)LanguageIds.GetRandomIdValue();
    }

    /// <summary>
    /// A sentence meaning is a unique string (in English using Oxford Spelling) that
    /// unambiguously represents the meaning of a sentence. We can therefore use a
    /// repeatable hash value to identify it.
    /// </summary>
    /// <param name="definition">A definition of the sentence meaning, written in
    /// English using Oxford Spelling.</param>
    /// <returns>A value that uniquely identifies the definition.</returns>
    public static SentenceMeaningId FromDefinition(ReadOnlySpan<char> definition)
    {
        if (definition.IsEmpty || definition.AllAreWhiteSpace())
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(definition), "A definition must be provided.");
        }

        var length = Encoding.UTF8.GetByteCount(definition);

        byte[]? rented = null;

        try
        {
            Span<byte> buffer = length > 256
                ? (rented = ArrayPool<byte>.Shared.Rent(length))
                : stackalloc byte[length];

            var l = Encoding.UTF8.GetBytes(definition, buffer);

            return (SentenceMeaningId)(LanguageIds.MinIdValue + (ulong)(XxHash3.HashToUInt64(buffer[..l]) / (decimal)ulong.MaxValue * LanguageIds.MaxRange));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}

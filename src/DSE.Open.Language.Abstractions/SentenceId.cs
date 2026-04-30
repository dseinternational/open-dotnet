// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.IO.Hashing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Globalization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// A value used to identify a sentence.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<SentenceId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct SentenceId
    : IEquatableValue<SentenceId, ulong>,
      IUtf8SpanSerializable<SentenceId>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters used to serialize a
    /// <see cref="SentenceId"/> value.
    /// </summary>
    public static int MaxSerializedCharLength => 16;

    /// <summary>
    /// The maximum number of bytes used to serialize a
    /// <see cref="SentenceId"/> value in UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => 16;

    /// <summary>
    /// Initializes a new <see cref="SentenceId"/> from the given <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    public SentenceId(ulong value) : this(value, false)
    {
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is within the
    /// range of valid <see cref="SentenceId"/> values.
    /// </summary>
    public static bool IsValidValue(ulong value)
    {
        return value is <= LanguageIds.MaxIdValue and >= LanguageIds.MinIdValue;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is within the
    /// range of valid <see cref="SentenceId"/> values.
    /// </summary>
    public static bool IsValidValue(long value)
    {
        return value is <= ((long)LanguageIds.MaxIdValue) and >= ((long)LanguageIds.MinIdValue);
    }

    /// <summary>
    /// Attempts to create a <see cref="SentenceId"/> from a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <param name="id">When this method returns, contains the resulting
    /// <see cref="SentenceId"/> if the conversion succeeded; otherwise the default value.</param>
    /// <returns><see langword="true"/> if the value was within the valid range; otherwise <see langword="false"/>.</returns>
    public static bool TryFromInt64(long value, out SentenceId id)
    {
        if (IsValidValue(value))
        {
            id = new SentenceId((ulong)value);
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    /// <summary>
    /// Creates a <see cref="SentenceId"/> from a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is
    /// outside the range of valid <see cref="SentenceId"/> values.</exception>
    public static SentenceId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new SentenceId((ulong)value);
    }

    /// <summary>
    /// Creates a <see cref="SentenceId"/> from a <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is
    /// outside the range of valid <see cref="SentenceId"/> values.</exception>
    public static SentenceId FromUInt64(ulong value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new SentenceId(value);
    }

    /// <summary>
    /// Converts a <see cref="long"/> to a <see cref="SentenceId"/>.
    /// </summary>
    public static explicit operator SentenceId(long value)
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
    /// Converts a <see cref="SentenceId"/> to a <see cref="long"/>.
    /// </summary>
    public static implicit operator long(SentenceId value)
    {
        return value.ToInt64();
    }

    /// <summary>
    /// Returns a randomly-generated <see cref="SentenceId"/>.
    /// </summary>
    public static SentenceId GetRandomId()
    {
        return (SentenceId)LanguageIds.GetRandomIdValue();
    }

    /// <summary>
    /// A sentence is a locale/language-specific string that expresses a meaning
    /// that can be identified by a sentence meaning identifier.
    /// </summary>
    /// <param name="meaningId"></param>
    /// <param name="language"></param>
    /// <param name="sentence"></param>
    /// <returns></returns>
    public static SentenceId FromSentence(SentenceMeaningId meaningId, LanguageTag language, ReadOnlySpan<char> sentence)
    {
        if (sentence.IsEmpty || sentence.AllAreWhiteSpace())
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(sentence), "A sentence must be provided.");
        }

        var langSpan = MemoryMarshal.AsBytes(((AsciiString)language).AsSpan());

        // Legacy hash-input layout pinned to preserve compatibility with ids
        // generated by builds predating #446: [meaningId decimal text][language ASCII][sentence UTF-8].
        var length = Encoding.UTF8.GetByteCount(sentence) +
            20 // max length of UInt64
            +
            langSpan.Length;

        byte[]? rented = null;

        try
        {
            Span<byte> buffer = length > 256
                ? (rented = ArrayPool<byte>.Shared.Rent(length))
                : stackalloc byte[length];

            _ = meaningId.TryFormat(buffer, out var bytesWritten, default, CultureInfo.InvariantCulture);

            langSpan.CopyTo(buffer[bytesWritten..]);
            bytesWritten += langSpan.Length;

            bytesWritten += Encoding.UTF8.GetBytes(sentence, buffer[bytesWritten..]);

            return (SentenceId)(LanguageIds.MinIdValue + (ulong)(XxHash3.HashToUInt64(buffer[..bytesWritten]) / (decimal)ulong.MaxValue * LanguageIds.MaxRange));
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

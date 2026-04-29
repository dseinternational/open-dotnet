// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Buffers.Binary;
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
/// A value used to identify a word.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<WordId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct WordId
    : IEquatableValue<WordId, ulong>,
      IUtf8SpanSerializable<WordId>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters used to serialize a
    /// <see cref="WordId"/> value.
    /// </summary>
    public static int MaxSerializedCharLength => 16;

    /// <summary>
    /// The maximum number of bytes used to serialize a
    /// <see cref="WordId"/> value in UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => 16;

    /// <summary>
    /// Initializes a new <see cref="WordId"/> from the given <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    public WordId(ulong value) : this(value, false)
    {
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is within the
    /// range of valid <see cref="WordId"/> values.
    /// </summary>
    public static bool IsValidValue(ulong value)
    {
        return value is <= LanguageIds.MaxIdValue and >= LanguageIds.MinIdValue;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is within the
    /// range of valid <see cref="WordId"/> values.
    /// </summary>
    public static bool IsValidValue(long value)
    {
        return value is <= ((long)LanguageIds.MaxIdValue) and >= ((long)LanguageIds.MinIdValue);
    }

    /// <summary>
    /// Attempts to create a <see cref="WordId"/> from a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <param name="id">When this method returns, contains the resulting
    /// <see cref="WordId"/> if the conversion succeeded; otherwise the default value.</param>
    /// <returns><see langword="true"/> if the value was within the valid range; otherwise <see langword="false"/>.</returns>
    public static bool TryFromInt64(long value, out WordId id)
    {
        if (IsValidValue(value))
        {
            id = new WordId((ulong)value);
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    /// <summary>
    /// Creates a <see cref="WordId"/> from a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is
    /// outside the range of valid <see cref="WordId"/> values.</exception>
    public static WordId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new WordId((ulong)value);
    }

    /// <summary>
    /// Creates a <see cref="WordId"/> from a <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is
    /// outside the range of valid <see cref="WordId"/> values.</exception>
    public static WordId FromUInt64(ulong value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new WordId(value);
    }

    /// <summary>
    /// Converts a <see cref="long"/> to a <see cref="WordId"/>.
    /// </summary>
    public static explicit operator WordId(long value)
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
    /// Converts a <see cref="WordId"/> to a <see cref="long"/>.
    /// </summary>
    public static implicit operator long(WordId value)
    {
        return value.ToInt64();
    }

    /// <summary>
    /// Returns a randomly-generated <see cref="WordId"/>.
    /// </summary>
    public static WordId GetRandomId()
    {
        return (WordId)LanguageIds.GetRandomIdValue();
    }

    /// <summary>
    /// Gets an id for a word specified by the given meaning id, word and language.
    /// </summary>
    /// <param name="meaningId"></param>
    /// <param name="word"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    public static WordId FromWord(WordMeaningId meaningId, WordText word, LanguageTag language)
    {
        return FromWord(meaningId, word.Span, language);
    }

    /// <summary>
    /// Gets an id for a word specified by the given meaning id, word characters and language.
    /// </summary>
    /// <param name="meaningId">The word meaning id.</param>
    /// <param name="word">The characters of the word.</param>
    /// <param name="language">The language of the word.</param>
    /// <returns>A <see cref="WordId"/> derived from the inputs.</returns>
    public static WordId FromWord(WordMeaningId meaningId, ReadOnlySpan<char> word, LanguageTag language)
    {
        var langSpan = MemoryMarshal.AsBytes(((AsciiString)language).AsSpan());

        // Legacy hash-input layout pinned to preserve compatibility with ids
        // generated by builds predating #384/#446: [meaningId 8 bytes LE][word UTF-8][language ASCII].
        // meaningId is written little-endian deterministically (equivalent to pre-#384
        // BitConverter.GetBytes output on every realistic .NET target, which is little-endian)
        // so the same inputs produce the same id on any platform — including big-endian.
        var length = sizeof(ulong)
            + Encoding.UTF8.GetByteCount(word)
            + langSpan.Length;

        byte[]? rented = null;

        try
        {
            Span<byte> buffer = length > 256
                ? (rented = ArrayPool<byte>.Shared.Rent(length))
                : stackalloc byte[length];

            BinaryPrimitives.WriteUInt64LittleEndian(buffer, meaningId.ToUInt64());
            var bytesWritten = sizeof(ulong);

            bytesWritten += Encoding.UTF8.GetBytes(word, buffer[bytesWritten..]);

            langSpan.CopyTo(buffer[bytesWritten..]);
            bytesWritten += langSpan.Length;

            return (WordId)(LanguageIds.MinIdValue + (ulong)(XxHash3.HashToUInt64(buffer[..bytesWritten]) / (decimal)ulong.MaxValue * LanguageIds.MaxRange));
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }
    }

    /// <summary>
    /// Gets an id for a word specified by the given meaning id, UTF-8-encoded word bytes and language.
    /// </summary>
    /// <param name="meaningId">The word meaning id.</param>
    /// <param name="wordUtf8">The UTF-8-encoded bytes of the word.</param>
    /// <param name="language">The language of the word.</param>
    /// <returns>A <see cref="WordId"/> derived from the inputs.</returns>
    public static WordId FromWord(WordMeaningId meaningId, ReadOnlySpan<byte> wordUtf8, LanguageTag language)
    {
        var langSpan = MemoryMarshal.AsBytes(((AsciiString)language).AsSpan());

        // See the char overload for the layout rationale.
        var length = sizeof(ulong)
            + wordUtf8.Length
            + langSpan.Length;

        byte[]? rented = null;

        try
        {
            Span<byte> buffer = length > 256
                ? (rented = ArrayPool<byte>.Shared.Rent(length))
                : stackalloc byte[length];

            BinaryPrimitives.WriteUInt64LittleEndian(buffer, meaningId.ToUInt64());
            var bytesWritten = sizeof(ulong);

            wordUtf8.CopyTo(buffer[bytesWritten..]);
            bytesWritten += wordUtf8.Length;

            langSpan.CopyTo(buffer[bytesWritten..]);
            bytesWritten += langSpan.Length;

            return (WordId)(LanguageIds.MinIdValue + (ulong)(XxHash3.HashToUInt64(buffer[..bytesWritten]) / (decimal)ulong.MaxValue * LanguageIds.MaxRange));
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

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
/// A value used to identify a word meaning.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<WordMeaningId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct WordMeaningId
    : IEquatableValue<WordMeaningId, ulong>,
      IUtf8SpanSerializable<WordMeaningId>,
      IRepeatableHash64
{
    /// <summary>
    /// The maximum number of characters used to serialize a
    /// <see cref="WordMeaningId"/> value.
    /// </summary>
    public static int MaxSerializedCharLength => 16;

    /// <summary>
    /// The maximum number of bytes used to serialize a
    /// <see cref="WordMeaningId"/> value in UTF-8.
    /// </summary>
    public static int MaxSerializedByteLength => 16;

    /// <summary>
    /// Initializes a new <see cref="WordMeaningId"/> from the given <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    public WordMeaningId(ulong value) : this(value, false)
    {
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is within the
    /// range of valid <see cref="WordMeaningId"/> values.
    /// </summary>
    public static bool IsValidValue(ulong value)
    {
        return value is <= LanguageIds.MaxIdValue and >= LanguageIds.MinIdValue;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is within the
    /// range of valid <see cref="WordMeaningId"/> values.
    /// </summary>
    public static bool IsValidValue(long value)
    {
        return value is <= ((long)LanguageIds.MaxIdValue) and >= ((long)LanguageIds.MinIdValue);
    }

    /// <summary>
    /// Attempts to create a <see cref="WordMeaningId"/> from a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <param name="id">When this method returns, contains the resulting
    /// <see cref="WordMeaningId"/> if the conversion succeeded; otherwise the default value.</param>
    /// <returns><see langword="true"/> if the value was within the valid range; otherwise <see langword="false"/>.</returns>
    public static bool TryFromInt64(long value, out WordMeaningId id)
    {
        if (IsValidValue(value))
        {
            id = new WordMeaningId((ulong)value);
            return true;
        }
        else
        {
            id = default;
            return false;
        }
    }

    /// <summary>
    /// Creates a <see cref="WordMeaningId"/> from a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is
    /// outside the range of valid <see cref="WordMeaningId"/> values.</exception>
    public static WordMeaningId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new WordMeaningId((ulong)value);
    }

    /// <summary>
    /// Creates a <see cref="WordMeaningId"/> from a <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The id value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is
    /// outside the range of valid <see cref="WordMeaningId"/> values.</exception>
    public static WordMeaningId FromUInt64(ulong value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new WordMeaningId(value);
    }

    /// <summary>
    /// Converts a <see cref="long"/> to a <see cref="WordMeaningId"/>.
    /// </summary>
    public static explicit operator WordMeaningId(long value)
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
    /// Converts a <see cref="WordMeaningId"/> to a <see cref="long"/>.
    /// </summary>
    public static implicit operator long(WordMeaningId value)
    {
        return value.ToInt64();
    }

    /// <summary>
    /// Returns a randomly-generated <see cref="WordMeaningId"/>.
    /// </summary>
    public static WordMeaningId GetRandomId()
    {
        return (WordMeaningId)LanguageIds.GetRandomIdValue();
    }

    /// <summary>
    /// Gets an id for a word meaning specified by the given label, universal POS tag and treebank POS tag.
    /// </summary>
    /// <param name="label"></param>
    /// <param name="pos"></param>
    /// <param name="altPos"></param>
    /// <returns></returns>
    public static WordMeaningId FromWordMeaning(ReadOnlySpan<char> label, AsciiString pos, AsciiString? altPos)
    {
        if (label.IsEmpty || label.AllAreWhiteSpace())
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(label), "A label must be provided.");
        }

        var posSpan = MemoryMarshal.AsBytes(pos.AsSpan());
        var altPosSpan = altPos is null ? default : MemoryMarshal.AsBytes(altPos.Value.AsSpan());

        // Legacy hash-input layout pinned to preserve compatibility with ids
        // generated by builds predating #446: [label UTF-8][pos ASCII][altPos ASCII].
        var length = Encoding.UTF8.GetByteCount(label) + posSpan.Length + altPosSpan.Length;

        byte[]? rented = null;

        try
        {
            Span<byte> buffer = length > 256
                ? (rented = ArrayPool<byte>.Shared.Rent(length))
                : stackalloc byte[length];

            var bytesWritten = Encoding.UTF8.GetBytes(label, buffer);

            posSpan.CopyTo(buffer[bytesWritten..]);
            bytesWritten += posSpan.Length;

            altPosSpan.CopyTo(buffer[bytesWritten..]);
            bytesWritten += altPosSpan.Length;

            return (WordMeaningId)(LanguageIds.MinIdValue + (ulong)(XxHash3.HashToUInt64(buffer[..bytesWritten]) / (decimal)ulong.MaxValue * LanguageIds.MaxRange));
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

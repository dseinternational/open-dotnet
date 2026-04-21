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
    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public WordId(ulong value) : this(value, false)
    {
    }

    public static bool IsValidValue(ulong value)
    {
        return value is <= LanguageIds.MaxIdValue and >= LanguageIds.MinIdValue;
    }

    public static bool IsValidValue(long value)
    {
        return value is <= ((long)LanguageIds.MaxIdValue) and >= ((long)LanguageIds.MinIdValue);
    }

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

    public static WordId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new WordId((ulong)value);
    }

    public static explicit operator WordId(long value)
    {
        return FromInt64(value);
    }

    public long ToInt64()
    {
        unchecked
        {
            return (long)_value;
        }
    }

    public ulong ToUInt64()
    {
        return _value;
    }

    public static implicit operator long(WordId value)
    {
        return value.ToInt64();
    }

    public static WordId GetRandomId()
    {
#pragma warning disable CA5394 // Do not use insecure randomness
        return (WordId)(ulong)Random.Shared.NextInt64((long)LanguageIds.MinIdValue, (long)LanguageIds.MaxIdValue + 1);
#pragma warning restore CA5394 // Do not use insecure randomness
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

    public static WordId FromWord(WordMeaningId meaningId, ReadOnlySpan<char> word, LanguageTag language)
    {
        var langSpan = MemoryMarshal.AsBytes(((AsciiString)language).AsSpan());

        var length = Encoding.UTF8.GetByteCount(word)
            + 20 // max length of UInt64 decimal text
            + langSpan.Length;

        byte[]? rented = null;

        try
        {
            Span<byte> buffer = length > 256
                ? (rented = ArrayPool<byte>.Shared.Rent(length))
                : stackalloc byte[length];

            _ = meaningId.TryFormat(buffer, out var bytesWritten, default, CultureInfo.InvariantCulture);

            langSpan.CopyTo(buffer[bytesWritten..]);
            bytesWritten += langSpan.Length;

            bytesWritten += Encoding.UTF8.GetBytes(word, buffer[bytesWritten..]);

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

    public static WordId FromWord(WordMeaningId meaningId, ReadOnlySpan<byte> wordUtf8, LanguageTag language)
    {
        var langSpan = MemoryMarshal.AsBytes(((AsciiString)language).AsSpan());

        var length = wordUtf8.Length
            + 20 // max length of UInt64 decimal text
            + langSpan.Length;

        byte[]? rented = null;

        try
        {
            Span<byte> buffer = length > 256
                ? (rented = ArrayPool<byte>.Shared.Rent(length))
                : stackalloc byte[length];

            _ = meaningId.TryFormat(buffer, out var bytesWritten, default, CultureInfo.InvariantCulture);

            langSpan.CopyTo(buffer[bytesWritten..]);
            bytesWritten += langSpan.Length;

            wordUtf8.CopyTo(buffer[bytesWritten..]);
            bytesWritten += wordUtf8.Length;

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

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}

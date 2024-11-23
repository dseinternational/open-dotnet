// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.IO.Hashing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Globalization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// A value used to identify a word.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<SentenceId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct SentenceId
    : IEquatableValue<SentenceId, ulong>,
      IUtf8SpanSerializable<SentenceId>
{
    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public SentenceId(ulong value) : this(value, false)
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

    public static SentenceId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new SentenceId((ulong)value);
    }

    public static SentenceId FromUInt64(ulong value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new SentenceId(value);
    }

    public static explicit operator SentenceId(long value)
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

    public static implicit operator long(SentenceId value)
    {
        return value.ToInt64();
    }

#pragma warning disable CA5394 // Do not use insecure randomness
    public static SentenceId GetRandomId()
    {
        return (SentenceId)(ulong)Random.Shared.NextInt64((long)LanguageIds.MinIdValue, (long)LanguageIds.MaxIdValue);
    }
#pragma warning restore CA5394 // Do not use insecure randomness

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

            var textBytes = Encoding.UTF8.GetBytes(sentence, buffer[bytesWritten..]);

            bytesWritten += textBytes;

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
}

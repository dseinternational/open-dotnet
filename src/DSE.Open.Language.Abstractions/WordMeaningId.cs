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
/// A value used to identify a word.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<WordMeaningId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct WordMeaningId
    : IEquatableValue<WordMeaningId, ulong>,
      IUtf8SpanSerializable<WordMeaningId>,
      IRepeatableHash64
{
    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public WordMeaningId(ulong value) : this(value, false)
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

    public static WordMeaningId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new WordMeaningId((ulong)value);
    }

    public static WordMeaningId FromUInt64(ulong value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new WordMeaningId(value);
    }

    public static explicit operator WordMeaningId(long value)
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

    public static implicit operator long(WordMeaningId value)
    {
        return value.ToInt64();
    }

#pragma warning disable CA5394 // Do not use insecure randomness
    public static WordMeaningId GetRandomId()
    {
        return (WordMeaningId)(ulong)Random.Shared.NextInt64((long)LanguageIds.MinIdValue, (long)LanguageIds.MaxIdValue + 1);
    }
#pragma warning restore CA5394 // Do not use insecure randomness

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

        // +2 for 0xFF separator bytes between label/pos and pos/altPos. 0xFF is never a
        // valid byte in UTF-8 or ASCII, so it cannot appear inside any field.
        var length = Encoding.UTF8.GetByteCount(label) + posSpan.Length + altPosSpan.Length + 2;

        byte[]? rented = null;

        try
        {
            Span<byte> buffer = length > 256
                ? (rented = ArrayPool<byte>.Shared.Rent(length))
                : stackalloc byte[length];

            var bytesWritten = Encoding.UTF8.GetBytes(label, buffer);

            buffer[bytesWritten++] = 0xFF;

            posSpan.CopyTo(buffer[bytesWritten..]);
            bytesWritten += posSpan.Length;

            buffer[bytesWritten++] = 0xFF;

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

    public ulong GetRepeatableHashCode()
    {
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(_value);
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.IO.Hashing;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Language;

/// <summary>
/// A value used to identify a word.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUInt64ValueConverter<SentenceMeaningId>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct SentenceMeaningId
    : IEquatableValue<SentenceMeaningId, ulong>,
      IUtf8SpanSerializable<SentenceMeaningId>
{
    public static int MaxSerializedCharLength => 16;

    public static int MaxSerializedByteLength => 16;

    public SentenceMeaningId(ulong value) : this(value, false)
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

    public static SentenceMeaningId FromInt64(long value)
    {
        if (!IsValidValue(value))
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
        }

        return new SentenceMeaningId((ulong)value);
    }

    public static explicit operator SentenceMeaningId(long value)
    {
        return FromInt64(value);
    }

    public static long ToInt64(SentenceMeaningId value)
    {
        unchecked
        {
            return (long)value._value;
        }
    }

    public static implicit operator long(SentenceMeaningId value)
    {
        return ToInt64(value);
    }

#pragma warning disable CA5394 // Do not use insecure randomness
    public static SentenceMeaningId GetRandomId()
    {
        return (SentenceMeaningId)(ulong)Random.Shared.NextInt64((long)LanguageIds.MinIdValue, (long)LanguageIds.MaxIdValue);
    }
#pragma warning restore CA5394 // Do not use insecure randomness

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
}

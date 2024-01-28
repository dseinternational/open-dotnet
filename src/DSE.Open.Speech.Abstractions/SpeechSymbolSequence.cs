// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Diagnostics;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Speech.Serialization;

namespace DSE.Open.Speech;

/// <summary>
/// 
/// </summary>
[JsonConverter(typeof(JsonStringSpeechSymbolSequenceConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly struct SpeechSymbolSequence
    : IEquatable<SpeechSymbolSequence>,
      IEquatable<ReadOnlyMemory<SpeechSymbol>>,
      IEqualityOperators<SpeechSymbolSequence, SpeechSymbolSequence, bool>,
      ISpanFormattable,
      ISpanParsable<SpeechSymbolSequence>,
      ISpanFormatableCharCountProvider
{
    private readonly ReadOnlyMemory<SpeechSymbol> _value;

    public SpeechSymbolSequence(ReadOnlySpan<SpeechSymbol> value) : this(value.ToArray(), false)
    {
    }

    public SpeechSymbolSequence(ReadOnlyMemory<SpeechSymbol> value) : this(value, true)
    {
    }

    private SpeechSymbolSequence(ReadOnlyMemory<SpeechSymbol> value, bool copy)
    {
        _value = copy ? value.ToArray() : value;
    }

    /// <summary>
    /// Returns a <see cref="SpeechSymbolSequence"/> that points to the same memory
    /// as <paramref name="value"/>. The caller is responsible for ensuring that the
    /// memory is not modified while the <see cref="SpeechSymbolSequence"/> is in use.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static SpeechSymbolSequence CreateUnsafe(ReadOnlyMemory<SpeechSymbol> value)
    {
        return new SpeechSymbolSequence(value, false);
    }

    public static bool IsValidValue(ReadOnlySpan<char> value)
    {
        return SpeechSymbol.AllStrictIpaChars(value);
    }

    public SpeechSymbol this[int i] => _value.Span[i];

    public SpeechSymbolSequence Slice(int start, int length)
    {
        return new SpeechSymbolSequence(_value.Slice(start, length));
    }

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    public ReadOnlyMemory<SpeechSymbol> AsMemory()
    {
        return _value;
    }

    public ReadOnlySpan<SpeechSymbol> AsSpan()
    {
        return _value.Span;
    }

    public bool Equals(ReadOnlySpan<SpeechSymbol> other)
    {
        return _value.Span.SequenceEqual(other);
    }

    public bool Equals(ReadOnlyMemory<SpeechSymbol> other)
    {
        return Equals(other.Span);
    }

    public bool Equals(SpeechSymbolSequence other)
    {
        return Equals(other._value.Span);
    }

    public override bool Equals(object? obj)
    {
        return obj is SpeechSymbolSequence other && Equals(other, StringComparison.Ordinal);
    }

    public bool Equals(
        string value,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        return Equals(value.AsSpan(), comparison);
    }

    public bool Equals(
        CharSequence chars,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        return Equals(chars.AsSpan(), comparison);
    }

    public bool Equals(
        ReadOnlySpan<char> chars,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        var buffer = MemoryMarshal.Cast<SpeechSymbol, char>(_value.Span);

        if (comparison == SpeechSymbolSequenceComparison.Exact)
        {
            return buffer.SequenceEqual(chars);
        }

        if (comparison == SpeechSymbolSequenceComparison.Permissive)
        {
            return EqualsPermissive(buffer, chars);
        }

        if (comparison == SpeechSymbolSequenceComparison.ConsonantsAndVowels)
        {
            return EqualsConsonantsAndVowels(buffer, chars);
        }

        ThrowHelper.ThrowArgumentOutOfRangeException(nameof(comparison));
        return false; // unreachable
    }

    internal static bool EqualsPermissive(
        ReadOnlySpan<char> left,
        ReadOnlySpan<char> right)
    {
        throw new NotImplementedException();
    }

    internal static bool EqualsConsonantsAndVowels(
        ReadOnlySpan<char> left,
        ReadOnlySpan<char> right)
    {
        var l = 0;
        var r = 0;

        while (l < left.Length || r < right.Length)
        {
            if (l < left.Length - 1 && !SpeechSymbol.IsConsonantOrVowel(left[l]))
            {
                l++;
                if (left[l] == right[r])
                {
                    l++;
                    r++;
                }
                continue;
            }

            if (r < right.Length - 1 && !SpeechSymbol.IsConsonantOrVowel(right[r]))
            {
                r++;
                if (left[l] == right[r])
                {
                    l++;
                    r++;
                }
                continue;
            }

            if (left[l] == right[r])
            {
                l++;
                r++;
                continue;
            }

            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the <paramref name="value"/> is contained within this <see cref="SpeechSymbolSequence"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(SpeechSymbol value)
    {
        return _value.Span.Contains(value);
    }

    public bool Contains(SpeechSymbolSequence value)
    {
        return _value.Span.IndexOfAny(value.AsSpan()) > -1;
    }

    public int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return _value.Length;
    }

    public int GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return _value.Length;
    }

    public Enumerator GetEnumerator()
    {
        return new(this);
    }

#pragma warning disable CA1034 // Nested types should not be visible
    public ref struct Enumerator
#pragma warning restore CA1034 // Nested types should not be visible
    {
        private readonly SpeechSymbolSequence _span;
        private int _index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(SpeechSymbolSequence span)
        {
            _span = span;
            _index = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            var index = _index + 1;
            if (index < _span.Length)
            {
                _index = index;
                return true;
            }

            return false;
        }

        public SpeechSymbol Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _span[_index];
        }
    }

    public static SpeechSymbolSequence Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    public static SpeechSymbolSequence Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Could not parse speech symbol sequence: [{s}]");
        return default; // unreachable
    }

    public static SpeechSymbolSequence Parse(string s)
    {
        return Parse(s, default);
    }

    public static SpeechSymbolSequence ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    public static SpeechSymbolSequence Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out SpeechSymbolSequence result)
    {
        return TryParse(s, default, out result);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out SpeechSymbolSequence result)
    {
        var value = new SpeechSymbol[s.Length];

        for (var i = 0; i < s.Length; i++)
        {
            var c = s[i];

            if (!SpeechSymbol.TryCreate(c, out var symbol))
            {
                Debugger.Break();

                result = default;
                return false;
            }

            value[i] = symbol;
        }

        result = new SpeechSymbolSequence((ReadOnlyMemory<SpeechSymbol>)value);
        return true;
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out SpeechSymbolSequence result)
    {
        return TryParse(s, default, out result);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out SpeechSymbolSequence result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public override int GetHashCode()
    {
        return string.GetHashCode(MemoryMarshal.Cast<SpeechSymbol, char>(_value.Span), StringComparison.Ordinal);
    }

    public int IndexOf(SpeechSymbol c)
    {
        return _value.Span.IndexOf(c);
    }

    public int LastIndexOf(SpeechSymbol c)
    {
        return _value.Span.LastIndexOf(c);
    }

    public override string ToString()
    {
        return ToString(default, default);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        char[]? rented = null;

        Span<char> chars = MemoryThresholds.CanStackalloc<char>(_value.Length)
            ? stackalloc char[_value.Length]
            : (rented = ArrayPool<char>.Shared.Rent(_value.Length));

        try
        {
            if (TryFormat(chars, out var charsWritten, format, formatProvider))
            {
                return new string(chars[..charsWritten]);
            }

            Expect.Unreachable();
            return null!; // unreachable
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (destination.Length >= _value.Length)
        {
            for (var i = 0; i < _value.Length; i++)
            {
                destination[i] = (char)_value.Span[i];
            }

            charsWritten = _value.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public static bool operator ==(SpeechSymbolSequence left, SpeechSymbolSequence right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SpeechSymbolSequence left, SpeechSymbolSequence right)
    {
        return !(left == right);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    public static SpeechSymbolSequence operator +(SpeechSymbolSequence left, SpeechSymbolSequence right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        var combined = new SpeechSymbol[left._value.Length + right._value.Length];
        left._value.Span.CopyTo(combined.AsSpan());
        right._value.Span.CopyTo(combined.AsSpan()[left._value.Length..]);
        return new SpeechSymbolSequence((ReadOnlyMemory<SpeechSymbol>)combined, false);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    public static explicit operator SpeechSymbolSequence(string value)
    {
        return Parse(value, null);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

}

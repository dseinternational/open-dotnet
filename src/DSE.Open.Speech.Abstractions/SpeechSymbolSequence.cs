// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Speech.Serialization;

namespace DSE.Open.Speech;

/// <summary>
///
/// </summary>
[JsonConverter(typeof(JsonStringSpeechSymbolSequenceConverter))]
[StructLayout(LayoutKind.Sequential)]
[CollectionBuilder(typeof(SpeechSymbolSequence), "Create")]
public readonly struct SpeechSymbolSequence
    : IEquatable<SpeechSymbolSequence>,
      IEquatable<ReadOnlyMemory<SpeechSymbol>>,
      IEqualityOperators<SpeechSymbolSequence, SpeechSymbolSequence, bool>,
      ISpanFormattable,
      ISpanParsable<SpeechSymbolSequence>,
      ISpanFormatableCharCountProvider,
      IUtf8SpanFormattable,
      IRepeatableHash64
{
    private readonly ReadOnlyMemory<SpeechSymbol> _value;

    public SpeechSymbolSequence(IEnumerable<SpeechSymbol> value) : this(value.ToArray(), false)
    {
    }

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

    public static SpeechSymbolSequence Create(ReadOnlySpan<SpeechSymbol> value)
    {
        return new(value);
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
        return new(value, false);
    }

    public static bool IsValidValue(ReadOnlySpan<char> value)
    {
        return SpeechSymbol.AllStrictIpa(value);
    }

    public SpeechSymbol this[int i] => _value.Span[i];

    public SpeechSymbolSequence Slice(int start, int length)
    {
        return new(_value.Slice(start, length));
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

    public ReadOnlySpan<char> AsCharSpan()
    {
        return MemoryMarshal.Cast<SpeechSymbol, char>(_value.Span);
    }

    /// <summary>
    /// Returns the initial sound of this <see cref="SpeechSymbolSequence"/>.
    /// </summary>
    /// <param name="includeInitialStressMarkers"></param>
    /// <returns></returns>
    [SkipLocalsInit]
    public SpeechSound GetInitialSound(bool includeInitialStressMarkers = false)
    {
        if (_value.IsEmpty)
        {
            return SpeechSound.Empty;
        }

        var span = _value.Span;

        Span<SpeechSymbol> buffer = stackalloc SpeechSymbol[SpeechSound.MaxLength];
        SpeechSoundType? type = null;
        var i = 0;
        var v = 0;

        foreach (var s in span)
        {
            if (i >= SpeechSound.MaxLength)
            {
                ThrowHelper.ThrowInvalidOperationException("Initial speech sound is too long.");
                break;
            }

            if (type is null)
            {
                if (SpeechSymbol.IsConsonant(s))
                {
                    type = SpeechSoundType.Consonant;
                }
                else if (SpeechSymbol.IsVowel(s))
                {
                    type = SpeechSoundType.Vowel;
                    v++;
                }

                if (type is not null
                    || (includeInitialStressMarkers
                    && (s == SpeechSymbol.PrimaryStress || s == SpeechSymbol.SecondaryStress)))
                {
                    buffer[i] = s;
                    i++;
                }
            }
            // consonant or vowel followed by tie-bar
            else if (s == SpeechSymbol.TieBar || s == SpeechSymbol.TieBarBelow)
            {
                buffer[i] = s;
                i++;
            }
            else if (type == SpeechSoundType.Consonant)
            {
                // do not join consonants, except tʃ
                if (ModifiesPrecedingConsonantVowel(s)
                    || (s == 'ʃ' && buffer[i - 1] == 't'))
                {
                    buffer[i] = s;
                    i++;
                }
                else
                {
                    break;
                }
            }
            else if (type == SpeechSoundType.Vowel)
            {
                // join vowel sounds for dipthongs
                if (SpeechSymbol.IsVowel(s))
                {
                    if (v == 2)
                    {
                        break;
                    }

                    buffer[i] = s;
                    i++;
                    v++;
                }
                else if (ModifiesPrecedingConsonantVowel(s))
                {
                    buffer[i] = s;
                    i++;
                }
                else
                {
                    break;
                }
            }
        }

        return new(buffer[..i]);

        static bool ModifiesPrecedingConsonantVowel(SpeechSymbol s)
        {
            return SpeechSymbol.IsDiacritic(s)
                || s == SpeechSymbol.Long
                || s == SpeechSymbol.HalfLong
                || s == SpeechSymbol.ExtraShort
                || s == SpeechSymbol.ExtraHighTone
                || s == SpeechSymbol.HighTone
                || s == SpeechSymbol.MidTone
                || s == SpeechSymbol.LowTone
                || s == SpeechSymbol.ExtraLowTone
                || s == SpeechSymbol.TieBar
                || s == SpeechSymbol.TieBarBelow;
        }
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
        return Equals(chars.Span, comparison);
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

        if (comparison == SpeechSymbolSequenceComparison.ConsonantsAndVowels)
        {
            return EqualsConsonantsAndVowels(buffer, chars);
        }

        ThrowHelper.ThrowArgumentOutOfRangeException(nameof(comparison));
        return false; // unreachable

        static bool EqualsConsonantsAndVowels(
            ReadOnlySpan<char> buffer,
            ReadOnlySpan<char> chars)
        {
            var l = 0;
            var r = 0;

            while (l < buffer.Length || r < chars.Length)
            {
                if (l < buffer.Length - 1 && !SpeechSymbol.IsConsonantOrVowel(buffer[l]))
                {
                    l++;

                    if (buffer[l] == chars[r])
                    {
                        l++;
                        r++;
                    }

                    continue;
                }

                if (r < chars.Length - 1 && !SpeechSymbol.IsConsonantOrVowel(chars[r]))
                {
                    r++;

                    if (buffer[l] == chars[r])
                    {
                        l++;
                        r++;
                    }

                    continue;
                }

                if (buffer[l] == chars[r])
                {
                    l++;
                    r++;
                    continue;
                }

                return false;
            }

            return true;
        }
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

    public bool Contains(
        SpeechSymbolSequence symbols,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        return IndexOf(symbols, comparison) > -1;
    }

    public bool Contains(
        ReadOnlySpan<char> chars,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        return IndexOf(chars, comparison) > -1;
    }

    public bool StartsWith(SpeechSymbol value)
    {
        return !_value.Span.IsEmpty && _value.Span[0] == value;
    }

    public bool StartsWith(
        SpeechSymbolSequence chars,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        var i = IndexOf(chars, comparison);

        if (i == 0)
        {
            return true;
        }

        switch (comparison)
        {
            case SpeechSymbolSequenceComparison.Exact:
                return false;
            case SpeechSymbolSequenceComparison.ConsonantsAndVowels:
                return SpeechSymbol.NoneConsonantOrVowel(_value.Span[..i]);
            default:
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(comparison));
                return false; // unreachable
        }
    }

    public bool StartsWith(
        ReadOnlySpan<char> chars,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        var i = IndexOf(chars, comparison);

        if (i == 0)
        {
            return true;
        }

        switch (comparison)
        {
            case SpeechSymbolSequenceComparison.Exact:
                return false;
            case SpeechSymbolSequenceComparison.ConsonantsAndVowels:
                return SpeechSymbol.NoneConsonantOrVowel(_value.Span[..i]);
            default:
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(comparison));
                return false; // unreachable
        }
    }

    public int IndexOf(
        SpeechSymbolSequence symbols,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        var buffer = _value.Span;
        var chars = symbols._value.Span;

        if (comparison == SpeechSymbolSequenceComparison.Exact)
        {
            return buffer.IndexOf(chars);
        }

        if (comparison == SpeechSymbolSequenceComparison.ConsonantsAndVowels)
        {
            return IndexOfConsonantsAndVowels(buffer, chars);
        }

        ThrowHelper.ThrowArgumentOutOfRangeException(nameof(comparison));
        return -1; // unreachable

        static int IndexOfConsonantsAndVowels(
            ReadOnlySpan<SpeechSymbol> buffer,
            ReadOnlySpan<SpeechSymbol> chars)
        {
            var bi = 0; // current buffer index
            var ci = 0; // current chars index
            var i = -1; // index of first match

            while (bi < buffer.Length)
            {
                if (ci >= chars.Length)
                {
                    // we have checked all of the symbols in the
                    // sequence we are looking for
                    return i;
                }

                var b = buffer[bi];
                var c = chars[ci];

                // if either symbol is not a consonant or vowel
                // ignore and move on

                if (!SpeechSymbol.IsConsonantOrVowel(b))
                {
                    bi++;
                    continue;
                }

                if (!SpeechSymbol.IsConsonantOrVowel(c))
                {
                    ci++;
                    continue;
                }

                if (b == c)
                {
                    // we have a match
                    if (i == -1)
                    {
                        // this is the first match
                        i = bi;
                    }

                    // move on to the next symbol in the sequence
                    // we are searching for
                    ci++;
                }
                else
                {
                    // no match, reset
                    i = -1;
                    ci = 0;
                }

                bi++;
            }

            return ci >= chars.Length ? i : -1;
        }
    }

    public int IndexOf(
        ReadOnlySpan<char> chars,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        // TODO: investigate if algorithm without the filtering/copying
        // is more performant.

        var buffer = MemoryMarshal.Cast<SpeechSymbol, char>(_value.Span);

        if (comparison == SpeechSymbolSequenceComparison.Exact)
        {
            return buffer.IndexOf(chars);
        }

        if (comparison == SpeechSymbolSequenceComparison.ConsonantsAndVowels)
        {
            return IndexOfConsonantsAndVowels(buffer, chars);
        }

        ThrowHelper.ThrowArgumentOutOfRangeException(nameof(comparison));
        return -1; // unreachable

        static int IndexOfConsonantsAndVowels(
            ReadOnlySpan<char> buffer,
            ReadOnlySpan<char> chars)
        {
            var bi = 0; // current buffer index
            var ci = 0; // current chars index
            var i = -1; // index of first match

            while (bi < buffer.Length)
            {
                if (ci >= chars.Length)
                {
                    // we have checked all of the symbols in the
                    // sequence we are looking for
                    return i;
                }

                var b = buffer[bi];
                var c = chars[ci];

                // if either symbol is not a consonant or vowel
                // ignore and move on

                if (!SpeechSymbol.IsConsonantOrVowel(b))
                {
                    bi++;
                    continue;
                }

                if (!SpeechSymbol.IsConsonantOrVowel(c))
                {
                    ci++;
                    continue;
                }

                if (b == c)
                {
                    // we have a match
                    if (i == -1)
                    {
                        // this is the first match
                        i = bi;
                    }

                    // move on to the next symbol in the sequence
                    // we are searching for
                    ci++;
                }
                else
                {
                    // no match, reset
                    i = -1;
                    ci = 0;
                }

                bi++;
            }

            return ci >= chars.Length ? i : -1;
        }
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

        result = new((ReadOnlyMemory<SpeechSymbol>)value);
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
        return string.Create(_value.Length, (this, format, formatProvider), (span, state) =>
        {
            var result = state.Item1.TryFormat(span, out var charsWritten, state.format, state.formatProvider);
            Debug.Assert(charsWritten == span.Length);
            Debug.Assert(result);
        });
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var chars = SpeechValuesMarshal.AsChars(_value.Span);

        if (chars.TryCopyTo(destination))
        {
            charsWritten = chars.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var chars = SpeechValuesMarshal.AsChars(_value.Span);

        if (Encoding.UTF8.TryGetBytes(chars, utf8Destination, out bytesWritten))
        {
            return true;
        }

        bytesWritten = 0;
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
        return new((ReadOnlyMemory<SpeechSymbol>)combined, false);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    public static explicit operator SpeechSymbolSequence(string value)
    {
        return Parse(value, null);
    }

    public static implicit operator SpeechSymbolSequence(Span<SpeechSymbol> symbols)
    {
        return new(symbols);
    }

    public static implicit operator SpeechSymbolSequence(ReadOnlySpan<SpeechSymbol> symbols)
    {
        return new(symbols);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Copies speech symbols that are consonants or vowels from <paramref name="source"/>
    /// to <paramref name="destination"/>. The number of symbols written is returned.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="charsWritten">The count of symbols written into <paramref name="destination"/>.</param>
    /// <exception cref="ArgumentException"><paramref name="destination"/> is not long enough to
    /// copy all of the consonant or vowel symbols.</exception>
    public static void CopyConsonantsAndVowels(
        ReadOnlySpan<SpeechSymbol> source,
        Span<SpeechSymbol> destination,
        out int charsWritten)
    {
        var i = 0;
        var j = 0;

        while (i < source.Length)
        {
            if (SpeechSymbol.IsConsonantOrVowel(source[i]))
            {
                if (j >= destination.Length)
                {
                    ThrowHelper.ThrowArgumentException(
                        "Unable to copy consonants and vowels: destination buffer is too short.");
                    charsWritten = j; // unreachable
                    return; // unreachable
                }

                destination[j] = source[i];
                j++;
            }

            i++;
        }

        charsWritten = j;
    }

    /// <summary>
    /// Copies speech symbols that are consonants or vowels from <paramref name="source"/>
    /// to <paramref name="destination"/>. The number of characters written is returned.
    /// Only Strict IPA characters classified as consonants or vowels
    /// (see <see cref="SpeechSymbol.IsConsonantOrVowel(SpeechSymbol)"/> ) are copied.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="charsWritten">The count of characters written into <paramref name="destination"/>.</param>
    /// <exception cref="ArgumentException"><paramref name="destination"/> is not long enough to
    /// copy all of the consonant or vowel symbols.</exception>
    public static void CopyConsonantsAndVowels(
        ReadOnlySpan<char> source,
        Span<char> destination,
        out int charsWritten)
    {
        var i = 0;
        var j = 0;

        while (i < source.Length)
        {
            if (SpeechSymbol.IsConsonantOrVowel(source[i]))
            {
                if (j >= destination.Length)
                {
                    ThrowHelper.ThrowArgumentException(
                        "Unable to copy consonants and vowels: destination buffer is too short.");
                    charsWritten = j; // unreachable
                    return; // unreachable
                }

                destination[j] = source[i];
                j++;
            }

            i++;
        }

        charsWritten = j;
    }

    public ulong GetRepeatableHashCode()
    {
        var chars = MemoryMarshal.Cast<SpeechSymbol, char>(_value.Span);
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(chars);
    }
}

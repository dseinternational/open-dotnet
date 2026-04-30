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
/// An immutable ordered sequence of <see cref="SpeechSymbol"/> values — an IPA
/// transcription of one or more speech sounds. Supports comparison either exactly
/// or ignoring suprasegmental markers and diacritics via
/// <see cref="SpeechSymbolSequenceComparison.ConsonantsAndVowels"/>.
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
      ISpanFormattableCharCountProvider,
      IUtf8SpanFormattable,
      IRepeatableHash64
{
    private readonly ReadOnlyMemory<SpeechSymbol> _value;

    /// <summary>
    /// Initializes a new <see cref="SpeechSymbolSequence"/> by copying the symbols
    /// from <paramref name="value"/>.
    /// </summary>
    public SpeechSymbolSequence(IEnumerable<SpeechSymbol> value) : this(value.ToArray(), false)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="SpeechSymbolSequence"/> by copying the symbols
    /// from <paramref name="value"/>.
    /// </summary>
    public SpeechSymbolSequence(ReadOnlySpan<SpeechSymbol> value) : this(value.ToArray(), false)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="SpeechSymbolSequence"/> by copying the symbols
    /// from <paramref name="value"/>.
    /// </summary>
    public SpeechSymbolSequence(ReadOnlyMemory<SpeechSymbol> value) : this(value, true)
    {
    }

    private SpeechSymbolSequence(ReadOnlyMemory<SpeechSymbol> value, bool copy)
    {
        _value = copy ? value.ToArray() : value;
    }

    /// <summary>
    /// Creates a new <see cref="SpeechSymbolSequence"/> from the specified
    /// <paramref name="value"/>. Used as the collection builder.
    /// </summary>
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

    /// <summary>
    /// Determines whether all of the characters in <paramref name="value"/> are valid
    /// strict-IPA characters. Returns <see langword="true"/> if the span is empty.
    /// </summary>
    public static bool IsValidValue(ReadOnlySpan<char> value)
    {
        return SpeechSymbol.AllStrictIpa(value);
    }

    /// <summary>
    /// Gets the <see cref="SpeechSymbol"/> at position <paramref name="i"/>.
    /// </summary>
    public SpeechSymbol this[int i] => _value.Span[i];

    /// <summary>
    /// Returns a sub-sequence starting at the specified <paramref name="start"/> index
    /// and containing <paramref name="length"/> symbols.
    /// </summary>
    public SpeechSymbolSequence Slice(int start, int length)
    {
        return new(_value.Slice(start, length));
    }

    /// <summary>
    /// Gets a value indicating whether this sequence contains no symbols.
    /// </summary>
    public bool IsEmpty => _value.IsEmpty;

    /// <summary>
    /// Gets the number of <see cref="SpeechSymbol"/> values in this sequence.
    /// </summary>
    public int Length => _value.Length;

    /// <summary>
    /// Returns the underlying symbols as a <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    public ReadOnlyMemory<SpeechSymbol> AsMemory()
    {
        return _value;
    }

    /// <summary>
    /// Returns the underlying symbols as a <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    public ReadOnlySpan<SpeechSymbol> AsSpan()
    {
        return _value.Span;
    }

    /// <summary>
    /// Returns the underlying symbols reinterpreted as a span of <see cref="char"/> values.
    /// </summary>
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

    /// <summary>
    /// Returns a value indicating whether this sequence is equal to <paramref name="other"/>.
    /// </summary>
    public bool Equals(ReadOnlySpan<SpeechSymbol> other)
    {
        return _value.Span.SequenceEqual(other);
    }

    /// <inheritdoc/>
    public bool Equals(ReadOnlyMemory<SpeechSymbol> other)
    {
        return Equals(other.Span);
    }

    /// <inheritdoc/>
    public bool Equals(SpeechSymbolSequence other)
    {
        return Equals(other._value.Span);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is SpeechSymbolSequence other && Equals(other, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns a value indicating whether this sequence is equal to the characters of
    /// <paramref name="value"/>, using the specified <paramref name="comparison"/>.
    /// </summary>
    public bool Equals(
        string value,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        return Equals(value.AsSpan(), comparison);
    }

    /// <summary>
    /// Returns a value indicating whether this sequence is equal to the characters of
    /// <paramref name="chars"/>, using the specified <paramref name="comparison"/>.
    /// </summary>
    public bool Equals(
        CharSequence chars,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        return Equals(chars.Span, comparison);
    }

    /// <summary>
    /// Returns a value indicating whether this sequence is equal to the characters of
    /// <paramref name="chars"/>, using the specified <paramref name="comparison"/>.
    /// </summary>
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

            while (l < buffer.Length && r < chars.Length)
            {
                if (!SpeechSymbol.IsConsonantOrVowel(buffer[l]))
                {
                    l++;
                    continue;
                }

                if (!SpeechSymbol.IsConsonantOrVowel(chars[r]))
                {
                    r++;
                    continue;
                }

                if (buffer[l] != chars[r])
                {
                    return false;
                }

                l++;
                r++;
            }

            // Any remaining characters on either side must be non-consonant/vowel
            // for the comparison to still hold.

            while (l < buffer.Length)
            {
                if (SpeechSymbol.IsConsonantOrVowel(buffer[l]))
                {
                    return false;
                }
                l++;
            }

            while (r < chars.Length)
            {
                if (SpeechSymbol.IsConsonantOrVowel(chars[r]))
                {
                    return false;
                }
                r++;
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

    /// <summary>
    /// Determines whether this sequence contains <paramref name="symbols"/> using the
    /// specified <paramref name="comparison"/>.
    /// </summary>
    public bool Contains(
        SpeechSymbolSequence symbols,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        return IndexOf(symbols, comparison) > -1;
    }

    /// <summary>
    /// Determines whether this sequence contains the characters of <paramref name="chars"/>
    /// using the specified <paramref name="comparison"/>.
    /// </summary>
    public bool Contains(
        ReadOnlySpan<char> chars,
        SpeechSymbolSequenceComparison comparison = SpeechSymbolSequenceComparison.Exact)
    {
        return IndexOf(chars, comparison) > -1;
    }

    /// <summary>
    /// Determines whether this sequence begins with the specified <paramref name="value"/>.
    /// </summary>
    public bool StartsWith(SpeechSymbol value)
    {
        return !_value.Span.IsEmpty && _value.Span[0] == value;
    }

    /// <summary>
    /// Determines whether this sequence begins with <paramref name="chars"/> using the
    /// specified <paramref name="comparison"/>.
    /// </summary>
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

    /// <summary>
    /// Determines whether this sequence begins with the characters of <paramref name="chars"/>
    /// using the specified <paramref name="comparison"/>.
    /// </summary>
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

    /// <summary>
    /// Returns the zero-based index of the first occurrence of <paramref name="symbols"/>
    /// in this sequence, or -1 if not found, using the specified <paramref name="comparison"/>.
    /// </summary>
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

    /// <summary>
    /// Returns the zero-based index of the first occurrence of <paramref name="chars"/>
    /// in this sequence, or -1 if not found, using the specified <paramref name="comparison"/>.
    /// </summary>
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

    /// <inheritdoc/>
    public int GetCharCount(ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return _value.Length;
    }

    /// <inheritdoc/>
    public int GetCharCount(string? format, IFormatProvider? formatProvider)
    {
        return _value.Length;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the symbols in this sequence.
    /// </summary>
    public Enumerator GetEnumerator()
    {
        return new(this);
    }

#pragma warning disable CA1034 // Nested types should not be visible
    /// <summary>
    /// Enumerates the <see cref="SpeechSymbol"/> values of a <see cref="SpeechSymbolSequence"/>.
    /// </summary>
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

        /// <summary>
        /// Advances the enumerator to the next <see cref="SpeechSymbol"/>.
        /// </summary>
        /// <returns><see langword="true"/> if the enumerator advanced; <see langword="false"/>
        /// if the end of the sequence has been reached.</returns>
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

        /// <summary>
        /// Gets the <see cref="SpeechSymbol"/> at the current position of the enumerator.
        /// </summary>
        public SpeechSymbol Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _span[_index];
        }
    }

    /// <summary>
    /// Parses the specified character sequence into a <see cref="SpeechSymbolSequence"/>.
    /// </summary>
    public static SpeechSymbolSequence Parse(ReadOnlySpan<char> s)
    {
        return Parse(s, default);
    }

    /// <inheritdoc/>
    public static SpeechSymbolSequence Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"Could not parse speech symbol sequence: [{s}]");
        return default; // unreachable
    }

    /// <summary>
    /// Parses the specified string into a <see cref="SpeechSymbolSequence"/>.
    /// </summary>
    public static SpeechSymbolSequence Parse(string s)
    {
        return Parse(s, default);
    }

    /// <summary>
    /// Parses the specified string into a <see cref="SpeechSymbolSequence"/> using the
    /// invariant culture.
    /// </summary>
    public static SpeechSymbolSequence ParseInvariant(string s)
    {
        return Parse(s, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public static SpeechSymbolSequence Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <summary>
    /// Attempts to parse the specified character sequence into a <see cref="SpeechSymbolSequence"/>.
    /// </summary>
    public static bool TryParse(
        ReadOnlySpan<char> s,
        out SpeechSymbolSequence result)
    {
        return TryParse(s, default, out result);
    }

    /// <inheritdoc/>
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
                result = default;
                return false;
            }

            value[i] = symbol;
        }

        result = new((ReadOnlyMemory<SpeechSymbol>)value);
        return true;
    }

    /// <summary>
    /// Attempts to parse the specified string into a <see cref="SpeechSymbolSequence"/>.
    /// </summary>
    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out SpeechSymbolSequence result)
    {
        return TryParse(s, default, out result);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return string.GetHashCode(MemoryMarshal.Cast<SpeechSymbol, char>(_value.Span), StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns the zero-based index of the first occurrence of <paramref name="c"/>
    /// in this sequence, or -1 if it is not found.
    /// </summary>
    public int IndexOf(SpeechSymbol c)
    {
        return _value.Span.IndexOf(c);
    }

    /// <summary>
    /// Returns the zero-based index of the last occurrence of <paramref name="c"/>
    /// in this sequence, or -1 if it is not found.
    /// </summary>
    public int LastIndexOf(SpeechSymbol c)
    {
        return _value.Span.LastIndexOf(c);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString(default, default);
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Create(_value.Length, (this, format, formatProvider), (span, state) =>
        {
            var result = state.Item1.TryFormat(span, out var charsWritten, state.format, state.formatProvider);
            Debug.Assert(charsWritten == span.Length);
            Debug.Assert(result);
        });
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <summary>
    /// Determines whether two <see cref="SpeechSymbolSequence"/> values are equal.
    /// </summary>
    public static bool operator ==(SpeechSymbolSequence left, SpeechSymbolSequence right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two <see cref="SpeechSymbolSequence"/> values are not equal.
    /// </summary>
    public static bool operator !=(SpeechSymbolSequence left, SpeechSymbolSequence right)
    {
        return !(left == right);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates
    /// <summary>
    /// Concatenates two <see cref="SpeechSymbolSequence"/> values into a new sequence.
    /// </summary>
    public static SpeechSymbolSequence operator +(SpeechSymbolSequence left, SpeechSymbolSequence right)
#pragma warning restore CA2225 // Operator overloads have named alternates
    {
        var combined = new SpeechSymbol[left._value.Length + right._value.Length];
        left._value.Span.CopyTo(combined.AsSpan());
        right._value.Span.CopyTo(combined.AsSpan()[left._value.Length..]);
        return new((ReadOnlyMemory<SpeechSymbol>)combined, false);
    }

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    /// <summary>
    /// Converts a <see cref="string"/> to a <see cref="SpeechSymbolSequence"/> by parsing it.
    /// </summary>
    public static explicit operator SpeechSymbolSequence(string value)
    {
        return Parse(value, null);
    }

    /// <summary>
    /// Converts a <see cref="Span{T}"/> of <see cref="SpeechSymbol"/> to a
    /// <see cref="SpeechSymbolSequence"/>.
    /// </summary>
    public static implicit operator SpeechSymbolSequence(Span<SpeechSymbol> symbols)
    {
        return new(symbols);
    }

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{T}"/> of <see cref="SpeechSymbol"/> to a
    /// <see cref="SpeechSymbolSequence"/>.
    /// </summary>
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

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        var chars = MemoryMarshal.Cast<SpeechSymbol, char>(_value.Span);
        return RepeatableHash64Provider.Default.GetRepeatableHashCode(chars);
    }
}

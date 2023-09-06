// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace DSE.Open.Language;

/// <summary>
/// A sign is anything that communicates a meaning that is not the sign
/// itself to the interpreter of the sign.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly record struct Sign : ISpanFormattable, ISpanParsable<Sign>
{
    /// <summary>
    /// The modality of the sign.
    /// </summary>
    public SignModality Modality { get; }

    /// <summary>
    /// A <see cref="Word"/> that identifies the meaning of the sign.
    /// </summary>
    public Word Word { get; }

    public static Sign Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();

    public static Sign Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Sign result) => throw new NotImplementedException();

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Sign result) => throw new NotImplementedException();

    public override string ToString() => $"{Modality}::{Word}";

    public string ToString(string? format, IFormatProvider? formatProvider) => throw new NotImplementedException();

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();
}

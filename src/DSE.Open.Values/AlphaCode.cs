// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace DSE.Open.Values;

// TODO

[NominalValue]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AlphaCode : INominalValue<AlphaCode, AsciiCharSequence>
{
    static int ISpanSerializable<AlphaCode>.MaxSerializedCharLength { get; } = MaxLength;

    public const int MaxLength = 32;

    public static bool IsValidValue(AsciiCharSequence value)
        => value is { IsEmpty: false, Length: <= MaxLength }
            && value.AsSpan().ContainsOnlyAsciiLetters();
}

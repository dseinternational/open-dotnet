// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace DSE.Open.Speech;

internal static class SpeechValuesMarshal
{
    public static ReadOnlySpan<char> AsChars(ReadOnlySpan<SpeechSymbol> valueSpan)
    {
        return MemoryMarshal.Cast<SpeechSymbol, char>(valueSpan);
    }
}

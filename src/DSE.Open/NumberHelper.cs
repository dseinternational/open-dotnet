// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class NumberHelper
{
    // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Number/MAX_SAFE_INTEGER
    public const long MaxJsonSafeInteger = 9007199254740991;

    public static bool IsJsonSafeInteger(ulong value)
    {
        return value <= MaxJsonSafeInteger;
    }
}

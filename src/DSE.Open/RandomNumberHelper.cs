// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Security;

namespace DSE.Open;

public static class RandomNumberHelper
{
    [Obsolete("Use RandomValueGenerator.GetJsonSafeUInt64()")]
    public static ulong GetJsonSafeInteger()
    {
        return RandomValueGenerator.GetJsonSafeUInt64();
    }
}

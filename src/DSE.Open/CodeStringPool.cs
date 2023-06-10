// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CommunityToolkit.HighPerformance.Buffers;

namespace DSE.Open;

/// <summary>
/// A shared pool for caching strings created by 'code' types
/// </summary>
internal static class CodeStringPool
{
    public static readonly StringPool Shared = new(512);
}

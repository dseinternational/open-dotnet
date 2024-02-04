// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;

namespace DSE.Open.Memory;

public static partial class MultiMemory
{
    /// <summary>
    /// Gets the set of strides that can be used to calculate the offset of n-shape in a 1-dimensional layout
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="reverseStride"></param>
    /// <returns></returns>
    public static uint[] GetStrides(ReadOnlySpan<uint> shape, bool reverseStride = false)
    {
        var strides = new uint[shape.Length];

        if (shape.Length == 0)
        {
            return strides;
        }

        var stride = 1u;

        if (reverseStride)
        {
            for (var i = 0; i < strides.Length; i++)
            {
                strides[i] = stride;
                stride *= shape[i];
            }
        }
        else
        {
            for (var i = strides.Length - 1; i >= 0; i--)
            {
                strides[i] = stride;
                stride *= shape[i];
            }
        }

        return strides;
    }

    /// <summary>
    /// Calculates the 1-d index for n-d indices in layout specified by strides.
    /// </summary>
    /// <param name="strides"></param>
    /// <param name="indices"></param>
    /// <param name="startFromDimension"></param>
    /// <returns></returns>
    public static uint GetIndex(ReadOnlySpan<uint> strides, ReadOnlySpan<uint> indices, uint startFromDimension = 0)
    {
        Debug.Assert(strides.Length == indices.Length);

        var index = 0u;

        for (var i = startFromDimension; i < indices.Length; i++)
        {
            index += strides[(int)i] * indices[(int)i];
        }

        return index;
    }
}

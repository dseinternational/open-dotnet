// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

public static class DataFrameExtensions
{
    public static bool TryGetVector(this IReadOnlyDataFrame dataFrame, string name, [NotNullWhen(true)] out Vector? vector)
    {
        ArgumentNullException.ThrowIfNull(dataFrame);

        vector = dataFrame[name];

        if (vector is null)
        {
            return false;
        }

        return true;
    }
}

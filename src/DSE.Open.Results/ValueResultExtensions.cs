// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

public static class ValueResultExtensions
{
    public static T GetRequiredValue<T>(this ValueResult<T> result)
    {
        Guard.IsNotNull(result);

        if (!result.HasValue)
        {
            ThrowHelper.ThrowInvalidOperationException("Cannot get required value from result.");
        }

        return result.Value;
    }
}

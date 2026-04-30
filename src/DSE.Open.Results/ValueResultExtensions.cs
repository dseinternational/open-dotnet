// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

/// <summary>
/// Extension methods for <see cref="ValueResult{T}"/>.
/// </summary>
public static class ValueResultExtensions
{
    /// <summary>
    /// Returns the value of <paramref name="result"/> if available; otherwise throws
    /// <see cref="InvalidOperationException"/>.
    /// </summary>
    [Obsolete("We have added RequiredValue() to ValueResult<T>.")]
    public static T GetRequiredValue<T>(this ValueResult<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (!result.HasValue)
        {
            ThrowHelper.ThrowInvalidOperationException("Cannot get required value from result.");
        }

        return result.Value;
    }
}

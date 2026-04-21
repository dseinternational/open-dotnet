// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public interface IObservationParameter
{
    public const ulong MinIntegerId = 0u;
    public const ulong MaxIntegerId = 9007199254740991u;

    public const int MinTextLength = 1;
    public const int MaxTextLength = 16;

    MeasurementParameterType ParameterType { get; }

    /// <summary>
    /// Gets an integer to identify the parameter. The value must be greater than or equal to
    /// <see cref="MinIntegerId"/> and less than or equal to
    /// <see cref="MaxIntegerId"/>.
    /// </summary>
    /// <returns></returns>
    ulong GetIntegerId();

    /// <summary>
    /// Gets a text value to identify the parameter. The value must not be empty and must be no
    /// longer than <see cref="MaxTextLength"/> UTF-16 characters.
    /// </summary>
    /// <returns></returns>
    ReadOnlySpan<char> GetTextId();

    [DoesNotReturn]
    public static T ThrowParameterMismatchException<T>()
    {
        throw new ParameterTypeMismatchException(
            "The observation parameter does not support the requested parameter type.");
    }
}

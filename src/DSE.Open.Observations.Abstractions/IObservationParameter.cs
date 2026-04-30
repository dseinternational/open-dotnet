// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

/// <summary>
/// Represents a parameter associated with an observation, identifying the subject of the
/// observation in addition to its measure.
/// </summary>
public interface IObservationParameter
{
    /// <summary>
    /// The minimum value of an integer parameter identifier.
    /// </summary>
    const ulong MinIntegerId = 0u;

    /// <summary>
    /// The maximum value of an integer parameter identifier.
    /// </summary>
    const ulong MaxIntegerId = 9007199254740991u;

    /// <summary>
    /// The minimum length, in UTF-16 characters, of a text parameter identifier.
    /// </summary>
    const int MinTextLength = 1;

    /// <summary>
    /// The maximum length, in UTF-16 characters, of a text parameter identifier.
    /// </summary>
    const int MaxTextLength = 16;

    /// <summary>
    /// Gets the type of identifier provided by the parameter.
    /// </summary>
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

    /// <summary>
    /// Throws a <see cref="ParameterTypeMismatchException"/> indicating that the observation
    /// parameter does not support the requested parameter type.
    /// </summary>
    /// <typeparam name="T">The return type expected by the caller (never returned).</typeparam>
    [DoesNotReturn]
    static T ThrowParameterMismatchException<T>()
    {
        throw new ParameterTypeMismatchException(
            "The observation parameter does not support the requested parameter type.");
    }
}

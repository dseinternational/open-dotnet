// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// Thrown when a caller asks an <see cref="IObservationParameter"/> for a
/// parameter type that does not match the parameter's actual type.
/// </summary>
/// <remarks>
/// Inherits from <see cref="InvalidOperationException"/> because the mismatch
/// reflects the runtime state of the parameter, not the validity of the call's
/// arguments.
/// </remarks>
public class ParameterTypeMismatchException : InvalidOperationException
{
    /// <summary>Initialises a new exception with the default message.</summary>
    public ParameterTypeMismatchException()
    {
    }

    /// <summary>Initialises a new exception with the specified message.</summary>
    public ParameterTypeMismatchException(string message) : base(message)
    {
    }

    /// <summary>Initialises a new exception with a message and inner exception.</summary>
    public ParameterTypeMismatchException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

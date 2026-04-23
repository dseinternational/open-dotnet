// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

/// <summary>
/// Thrown when a caller asks an <see cref="IObservationValue"/> for a value
/// type (binary, ordinal, count, amount, ratio, or frequency) that does not
/// match the value's actual <see cref="IObservationValue.ValueType"/>.
/// </summary>
/// <remarks>
/// Inherits from <see cref="InvalidOperationException"/> because the mismatch
/// reflects the runtime state of the value, not the validity of the call's
/// arguments.
/// </remarks>
public class ValueTypeMismatchException : InvalidOperationException
{
    /// <summary>Initialises a new exception with the default message.</summary>
    public ValueTypeMismatchException()
    {
    }

    /// <summary>Initialises a new exception with the specified message.</summary>
    public ValueTypeMismatchException(string message) : base(message)
    {
    }

    /// <summary>Initialises a new exception with a message and inner exception.</summary>
    public ValueTypeMismatchException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

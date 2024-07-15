// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Units;

/// <summary>The exception that is thrown when ...</summary>
public class UnitOfMeasureMismatchException : InvalidOperationException
{

    /// <summary>Initialises a new instance of the <see cref="UnitOfMeasureMismatchException" />
    ///     class.</summary>
    public UnitOfMeasureMismatchException() { }

    /// <summary>Initialises a new instance of the <see cref="UnitOfMeasureMismatchException" />
    ///     class with a specified error message.</summary>
    /// <param name="message">The message that describes the error.</param>
    public UnitOfMeasureMismatchException(string message) : base(message) { }

    /// <summary>
    ///     Initialises a new instance of the <see cref="UnitOfMeasureMismatchException" />
    ///     class with a specified error message and a reference to the inner exception that is
    ///     the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception. If
    ///     the innerException parameter is not a null reference, the current exception is raised
    ///     in a catch block that handles the inner exception.
    /// </param>
    public UnitOfMeasureMismatchException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    ///     Initialises a new instance of the <see cref="UnitOfMeasureMismatchException" />
    ///     class with a specified error message and a reference to the inner exception that is
    ///     the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="dataSource">A description identifying the source of the error.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception. If
    ///     the innerException parameter is not a null reference, the current exception is raised
    ///     in a catch block that handles the inner exception.
    /// </param>
    public UnitOfMeasureMismatchException(string message, string dataSource, Exception innerException)
        : base(message, innerException) { }
}

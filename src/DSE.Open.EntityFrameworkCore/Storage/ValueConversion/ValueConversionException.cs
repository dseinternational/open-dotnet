// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// The exception thrown when a value cannot be converted between its model and store representations.
/// </summary>
public sealed class ValueConversionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueConversionException"/> class.
    /// </summary>
    public ValueConversionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueConversionException"/> class with the specified message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public ValueConversionException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueConversionException"/> class with the specified
    /// message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception, if any.</param>
    public ValueConversionException(string message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueConversionException"/> class with the specified
    /// message, the value that could not be converted, and the inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="value">The value that could not be converted.</param>
    /// <param name="innerException">The inner exception, if any.</param>
    public ValueConversionException(string message, object value, Exception? innerException) : base(message, innerException)
    {
        Value = value;
    }

    /// <summary>
    /// Gets or sets the value that could not be converted, if available.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Throws a <see cref="ValueConversionException"/> with the specified message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(string message)
    {
        throw new ValueConversionException(message);
    }

    /// <summary>
    /// Throws a <see cref="ValueConversionException"/> with the specified message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception, if any.</param>
    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(string message, Exception? innerException)
    {
        throw new ValueConversionException(message, innerException);
    }

    /// <summary>
    /// Throws a <see cref="ValueConversionException"/> with the specified message, the value that could
    /// not be converted, and the inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="value">The value that could not be converted.</param>
    /// <param name="innerException">The inner exception, if any.</param>
    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(string message, object value, Exception? innerException)
    {
        throw new ValueConversionException(message, value, innerException);
    }
}

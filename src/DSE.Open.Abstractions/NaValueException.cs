// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// An exception that is throw when attempting to access a value that is unknown or is missing.
/// </summary>
/// <remarks>
/// <para>Examples include attempting to access the value of an <see cref="INaValue"/>
/// where <see cref="INaValue.HasValue"/> is <see langword="false"/>, or casting a
/// <see cref="Trilean"/> to a <see cref="bool"/> when <see cref="Trilean.IsNa"/> is
/// <see langword="true"/>.</para>
/// </remarks>
public class NaValueException : InvalidOperationException
{
    private const string DefaultMessage = "Cannot access value as the value is unknown.";

    /// <summary>
    /// Initializes a new instance of the <see cref="NaValueException"/> class with a default message.
    /// </summary>
    public NaValueException() : base(DefaultMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NaValueException"/> class with the specified message,
    /// or a default message when <paramref name="message"/> is <see langword="null"/>.
    /// </summary>
    public NaValueException(string? message) : base(message ?? DefaultMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NaValueException"/> class with the specified
    /// message and inner exception. A default message is used when <paramref name="message"/> is
    /// <see langword="null"/>.
    /// </summary>
    public NaValueException(string? message, Exception? innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }

    /// <summary>
    /// Throws a <see cref="NaValueException"/> if <paramref name="value"/> does not have a value.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="NaValueException"><paramref name="value"/> has no value.</exception>
    public static void ThrowIfNa(INaValue value, string? message = null)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!value.HasValue)
        {
            throw new NaValueException(message);
        }
    }

    /// <summary>
    /// Throws a <see cref="NaValueException"/> if <paramref name="value"/> is <see cref="Trilean.Na"/>.
    /// </summary>
    /// <exception cref="NaValueException"><paramref name="value"/> is <see cref="Trilean.Na"/>.</exception>
    public static void ThrowIfUnknown(Trilean value, string? message = null)
    {
        if (value.IsNa)
        {
            throw new NaValueException(message);
        }
    }
}

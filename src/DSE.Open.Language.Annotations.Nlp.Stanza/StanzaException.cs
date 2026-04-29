// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

/// <summary>
/// The exception thrown for errors raised by the Stanza integration.
/// </summary>
public sealed class StanzaException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StanzaException"/> class with the specified message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public StanzaException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StanzaException"/> class with the specified message and inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public StanzaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

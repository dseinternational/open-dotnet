// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public sealed class StanzaException : Exception
{
    public StanzaException(string message) : base(message)
    {
    }

    public StanzaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

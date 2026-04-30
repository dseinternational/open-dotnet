// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using DSE.Open.Interop.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

/// <summary>
/// Base class for objects that wrap a Stanza Python object and hold a reference to the underlying Stanza service.
/// </summary>
public abstract class StanzaObject : PyObjectWrapper
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StanzaObject"/> class.
    /// </summary>
    /// <param name="pyObject">The wrapped Python object.</param>
    /// <param name="stanza">The Stanza service associated with the object.</param>
    protected internal StanzaObject(PyObject pyObject, IStanzaService stanza) : base(pyObject)
    {
        Debug.Assert(stanza is not null);
        Stanza = stanza;
    }

    /// <summary>
    /// Gets the Stanza service associated with this object.
    /// </summary>
    protected IStanzaService Stanza { get; }
}

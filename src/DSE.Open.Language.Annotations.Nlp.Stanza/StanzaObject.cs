// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using DSE.Open.Interop.Python;

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public abstract class StanzaObject : PyObjectWrapper
{
    protected internal StanzaObject(PyObject pyObject, IStanzaService stanza) : base(pyObject)
    {
        Debug.Assert(stanza is not null);
        Stanza = stanza;
    }

    protected IStanzaService Stanza { get; }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using CSnakes.Runtime.Python;

namespace DSE.Open.Interop.Python;

public abstract class PyObjectWrapper
{
    protected internal PyObjectWrapper(PyObject pyObject)
    {
        Debug.Assert(pyObject is not null);
        InnerObject = pyObject;
    }

    protected PyObject InnerObject { get; }
}

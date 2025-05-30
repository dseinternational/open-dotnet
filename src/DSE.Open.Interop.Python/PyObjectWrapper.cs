// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using CSnakes.Runtime.Python;

namespace DSE.Open.Interop.Python;

public abstract class PyObjectWrapper : IDisposable
{
    private bool _isDisposed;

    protected internal PyObjectWrapper(PyObject pyObject)
    {
        Debug.Assert(pyObject is not null);
        InnerObject = pyObject;
    }

    protected PyObject InnerObject { get; }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                InnerObject?.Dispose();
            }

            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

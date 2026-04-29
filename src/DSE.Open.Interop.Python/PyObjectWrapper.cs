// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using CSnakes.Runtime.Python;

namespace DSE.Open.Interop.Python;

/// <summary>
/// Base class for managed wrappers around a <see cref="PyObject"/> that owns the lifetime of
/// the underlying Python reference.
/// </summary>
public abstract class PyObjectWrapper : IDisposable
{
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance that takes ownership of the supplied Python object.
    /// </summary>
    protected internal PyObjectWrapper(PyObject pyObject)
    {
        Debug.Assert(pyObject is not null);
        InnerObject = pyObject;
    }

    /// <summary>
    /// Gets the wrapped Python object.
    /// </summary>
    protected PyObject InnerObject { get; }

    /// <summary>
    /// Releases the resources used by the wrapper, disposing the underlying
    /// <see cref="PyObject"/> when <paramref name="disposing"/> is <see langword="true"/>.
    /// </summary>
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

    /// <inheritdoc />
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

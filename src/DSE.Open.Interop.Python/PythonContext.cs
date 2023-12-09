// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Python.Runtime;

namespace DSE.Open.Interop.Python;

public sealed class PythonContext : IDisposable
{
    private readonly nint _threadState;
    private bool _disposed;

    public PythonContext(PythonContextConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        Runtime.PythonDLL = configuration.PythonDLL;

        PythonEngine.Initialize();

        _threadState = PythonEngine.BeginAllowThreads();
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                PythonEngine.EndAllowThreads(_threadState);

                // https://github.com/pythonnet/pythonnet/issues/2282
                //PythonEngine.Shutdown();
            }

            _disposed = true;
        }
    }

    ~PythonContext()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Python.Runtime;

namespace DSE.Open.Interop.Python;

public sealed class PythonContext : IDisposable
{
    private static PythonContext? s_current;
    private static readonly object s_currentLock = new();

    private readonly nint _threadState;

    public PythonContext(PythonContextConfiguration configuration)
    {
        Guard.IsNotNull(configuration);

        EnsureNotInitialized();

        lock (s_currentLock)
        {
            EnsureNotInitialized();

            global::Python.Runtime.Runtime.PythonDLL = configuration.PythonDLL;

            PythonEngine.Initialize();

            _threadState = PythonEngine.BeginAllowThreads();

            s_current = this;
        }
    }

    private static void EnsureNotInitialized()
    {
        if (s_current is not null)
        {
            throw new InvalidOperationException($"A {nameof(PythonContext)} is already initialized. " +
                $"Only one {nameof(PythonContext)} may be initialized at once. " +
                $"Dispose of the other instance before initializing an new instance.");
        }
    }

    public bool Disposed { get; private set; }

    private void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            if (disposing)
            {
                using (Py.GIL())
                {
                    _ = global::Python.Runtime.Runtime.TryCollectingGarbage(3);
                }

                PythonEngine.EndAllowThreads(_threadState);

                // https://github.com/pythonnet/pythonnet/issues/2282
                //PythonEngine.Shutdown();
            }

            Disposed = true;
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

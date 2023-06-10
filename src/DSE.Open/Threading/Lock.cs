// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

// See https://blogs.msdn.microsoft.com/pfxteam/2012/02/12/building-async-coordination-primitives-part-6-asynclock/
// and http://www.hanselman.com/blog/ComparingTwoTechniquesInNETAsynchronousCoordinationPrimitives.aspx

namespace DSE.Open.Threading;

/// <summary>
/// Enables a lightweight async lock around a region of code using a using(await Lock.AcquireAsync()){} construct.
/// </summary>
public sealed class Lock : IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly Task<IDisposable?> _releaser;

    public Lock()
    {
        _releaser = Task.FromResult((IDisposable?)new Releaser(this));
    }

    public bool IsAcquired => _semaphore.CurrentCount == 0;

    public void Acquire() => AcquireAsync().Wait();

    public Task<IDisposable?> AcquireAsync()
    {
        var wait = _semaphore.WaitAsync();

#pragma warning disable CA1849 // Call async methods when in an async method
        return wait.IsCompleted
            ? _releaser
            : wait.ContinueWith((_, state) => (IDisposable?)state, _releaser.Result,
                CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
#pragma warning restore CA1849 // Call async methods when in an async method
    }

    private sealed class Releaser : IDisposable
    {
        private readonly Lock _toRelease;

        internal Releaser(Lock toRelease) { _toRelease = toRelease; }

        public void Dispose() => _ = _toRelease._semaphore.Release();
    }

    public void Dispose() => _semaphore.Dispose();
}

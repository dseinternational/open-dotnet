// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DSE.Open.EntityFrameworkCore.Diagnostics;

/// <summary>
/// A <see cref="SaveChangesInterceptor"/> that prevents <c>SaveChanges</c> operations
/// from executing by throwing <see cref="UpdateInReadOnlyContextException"/>.
/// </summary>
public class ReadOnlySaveChangesInterceptor : SaveChangesInterceptor
{
    /// <inheritdoc/>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateInReadOnlyContextException.Throw();

        return default; // unreachable
    }

    /// <inheritdoc/>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateInReadOnlyContextException.Throw();

        return default; // unreachable
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Transactions;
using DSE.Open.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Polly.Contrib.WaitAndRetry;

namespace DSE.Open.EntityFrameworkCore.SqlServer;

/// <summary>
///     [Experimental] Implements a <see cref="SqlServerRetryingExecutionStrategy"/> that does
///     not buffer results, yet retries on connection failures.
/// </summary>
/// <remarks>
///     <para>
///         By default, EF buffers results when an execution strategy retries on failures. This
///         increases memory load where results would otherwise be streamed. For resultsets with
///         many rows, or very large rows, this can substantially increase memory load.
///     </para>
///     <para>
///         This <see cref="SqlServerRetryingExecutionStrategy" /> implementation disables
///         buffering. A consequence of this is that it may not be safe to retry in the middle of
///         reading results - for example, if the results are being iterated over and written into
///         a response stream.
///     </para>
///     <para>
///         We therefore limit retries to connection-related failures - not failures that occur
///         during command execution.
///     </para>
///     <list type="bullet">
///         <item><see href="https://github.com/dotnet/efcore/issues/30112"/></item>
///     </list>
/// </remarks>
public sealed partial class SqlServerNonBufferingRetryingExecutionStrategy : ExecutionStrategy
{
    // https://learn.microsoft.com/en-us/azure/azure-sql/database/troubleshoot-common-connectivity-issues?view=azuresql#interval-increase-between-retries

    // 5 + 1 + 2 + 4 + 8 + 16 = 36 seconds (+ actual time retrying)

    /// <summary>
    /// The default maximum number of retry attempts.
    /// </summary>
    public static new readonly int DefaultMaxRetryCount = 6;

    /// <summary>
    /// The default delay before the first retry attempt.
    /// </summary>
    public static readonly TimeSpan DefaultFirstRetryDelay = TimeSpan.FromMilliseconds(5000);

    /// <summary>
    /// The default median delay used to compute subsequent retry delays.
    /// </summary>
    public static readonly TimeSpan DefaultMedianRetryDelay = TimeSpan.FromMilliseconds(1000);

    private readonly TimeSpan[] _retryDelays;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlServerNonBufferingRetryingExecutionStrategy"/>
    /// class with default retry settings.
    /// </summary>
    /// <param name="context">The context on which the operations will be invoked.</param>
    public SqlServerNonBufferingRetryingExecutionStrategy(DbContext context)
        : this(context, DefaultMaxRetryCount, DefaultMedianRetryDelay)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlServerNonBufferingRetryingExecutionStrategy"/> class.
    /// </summary>
    /// <param name="context">The context on which the operations will be invoked.</param>
    /// <param name="maxRetryCount">The maximum number of retry attempts.</param>
    /// <param name="medianRetryDelay">The median delay used to compute subsequent retry delays.</param>
    public SqlServerNonBufferingRetryingExecutionStrategy(DbContext context, int maxRetryCount, TimeSpan medianRetryDelay)
        : base(context ?? throw new ArgumentNullException(nameof(context)), maxRetryCount, DefaultMaxDelay)
    {
        ArgumentNullException.ThrowIfNull(context);
        _logger = context.GetService<ILoggerFactory>().CreateLogger<SqlServerNonBufferingRetryingExecutionStrategy>();
        _retryDelays = GetRetryDelays(maxRetryCount, DefaultFirstRetryDelay, medianRetryDelay);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlServerNonBufferingRetryingExecutionStrategy"/>
    /// class with default retry settings.
    /// </summary>
    /// <param name="dependencies">Parameter object containing service dependencies.</param>
    public SqlServerNonBufferingRetryingExecutionStrategy(ExecutionStrategyDependencies dependencies)
        : this(dependencies, DefaultMaxRetryCount, DefaultMedianRetryDelay)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlServerNonBufferingRetryingExecutionStrategy"/> class.
    /// </summary>
    /// <param name="dependencies">Parameter object containing service dependencies.</param>
    /// <param name="maxRetryCount">The maximum number of retry attempts.</param>
    /// <param name="medianRetryDelay">The median delay used to compute subsequent retry delays.</param>
    public SqlServerNonBufferingRetryingExecutionStrategy(
        ExecutionStrategyDependencies dependencies,
        int maxRetryCount,
        TimeSpan medianRetryDelay)
        : base(dependencies ?? throw new ArgumentNullException(nameof(dependencies)), maxRetryCount, DefaultMaxDelay)
    {
        _logger = dependencies.CurrentContext.Context.GetService<ILoggerFactory>().CreateLogger<SqlServerNonBufferingRetryingExecutionStrategy>();
        _retryDelays = GetRetryDelays(maxRetryCount, DefaultFirstRetryDelay, medianRetryDelay);
    }

    // Produces a sequence of delays with a first delay close to that specified, followed by
    // a sequence of delays where the medians of the timing of subsequent retries will be found
    // to fall close to a pattern of 2x, 4x, 8x etc the median of the initial retry delay medianRetryDelay.
    //
    // In other words, special cases first retry delay.

    private static TimeSpan[] GetRetryDelays(int maxRetryCount, TimeSpan firstRetryDelay, TimeSpan medianRetryDelay)
    {
        if (maxRetryCount == 0)
        {
            return [];
        }

        var delays = Backoff.DecorrelatedJitterBackoffV2(firstRetryDelay, maxRetryCount).ToArray();

        if (maxRetryCount > 1)
        {
            var subsequent = Backoff.DecorrelatedJitterBackoffV2(medianRetryDelay, maxRetryCount - 1).ToArray();
            Array.Copy(subsequent, 0, delays, 1, subsequent.Length);
        }

        return delays;
    }

    /// <summary>
    /// Gets or sets a value indicating whether buffering is enabled. When <see langword="false"/>,
    /// <see cref="RetriesOnFailure"/> reports <see langword="false"/> in order to disable EF's
    /// buffering of results during retrying execution.
    /// </summary>
    public bool IsBuffering { get; set; }

    /// <summary>
    /// [Hack] Overridden to return <see langword="false"/> when <see cref="IsBuffering"/>
    /// returns <see langword="false"/> to disabled buffering. Does not indicate of retries
    /// are really not possible - see <see cref="RetriesOnFailureForReal"/>.
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/dotnet/efcore/issues/30112"/>
    /// </remarks>
    public override bool RetriesOnFailure => IsBuffering;

    /// <summary>
    /// Indicates whether this <see cref="IExecutionStrategy" /> might retry the execution after a failure.
    /// </summary>
    public bool RetriesOnFailureForReal
    {
        get
        {
            var current = Current;
            return (current == null || current == this) && MaxRetryCount > 0;
        }
    }

    /// <inheritdoc/>
    protected override TimeSpan? GetNextDelay(Exception lastException)
    {
        var currentRetryCount = ExceptionsEncountered.Count - 1;
        return currentRetryCount < MaxRetryCount ? _retryDelays[currentRetryCount] : null;
    }

    /// <inheritdoc/>
    protected override void OnFirstExecution()
    {
        if (RetriesOnFailureForReal
            && (Dependencies.CurrentContext.Context.Database.CurrentTransaction is not null
                || Dependencies.CurrentContext.Context.Database.GetEnlistedTransaction() is not null
                || (((IDatabaseFacadeDependenciesAccessor)Dependencies.CurrentContext.Context.Database).Dependencies
                    .TransactionManager as
                    ITransactionEnlistmentManager)?.CurrentAmbientTransaction is not null))
        {
            throw new InvalidOperationException(
                CoreStrings.ExecutionStrategyExistingTransaction(
                    GetType().Name,
                    nameof(DbContext)
                    + "."
                    + nameof(DbContext.Database)
                    + "."
                    + nameof(DatabaseFacade.CreateExecutionStrategy)
                    + "()"));
        }

        ExceptionsEncountered.Clear();
    }

    /// <inheritdoc/>
    protected override bool ShouldRetryOn(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var shouldRetry = SqlServerTransientConnectionExceptionDetector.ShouldRetryOn(exception);

        if (shouldRetry)
        {
            Log.Retry(_logger, exception.Message, exception);
        }
        else
        {
            Log.Failure(_logger, exception.Message, exception);
        }

        return shouldRetry;
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Warning,
            Message = "Retrying operation following exception: '{message}'.")]
        public static partial void Retry(ILogger logger, string message, Exception exception);

        [LoggerMessage(
            EventId = 2,
            Level = LogLevel.Warning,
            Message = "Unable to retry operation following exception: '{message}'.")]
        public static partial void Failure(ILogger logger, string message, Exception exception);
    }
}

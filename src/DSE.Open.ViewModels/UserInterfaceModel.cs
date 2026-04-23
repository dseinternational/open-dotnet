// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace DSE.Open.ViewModels;

/// <summary>
/// A base <see cref="ObservableObject"/> implementation of <see cref="IUserInterfaceModel"/>
/// that tracks busy / read-only / initialised state and exposes formatting and presentation
/// cultures. Intended to be subclassed by concrete view models.
/// </summary>
public abstract partial class UserInterfaceModel : ObservableObject, IUserInterfaceModel
{
    private bool _isBusy;
    private bool _isReadOnly;
    private bool _isInitialized;
    private CultureInfo? _formatCulture;
    private CultureInfo? _presentationCulture;

    /// <summary>Initialises a new <see cref="UserInterfaceModel"/>.</summary>
    protected UserInterfaceModel()
    {
    }

    /// <summary>
    /// Gets or sets the culture used for formatting and parsing text output/input.
    /// Defaults to <see cref="CultureInfo.CurrentCulture"/> on first access.
    /// </summary>
    /// <exception cref="ArgumentNullException">Setter value is <see langword="null"/>.</exception>
    public CultureInfo FormatCulture
    {
        get => _formatCulture ??= CultureInfo.CurrentCulture;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _ = SetProperty(ref _formatCulture, value);
        }
    }

    /// <summary>
    /// Gets or sets the culture used for localising text and speech presented within the
    /// user interface. Defaults to <see cref="CultureInfo.CurrentUICulture"/> on first access.
    /// </summary>
    /// <exception cref="ArgumentNullException">Setter value is <see langword="null"/>.</exception>
    public CultureInfo PresentationCulture
    {
        get => _presentationCulture ??= CultureInfo.CurrentUICulture;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _ = SetProperty(ref _presentationCulture, value);
        }
    }

    /// <inheritdoc />
    public virtual bool IsBusy
    {
        get => _isBusy;
        protected set => SetProperty(ref _isBusy, value, nameof(IsBusy));
    }

    /// <inheritdoc />
    public virtual bool IsReadOnly
    {
        get => _isReadOnly;
        protected set => SetProperty(ref _isReadOnly, value, nameof(IsReadOnly));
    }

    /// <inheritdoc />
    public virtual bool IsInitialized
    {
        get => _isInitialized;
        protected set => SetProperty(ref _isInitialized, value, nameof(IsInitialized));
    }

    /// <inheritdoc />
    /// <remarks>
    /// Not thread-safe: concurrent callers may both pass the <see cref="IsInitialized"/>
    /// check and invoke <see cref="InitializeCoreAsync"/> more than once. Callers should
    /// invoke <see cref="InitializeAsync"/> from a single thread (typically the UI thread).
    /// </remarks>
    public async ValueTask InitializeAsync(object? state = null, CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            throw new InvalidOperationException("Already initialized.");
        }

        await InitializeCoreAsync(state, cancellationToken).ConfigureAwait(false);
        IsInitialized = true;
    }

    /// <summary>
    /// Subclass initialisation hook called by <see cref="InitializeAsync"/> on each
    /// initialisation attempt. Throw from this method to abort initialisation —
    /// <see cref="IsInitialized"/> will remain <see langword="false"/> and a subsequent
    /// call to <see cref="InitializeAsync"/> may retry.
    /// </summary>
    /// <param name="state">The state value passed to <see cref="InitializeAsync"/>.</param>
    /// <param name="cancellationToken">Propagates notification that the operation should
    /// be cancelled.</param>
    protected virtual ValueTask InitializeCoreAsync(object? state, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace DSE.Open.ViewModels;

public abstract partial class UserInterfaceModel : ObservableObject, IUserInterfaceModel
{
    private bool _isBusy;
    private bool _isReadOnly;
    private bool _isInitialized;
    [ObservableProperty] private CultureInfo _formatCulture = CultureInfo.CurrentCulture;
    [ObservableProperty] private CultureInfo _presentationCulture = CultureInfo.CurrentUICulture;

    protected UserInterfaceModel()
    {
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
    /// Called once to initialise the view model.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual ValueTask InitializeCoreAsync(object? state, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
}

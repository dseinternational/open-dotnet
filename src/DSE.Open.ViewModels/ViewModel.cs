// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;

namespace DSE.Open.ViewModels;

public abstract partial class ViewModel : ObservableObject, IViewModel
{
    private bool _isBusy;
    private bool _isReadOnly;
    private bool _isInitialized;
    [ObservableProperty] private CultureInfo _viewCulture = CultureInfo.CurrentCulture;
    [ObservableProperty] private CultureInfo _viewUiCulture = CultureInfo.CurrentUICulture;

    protected ViewModel()
    {
        ViewName = GetType().Name;
    }

    public virtual bool IsBusy
    {
        get => _isBusy;
        protected set => SetProperty(ref _isBusy, value, nameof(IsBusy));
    }

    public virtual bool IsReadOnly
    {
        get => _isReadOnly;
        protected set => SetProperty(ref _isReadOnly, value, nameof(IsReadOnly));
    }

    public virtual bool IsInitialized
    {
        get => _isInitialized;
        protected set => SetProperty(ref _isInitialized, value, nameof(IsInitialized));
    }

    public virtual string ViewName { get; }

    /// <summary>
    /// Initializes the view model.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
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
    /// Initializes the view model.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual ValueTask InitializeCoreAsync(object? state, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
}

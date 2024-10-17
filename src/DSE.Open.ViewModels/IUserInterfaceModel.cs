// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;

namespace DSE.Open.ViewModels;

/// <summary>
/// Provides data and behaviours for a user interface.
/// </summary>
public interface IUserInterfaceModel : INotifyPropertyChanged, INotifyPropertyChanging
{
    /// <summary>
    /// Gets a value indicating whether the model is busy with background tasks and cannot
    /// currently respond to user input.
    /// </summary>
    bool IsBusy { get; }

    /// <summary>
    /// Gets a value indicating whether the model has been initialised.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Gets a value indicating whether the model is read-only and therefore data cannot
    /// be modified. Even if read-only, the model may permit interactions that are not data-modifying.
    /// </summary>
    bool IsReadOnly { get; }

    /// <summary>
    /// Gets or sets the culture used for formatting and parsing text output/input.
    /// </summary>
    CultureInfo FormatCulture { get; set; }

    /// <summary>
    /// Gets or sets the culture used for localizing text and speech presented withing the user interface.
    /// </summary>
    CultureInfo PresentationCulture { get; set; }

    /// <summary>
    /// Initialises the user interface model.
    /// </summary>
    /// <param name="state">An object that can be used to provide data/configuration for initialisation.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">The model has already been initialised.</exception>
    ValueTask InitializeAsync(object? state = null, CancellationToken cancellationToken = default);
}

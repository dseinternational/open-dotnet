// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Serialization;

/// <summary>
/// An object that can have additional data attached via an <see cref="ExtensionData"/> dictionary.
/// </summary>
[Obsolete("Removing")]
public interface IExtensionData
{
    /// <summary>
    /// Gets a value indicating whether any extension data is present.
    /// </summary>
    bool HasExtensionData { get; }

    /// <summary>
    /// Gets the dictionary of additional data attached to the object, keyed by name.
    /// </summary>
    IDictionary<string, object> ExtensionData { get; }
}

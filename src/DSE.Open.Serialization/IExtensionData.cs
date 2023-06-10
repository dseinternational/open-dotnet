// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Serialization;

/// <summary>
/// An object that can have additional data attached via an <see cref="ExtensionData"/> dictionary.
/// </summary>
public interface IExtensionData
{
    bool HasExtensionData { get; }

    IDictionary<string, object> ExtensionData { get; }
}

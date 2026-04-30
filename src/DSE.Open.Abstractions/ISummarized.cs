// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Represents an object that has a summary.
/// </summary>
public interface ISummarized
{
    /// <summary>
    /// Gets the summary text for the object.
    /// </summary>
    string Summary { get; }
}

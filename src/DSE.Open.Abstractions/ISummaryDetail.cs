// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Represents an object that has a summary and detail text.
/// </summary>
public interface ISummaryDetail : ISummarized
{
    string Detail { get; }
}

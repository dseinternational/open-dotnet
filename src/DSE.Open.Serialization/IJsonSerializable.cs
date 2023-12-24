// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Serialization;

/// <summary>
/// Indicates an object is designed to be serialized to Json.
/// </summary>
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "<Pending>")]
public interface IJsonSerializable;

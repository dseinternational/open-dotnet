// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Records;

/// <summary>
/// Represents a value that wraps a SNOMED CT Concept ID stored as a <see cref="long"/>.
/// </summary>
/// <typeparam name="TSelf">The implementing type.</typeparam>
public interface IClinicalConceptCode<TSelf> : IEquatableValue<TSelf, long>
    where TSelf : struct, IClinicalConceptCode<TSelf>;

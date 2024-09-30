// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

#pragma warning disable CA1716 // Identifiers should not match keywords

public interface IIdentifier<TSelf>
    : IEquatable<TSelf>,
      ISpanFormattable
    where TSelf : IIdentifier<TSelf>
{
    static abstract TSelf New();
}

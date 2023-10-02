// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

public interface IFormattableValue<TSelf, T> : IValue<TSelf, T>, IFormattable
    where T : IEquatable<T>, IFormattable
    where TSelf : struct, IFormattableValue<TSelf, T>
{
    new virtual string ToString(string? format, IFormatProvider? formatProvider)
       => ((T)this).ToString(format, formatProvider);
}

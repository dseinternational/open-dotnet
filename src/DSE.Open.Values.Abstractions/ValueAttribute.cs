// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

public abstract class ValueAttribute : Attribute
{
    /// <summary>
    /// If specified, source generator will generate a <see cref="ISpanSerializable{TSelf}.MaxSerializedCharLength"/>
    /// property that returns the specified value. Otherwise, the property must be implemented.
    /// </summary>
    public int MaxSerializedCharLength { get; set; }

    /// <summary>
    /// Defines whether to emit a method for implicit conversion to the inner value type. Defaults to <see langword="true"/>.
    /// </summary>
    public bool ImplicitDowncast { get; set; } = true;
}

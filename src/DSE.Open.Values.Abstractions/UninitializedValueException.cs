// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Values;

/// <summary>
/// An exception that is thrown when an attempt is made to use an uninitialized <see cref="IValue{TSelf, T}"/>
/// value in an operation that requires a valid value.
/// </summary>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
public sealed class UninitializedValueException<TValue, T> : Exception
    where T : IEquatable<T>
    where TValue : struct
{
    private static readonly string DefaultMessage =
        $"Cannot use a value of type {typeof(TValue)} that is not initialized.";

    public UninitializedValueException() : this(DefaultMessage)
    {
    }

    public UninitializedValueException(string message)
        : base(message ?? DefaultMessage)
    {
    }

    public UninitializedValueException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }

    public static void ThrowIfUninitialized(TValue value)
    {
        if (default(TValue).Equals(value))
        {
            Throw();
        }
    }

    [DoesNotReturn]
    private static void Throw()
    {
        throw new UninitializedValueException<TValue, T>();
    }
}

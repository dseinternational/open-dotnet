// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

public class ValueConversionException : Exception
{
    public ValueConversionException()
    {
    }

    public ValueConversionException(string message) : base(message)
    {
    }

    public ValueConversionException(string message, Exception? innerException) : base(message, innerException)
    {
    }

    public ValueConversionException(string message, object value, Exception? innerException) : base(message, innerException)
    {
        Value = value;
    }

    public object? Value { get; set; }

    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(string message) => throw new ValueConversionException(message);

    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(string message, Exception? innerException) => throw new ValueConversionException(message, innerException);

    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(string message, object value, Exception? innerException) => throw new ValueConversionException(message, value, innerException);
}

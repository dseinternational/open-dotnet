// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using CSnakes.Runtime.Python;

namespace DSE.Open.Interop.Python;

/// <summary>
/// Extension methods for converting <see cref="PyObject"/> values to .NET numeric and nullable types.
/// </summary>
public static class PyObjectExtensions
{
    /// <summary>
    /// Converts a Python object to a .NET numeric value of type <typeparamref name="T"/>,
    /// reading the source as a <see cref="double"/> when <typeparamref name="T"/> is a floating-point
    /// type and as a <see cref="long"/> otherwise.
    /// </summary>
    public static T AsNumber<T>(this PyObject pyObject)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(pyObject);

        if (typeof(T) == typeof(float) || typeof(T) == typeof(double) || typeof(T) == typeof(Half))
        {
            return T.CreateChecked(pyObject.As<double>());
        }

        return T.CreateChecked(pyObject.As<long>());
    }

    /// <summary>
    /// Converts a Python object to a .NET numeric value of type <typeparamref name="T"/>, or the
    /// default value when the Python object is <c>None</c>.
    /// </summary>
    public static T? AsNullableNumber<T>(this PyObject pyObject)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(pyObject);
        return pyObject.IsNone() ? default : AsNumber<T>(pyObject);
    }

    /// <summary>
    /// Converts a Python object to a value of type <typeparamref name="T"/>, or the default
    /// value when the Python object is <c>None</c>.
    /// </summary>
    public static T? AsNullable<T>(this PyObject pyObject)
    {
        ArgumentNullException.ThrowIfNull(pyObject);
        return pyObject.IsNone() ? default : pyObject.As<T>();
    }
}

// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using CSnakes.Runtime.Python;

namespace DSE.Open.Interop.Python;

public static class PyObjectExtensions
{
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

    public static T? AsNullableNumber<T>(this PyObject pyObject)
        where T : INumberBase<T>
    {
        ArgumentNullException.ThrowIfNull(pyObject);
        return pyObject.IsNone() ? default : AsNumber<T>(pyObject);
    }

    public static T? AsNullable<T>(this PyObject pyObject)
    {
        ArgumentNullException.ThrowIfNull(pyObject);
        return pyObject.IsNone() ? default : pyObject.As<T>();
    }
}

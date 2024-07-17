// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using Python.Runtime;

namespace DSE.Open.Interop.Python;

public static class PyConverter
{
    public static bool GetBool(dynamic pyBool)
    {
        ArgumentNullException.ThrowIfNull((object?)pyBool);

        var value = GetInt32(pyBool);

        return value != 0;
    }

    public static short GetInt16(dynamic pyInt)
    {
        ArgumentNullException.ThrowIfNull((object?)pyInt);

        using (Py.GIL())
        {
            using var pyObj = new PyInt(pyInt);
            return pyObj.ToInt16();
        }
    }

    public static int GetInt32(dynamic pyInt)
    {
        ArgumentNullException.ThrowIfNull((object?)pyInt);

        using (Py.GIL())
        {
            using var pyObj = new PyInt(pyInt);
            return pyObj.ToInt32();
        }
    }

    public static long GetInt64(dynamic pyInt)
    {
        ArgumentNullException.ThrowIfNull((object?)pyInt);

        using (Py.GIL())
        {
            using var pyObj = new PyInt(pyInt);
            return pyObj.ToInt64();
        }
    }

    public static float GetFloat(dynamic pyFloat)
    {
        ArgumentNullException.ThrowIfNull((object?)pyFloat);

        using (Py.GIL())
        {
            var dynDoublePyFloat = PyFloat.AsFloat(pyFloat);
            return dynDoublePyFloat.As<float>();
        }
    }

    public static double GetDouble(dynamic pyFloat)
    {
        ArgumentNullException.ThrowIfNull((object?)pyFloat);

        using (Py.GIL())
        {
            var dynDoublePyFloat = PyFloat.AsFloat(pyFloat);
            return dynDoublePyFloat.As<double>();
        }
    }

    public static short? GetInt16OrNull(dynamic pyInt)
    {
        if (pyInt is null)
        {
            return null;
        }

        using (Py.GIL())
        {
            using var pyObj = new PyInt(pyInt);
            return pyObj.ToInt16();
        }
    }

    public static int? GetInt32OrNull(dynamic pyInt)
    {
        if (pyInt is null)
        {
            return null;
        }

        using (Py.GIL())
        {
            using var pyObj = new PyInt(pyInt);
            return pyObj.ToInt32();
        }
    }

    public static long? GetInt64OrNull(dynamic pyInt)
    {
        if (pyInt is null)
        {
            return null;
        }

        using (Py.GIL())
        {
            using var pyObj = new PyInt(pyInt);
            return pyObj.ToInt64();
        }
    }

    public static string GetString(dynamic pyString)
    {
        ArgumentNullException.ThrowIfNull((object?)pyString);

        using (Py.GIL())
        {
            using var pyStr = new PyString((PyObject)pyString);
            var str = pyStr.ToString(CultureInfo.InvariantCulture);
            return str;
        }
    }

    public static string? GetStringOrNull(dynamic pyObj)
    {
        if (pyObj is null)
        {
            return null;
        }

        using (Py.GIL())
        {
            using var depPy = new PyString((PyObject)pyObj);
            var str = depPy.ToString(CultureInfo.InvariantCulture);
            return str;
        }
    }

    public static string? GetStringOrEmpty(dynamic pyObj)
    {
        if (pyObj is null)
        {
            return string.Empty;
        }

        using (Py.GIL())
        {
            using var depPy = new PyString((PyObject)pyObj);
            var str = depPy.ToString(CultureInfo.InvariantCulture);
            return str;
        }
    }

    private static T CreateObject<T>(PyObject pyObj)
        where T : IPyObjectWrapper<T>
    {
        return T.FromPyObject(pyObj);
    }

    public static Collection<T> GetList<T>(dynamic pyList)
        where T : IPyObjectWrapper<T>
    {
        ArgumentNullException.ThrowIfNull((object?)pyList);

        using (Py.GIL())
        {
            dynamic pyBuiltIn = Py.Import("builtins");

            using var pyCount = new PyInt(pyBuiltIn.len(pyList));

            var count = pyCount.ToInt32();

            var list = new Collection<T>(count);

            for (var i = 0; i < count; i++)
            {
                var element = pyList[i];

                var item = T.FromPyObject(element);

                if (item is null)
                {
                    throw new PythonInteropException($"Failed to create {typeof(T).Name} object.");
                }

                list.Add(item);
            }

            return list;
        }
    }

    public static Collection<T> GetList<T>(dynamic pyList, PyWrapperFactory<T> factory)
    {
        ArgumentNullException.ThrowIfNull((object?)pyList);
        ArgumentNullException.ThrowIfNull(factory);

        using (Py.GIL())
        {
            dynamic pyBuiltIn = Py.Import("builtins");

            using var pyCount = new PyInt(pyBuiltIn.len(pyList));

            var count = pyCount.ToInt32();

            var list = new Collection<T>(count);

            for (var i = 0; i < count; i++)
            {
                var element = pyList[i];

                if (element is null)
                {
                    throw new InvalidOperationException("Null value in list.");
                }

                var item = factory(element);

                if (item is null)
                {
                    throw new PythonInteropException($"Failed to create {typeof(T).Name} object.");
                }

                list.Add(item);
            }

            return list;
        }
    }
}

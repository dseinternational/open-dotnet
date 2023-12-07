// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Python.Runtime;

namespace DSE.Open.Interop.Python;

public interface IPyObjectWrapper<TSelf>
{
#pragma warning disable CA1000 // Do not declare static members on generic types
    static abstract TSelf FromPyObject(PyObject pyObj);
#pragma warning restore CA1000 // Do not declare static members on generic types
}

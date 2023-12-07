// Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
// Information contained herein is PROPRIETARY AND CONFIDENTIAL.

using Python.Runtime;

namespace DSE.Language.Annotations.Stanza.Interop;

internal interface IPyObjectWrapper<TSelf>
{
    static abstract TSelf FromPyObject(PyObject pyObj);
}

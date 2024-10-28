// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Interop.Python;

public sealed class PythonContextConfiguration
{
    public PythonContextConfiguration()
    {
        if (OperatingSystem.IsWindows())
        {
            PythonDLL = "python313.dll";
        }
        else
        {
            PythonDLL = OperatingSystem.IsLinux()
                ? "libpython3.13.so"
                : OperatingSystem.IsMacOS() ? "/Library/Frameworks/Python.framework/Versions/3.13/bin/python3" : throw new PlatformNotSupportedException();
        }
    }

    public string PythonDLL { get; set; }
}

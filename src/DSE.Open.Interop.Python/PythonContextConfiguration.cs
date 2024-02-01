// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Interop.Python;

public sealed class PythonContextConfiguration
{
    public PythonContextConfiguration()
    {
        if (OperatingSystem.IsWindows())
        {
            PythonDLL = "python312.dll";
        }
        else
        {
            PythonDLL = OperatingSystem.IsLinux()
                ? "libpython3.12.so"
                : OperatingSystem.IsMacOS() ? "libpython3.12.dylib" : throw new PlatformNotSupportedException();
        }
    }

    public string PythonDLL { get; set; }
}

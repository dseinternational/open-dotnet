// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language.Annotations.Nlp.Stanza;

public sealed class PythonContextConfiguration
{
    public PythonContextConfiguration()
    {
        if (OperatingSystem.IsWindows())
        {
            PythonDLL = "python311.dll";
        }
        else if (OperatingSystem.IsLinux())
        {
            PythonDLL = "libpython3.11.so";
        }
        else
        {
            PythonDLL = OperatingSystem.IsMacOS() ? "libpython3.11.dylib" : throw new PlatformNotSupportedException();
        }
    }

    public string PythonDLL { get; set; }
}

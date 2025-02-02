// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Interop.Python;

public sealed class PythonContextConfiguration
{
    public PythonContextConfiguration()
    {
        string? location = null;

        if (OperatingSystem.IsWindows())
        {
            var localAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Programs\\Python\\Python313\\python313.dll");

            if (File.Exists(localAppData))
            {
                location = localAppData;
            }
            else
            {
                var programFiles = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "Python313\\python313.dll");

                if (File.Exists(programFiles))
                {
                    location = programFiles;
                }
            }
        }
        else
        {
            PythonDLL = OperatingSystem.IsLinux()
                ? "libpython3.13.so"
                : OperatingSystem.IsMacOS()
                    ? "/Library/Frameworks/Python.framework/Versions/3.13/bin/python3.13"
                    : throw new PlatformNotSupportedException();
        }

        PythonDLL = location ?? throw new InvalidOperationException("Python 3.13 library not found.");
    }

    public string PythonDLL { get; set; }
}

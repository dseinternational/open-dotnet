// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;

namespace DSE.Open.Interop.Python;

public sealed class PythonContextConfiguration
{
    private static readonly string[] s_environmentVariables =
    [
        "PYTHON3_HOME",
        "PYTHON_HOME",
        "pythonLocation"
    ];

    public PythonContextConfiguration()
    {
        var filename = OperatingSystem.IsWindows()
            ? "python313.dll"
            : OperatingSystem.IsMacOS()
                ? "libpython3.13.dylib"
                : "libpython3.13.so";

        var searchLocations = s_environmentVariables
            .Select(v =>
            {
                if (Environment.GetEnvironmentVariable(v) is { Length: > 0 } value)
                {
                    return Path.Combine(value, filename);
                }

                return null;
            })
            .Where(s => s is not null)
            .ToList();

        if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
        {
            searchLocations.AddRange(s_environmentVariables
                .Select(v =>
                {
                    if (Environment.GetEnvironmentVariable(v) is { Length: > 0 } value)
                    {
                        return Path.Combine(value, "lib", filename);
                    }

                    return null;
                })
                .Where(s => s is not null));
        }

        if (OperatingSystem.IsWindows())
        {
            searchLocations.Add(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Programs\\Python\\Python313\\python313.dll"));
            searchLocations.Add(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "Python313\\python313.dll"));
        }
        else if (OperatingSystem.IsMacOS())
        {
            searchLocations.Add($"/Library/Frameworks/Python.framework/Versions/3.13/lib/{filename}");
        }

        searchLocations.Add(filename);

        var location = searchLocations.FirstOrDefault(File.Exists);

        PythonDLL = location ?? throw new InvalidOperationException(
            "Python 3.13 library not found. Locations searched: "
            + string.Join(", ", searchLocations.Select(s => $"\"{s}\"")));

        Trace.WriteLine($"Using Python 3.13 library at: {PythonDLL}");
    }

    public string PythonDLL { get; set; }
}

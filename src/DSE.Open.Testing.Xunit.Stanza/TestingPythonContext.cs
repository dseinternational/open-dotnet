// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Interop.Python;

namespace DSE.Open.Testing.Xunit.Stanza;

/// <summary>
/// A singleton instance of <see cref="PythonContext"/> for use in testing.
/// </summary>
public static class TestingPythonContext
{
    private static readonly Lazy<PythonContext> s_pythonContext =
        new(() => new(new PythonContextConfiguration()));

    public static PythonContext Instance => s_pythonContext.Value;
}

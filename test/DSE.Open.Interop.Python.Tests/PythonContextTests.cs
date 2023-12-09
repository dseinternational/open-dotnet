// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Interop.Python;

public class PythonContextTests
{
    [Fact]
    public void CanCreateAndDispose()
    {
        var context = new PythonContext(new PythonContextConfiguration());

        context.Dispose();
    }
}

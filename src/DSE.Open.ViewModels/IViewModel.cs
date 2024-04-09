// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.ViewModels;

public interface IViewModel
{
    bool IsReadOnly { get; }

    string ViewName { get; }
}

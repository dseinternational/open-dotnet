// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.ViewModels;

public abstract class ViewModel : IViewModel
{
    protected ViewModel()
    {
        ViewName = GetType().Name;
    }

    public virtual bool IsReadOnly { get; set; }

    public virtual string ViewName { get; set; }
}

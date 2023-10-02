// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values.Generators;

internal class ParentClass
{
    public ParentClass(string keyword, string name, string constraints, ParentClass? child)
    {
        Keyword = keyword;
        Name = name;
        Constraints = constraints;
        Child = child;
    }

    public ParentClass? Child { get; }
    public string Keyword { get; }
    public string Name { get; }
    public string Constraints { get; }
}

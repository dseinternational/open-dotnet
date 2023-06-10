// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Text;

/// <summary>Indicates a text capitalisation style.</summary>
public enum CapitalizationStyle
{
    /// <summary>The capitalisation style is undefined.</summary>
    Undefined,

    /// <summary>Pascal case - e.g. <strong>BackColor</strong>
    /// </summary>
    PascalCase,

    /// <summary>Camel case - e.g. <strong>backColor</strong>
    /// </summary>
    CamelCase,

    /// <summary>Lowercase - e.g. <strong>color</strong>
    /// </summary>
    Lowercase,

    /// <summary>Uppercase - e.g. <strong>IO</strong>
    /// </summary>
    Uppercase
}

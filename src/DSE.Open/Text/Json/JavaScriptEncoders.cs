// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace DSE.Open.Text.Json;

public static class JavaScriptEncoders
{
    /// <summary>
    /// A <see cref="JavaScriptEncoder"/> configured to serialize all language sets without escaping.
    /// </summary>
    public static readonly JavaScriptEncoder UnicodeRangesAll = JavaScriptEncoder.Create(UnicodeRanges.All);

    /// <summary>
    /// A <see cref="JavaScriptEncoder"/> configured to serialize all language sets without escaping
    /// and that does not escape HTML-sensitive characters such as &lt;, &gt;, &amp;, and '.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Compared to the default encoder, the <see cref="JavaScriptEncoder.UnsafeRelaxedJsonEscaping"/>
    /// encoder is more permissive about allowing characters to pass through unescaped:
    /// </para>
    /// <list type="bullet">
    /// <item>It does not escape HTML-sensitive characters such as &lt;, &gt;, &amp;, and '.</item>
    /// <item>It does not offer any additional defense-in-depth protections against XSS or information
    /// disclosure attacks, such as those which might result from the client and server disagreeing on
    /// the charset.</item>
    /// </list>
    /// <para>
    /// Use the unsafe encoder only when it's known that the client will be interpreting the resulting
    /// payload as UTF-8 encoded JSON. For example, you can use it if the server is sending the response
    /// header Content-Type: application/json; charset=utf-8. Never allow the raw UnsafeRelaxedJsonEscaping
    /// output to be emitted into an HTML page or a &lt;script&gt; element.
    /// </para>
    /// </remarks>
    public static readonly JavaScriptEncoder RelaxedJsonEscaping = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
}

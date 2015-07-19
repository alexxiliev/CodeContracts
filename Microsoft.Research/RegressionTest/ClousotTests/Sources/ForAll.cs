// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Diagnostics.Contracts;
using Microsoft.Research.ClousotRegression;
using System.Collections.Generic;

internal class Test
{
    [ClousotRegressionTest]
    private static string CSharpColorizePre(string text)
    {
        Contract.Requires(text != null);
        var split = text.Split(new string[] { "<pre>", "</pre>" }, StringSplitOptions.None);
        if (split.Length == 0) return text;
        Contract.Assume(Contract.ForAll(split, s => s != null));
        var result = new StringBuilder();
        result.Append(split[0]);
        var index = 1;
        while (index < split.Length)
        {
            result.Append("<pre>");
            result.Append(CSharpColorize(split[index++]));
            result.Append("</pre>");
            if (index < split.Length)
            {
                result.Append(split[index++]);
            }
        }
        return result.ToString();
    }

    [ClousotRegressionTest]
    private static string CSharpColorize(string text)
    {
        Contract.Requires(text != null);
        Contract.Ensures(Contract.Result<string>() != null);

        var result = text;
        result = System.Text.RegularExpressions.Regex.Replace(result, "(bool|new|throw|public|interface|abstract|class|typeof|get|return|default|if|void|string|null)", "<span class=\"code\" style=\"color:Blue;\">$&</span>");
        result = System.Text.RegularExpressions.Regex.Replace(result, "Contract[a-zA-Z]*", "<span class=\"code\" style=\"color:#2B91AF;\">$&</span>");
        result = System.Text.RegularExpressions.Regex.Replace(result, "//.*", "<span class=\"code\" style=\"color:Green;\">$&</span>");
        return result;
    }
}

public static class FrancescoTest
{
    [Pure]
    [ClousotRegressionTest]
    public static IEnumerable<T> AssumeAllNonNull<T>(this IEnumerable<T> sequence) where T : class
    {
        Contract.Requires(sequence != null);
        Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
        Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<T>>(), e => e != null));
        Contract.Assume(Contract.ForAll(sequence, e => e != null));
        return sequence;
    }
    [ClousotRegressionTest]
    public static void Test1(IEnumerable<Object> x)
    {
        Contract.Requires(x != null);

        foreach (var e in x.AssumeAllNonNull())
        {
            Contract.Assert(e != null);
        }
    }
    [ClousotRegressionTest]
    public static void Test2(IEnumerable<Object> x)
    {
        Contract.Requires(x != null);
        Contract.Requires(Contract.ForAll(x, el => el != null));

        foreach (var e in x)
        {
            Contract.Assert(e != null);
        }
    }
}

public class Class1<TKey, TValue> where TValue : class
{
    [ClousotRegressionTest]
    public ICollection<TValue> ValuesAsCollection()
    {
        Contract.Ensures(Contract.Result<ICollection<TValue>>() != null);
        Contract.Ensures(Contract.ForAll(Contract.Result<ICollection<TValue>>(), v => v != null));

        ICollection<TValue> values = _inner.Values;

        Contract.Assume(Contract.ForAll(values, v => v != null));

        return values;
    }

    [ContractInvariantMethod]
    private void ObjectInvariant()
    {
        Contract.Invariant(_inner != null);
    }

    private readonly Dictionary<TKey, TValue> _inner = new Dictionary<TKey, TValue>();
}



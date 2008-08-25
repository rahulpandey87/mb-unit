using System;
using System.Collections;

namespace MbUnit.Framework
{
    /// <summary>
    /// Tags a method as producing a seed value for a production grammar test fixture
    /// </summary>
    /// <remarks>
    /// For more information on Production Grammar Fixtures, please read this <a href="http://blog.dotnetwiki.org/ProductionGrammarFixtureInMbUnit.aspx">introduction</a>.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
    public sealed class SeedAttribute : Attribute
    {}
}

using System;
using System.Collections;

namespace MbUnit.Framework
{
    /// <summary>
    /// Identifies the tagged method as a Production Grammar 
    /// </summary>
    /// <remarks>
    /// For more information on Production Grammars, please read this <a href="http://blog.dotnetwiki.org/ProductionGrammarFixtureInMbUnit.aspx">introduction</a>.
    /// </remarks>
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
    public sealed class GrammarAttribute : Attribute
    {}
}

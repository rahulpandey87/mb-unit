using System;
using System.Collections;
using System.Reflection;
using MbUnit.Core;
using MbUnit.Core.Runs;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;
using TestFu.Grammars;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=false,Inherited=true)]	
	public sealed  class GrammarFixtureAttribute : TestFixturePatternAttribute
	{
		public override IRun GetRun()
		{
			return new ProductionGrammarRun();
        }

        #region ProductionGrammarRun
        public class ProductionGrammarRun : Run
        {
            public override void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
            {
                foreach (MethodInfo grammar in TypeHelper.GetAttributedMethods(t, typeof(GrammarAttribute)))
                {
                    foreach (MethodInfo seed in TypeHelper.GetAttributedMethods(t, typeof(SeedAttribute)))
                    {
                        ProductionGrammarRunInvoker invoker =
                            new ProductionGrammarRunInvoker(this, grammar, seed);
                        tree.AddChild(parent, invoker);
                    }
                }

            }

            public override bool IsTest
            {
                get
                {
                    return true;
                }
            }

            public override string Name
            {
                get
                {
                    return this.GetType().Name;
                }
            }

        }
        #endregion    
    
        #region ProductionGrammarRunInvoker

        public class ProductionGrammarRunInvoker : RunInvoker
        {
            private MethodInfo grammarMethod;
            private MethodInfo seedMethod;

            public ProductionGrammarRunInvoker(
                IRun generator,
                MethodInfo grammarMethod,
                MethodInfo seedMethod
                )
			:base(generator)
            {
                if (grammarMethod == null)
                    throw new ArgumentNullException("grammarMethod");
                if (seedMethod == null)
                    throw new ArgumentNullException("seedMethod");
                this.grammarMethod = grammarMethod;
                this.seedMethod = seedMethod;
            }

            public override string Name
            {
                get
                {
                    return String.Format("Grammar({0},{1})",
                        this.grammarMethod.Name,
                        this.seedMethod.Name
                        );
                }
            }

            public override object Execute(object o, System.Collections.IList args)
            {
                Object seed = this.seedMethod.Invoke(o, null);
                IGrammar grammar = this.grammarMethod.Invoke(o, null) as IGrammar;
                if (grammar == null)
                    throw new ArgumentException("Grammar method " + this.grammarMethod.Name + " does not return a IGrammar");
                try
                {
                    grammar.Produce(seed);
                }
                catch (ProductionException)
                { }

                return null;
            }
        }
        #endregion
    }
}

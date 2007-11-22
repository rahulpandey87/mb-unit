// MbUnit Test Framework
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		MbUnit HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux


namespace MbUnit.Core
{
	using System;
	using System.IO;
	using MbUnit.Core.Collections;
	using MbUnit.Core.Invokers;
	using MbUnit.Framework;
	
	/// <summary>
	/// This class represents the execution pipe of a test. It contains a 
	/// sequence of <see cref="IRunInvoker"/>.
	/// </summary>
	/// <include file="MbUnit.Framework.Doc.xml" path="doc/remarkss/remarks[@name='RunPipe']"/>
	public class RunPipe
	{
		private Guid identifier = Guid.NewGuid();
        private Fixture fixture = null;
        private RunInvokerVertexCollection invokers = new RunInvokerVertexCollection();
		
		/// <summary>
		/// Default constructor - initializes all fields to default values
		/// </summary>
        public RunPipe(Fixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            this.fixture = fixture;
        }		

		public Guid Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		public RunInvokerVertexCollection Invokers
		{
			get
			{
				return this.invokers;
			}
		}

        public Fixture Fixture
        {
            get
            {
                return this.fixture;
            }
        }
        public Type FixtureType
        {
            get
            {
                return this.fixture.Type;
            }
        }

        public string FixtureName
		{
			get
			{
                if (fixture.Type.DeclaringType!=null)
                {
                    string name = this.FixtureType.FullName;
                    return name.Substring(
                        this.FixtureType.Namespace.Length+1,
                        name.Length - this.FixtureType.Namespace.Length-1
                        );
                }
                else
                    return this.FixtureType.Name;
            }
		}

        public string ShortName
		{
			get
			{
				StringWriter sw = new StringWriter();
				foreach(RunInvokerVertex v in this.invokers)
				{
                    //if(v.Invoker.Generator.IsTest)
    					sw.Write(".{0}",v.Invoker.Name);
				}
				return sw.ToString().TrimStart('.');
			}
		}
		
		public string Name
		{
			get
			{
				return String.Format("{0}.{1}",this.FixtureName,this.ShortName);
			}
		}

		public override string ToString()
		{
			return this.Name;
		}
    }
}

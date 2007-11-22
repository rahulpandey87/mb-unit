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

using System;
using System.Reflection;
using MbUnit.Framework;
using MbUnit.Core.Runs;
using MbUnit.Core.Collections;
using MbUnit.Core.Framework;

namespace MbUnit.Core
{
	internal class AttributeFixtureFactory : IFixtureFactory
	{
		public void Create(Type t, FixtureCollection fixtures)
		{
            MethodInfo setUp = null;
            MethodInfo tearDown = null;
            bool setUpSearched = false;
            bool tearDownSearched=false;

            bool ignored = TypeHelper.HasCustomAttribute(t, typeof(IgnoreAttribute));

            foreach (TestFixturePatternAttribute attr in t.GetCustomAttributes(typeof(TestFixturePatternAttribute), true))
            {
                IRun run = null;
                try
                {
                    run = attr.GetRun();
                }
                catch (Exception ex)
                {
                    run = new FixtureFailedLoadingRun(t, ex);
                }
                if (!setUpSearched)
                {
                    setUp = TypeHelper.GetAttributedMethod(t, typeof(TestFixtureSetUpAttribute));
                    setUpSearched = true;
                }
                if (!tearDownSearched)
                {
                    tearDown = TypeHelper.GetAttributedMethod(t, typeof(TestFixtureTearDownAttribute));
                    tearDownSearched = true;
                }

                Fixture fixture = new Fixture(t, run, setUp, tearDown, ignored);
                fixture.TimeOut = attr.GetTimeOut();
                fixture.ApartmentState = attr.ApartmentState;
                fixtures.Add(fixture);
            }
        }
	}
}

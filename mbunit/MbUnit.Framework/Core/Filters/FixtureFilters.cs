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

namespace MbUnit.Core.Filters
{
	using MbUnit.Framework;

    public sealed class FixtureFilters
    {
        private FixtureFilters() { }

        public static AndFixtureFilter And(FixtureFilterBase left, FixtureFilterBase right)
        {
            return new AndFixtureFilter(left, right);
        }

        public static OrFixtureFilter Or(FixtureFilterBase left, FixtureFilterBase right)
        {
            return new OrFixtureFilter(left, right);
        }

        public static NotFixtureFilter Not(FixtureFilterBase innerFilter)
        {
            return new NotFixtureFilter(innerFilter);
        }

        public static AnyFixtureFilter Any
        {
            get
            {
                return new AnyFixtureFilter();
            }
        }

        public static CategoryFixtureFilter Category(string pattern)
        {
            return new CategoryFixtureFilter(pattern);
        }

        public static NamespaceFixtureFilter Namespace(string pattern)
        {
            return new NamespaceFixtureFilter(pattern);
        }

        public static TypeFixtureFilter Type(string pattern)
        {
            return new TypeFixtureFilter(pattern);
        }

        public static TestImportanceFixtureFilter Importance(TestImportance importance)
        {
            return new TestImportanceFixtureFilter(importance);
        }

        public static AuthorFixtureFilter Author(string name)
        {
            return new AuthorFixtureFilter(name);
        }
/*
		public static TargetFixtureFilter Target(string name,ReflectionTarget rt)
		{
			return new TargetFixtureFilter(name,rt);
		}

		public static TargetFixtureFilter Target(string testPath)
		{
			return TargetFixtureFilter.Parse(testPath);
		}
*/
        public static CurrentFixtureFilter Current
        {
            get
            {
                return new CurrentFixtureFilter();
            }
        }
    }
}
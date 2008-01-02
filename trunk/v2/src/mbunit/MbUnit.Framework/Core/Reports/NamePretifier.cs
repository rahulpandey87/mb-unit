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
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

namespace MbUnit.Core.Reports
{
	public sealed class NamePretifier
	{
		private string fixtureSuffix = "Test";
		private string testSuffix = "Test";
        private IDictionary ignoredTokens = new Hashtable();
        private static Regex capitalWord = new Regex(@"[A-Z][a-z]*|[0-9]",
            RegexOptions.Compiled | RegexOptions.Singleline);

        public NamePretifier()
        {
            this.ignoredTokens.Add("SetUp", null);
            this.ignoredTokens.Add("TearDown", null);
            this.ignoredTokens.Add("", null);
        }
		public string FixtureSuffix
		{
			get
			{
				return this.fixtureSuffix;
			}
			set
			{
				this.fixtureSuffix=value;
			}
		}

		public string TestSuffix
		{
			get
			{
				return this.testSuffix;
			}
			set
			{
				this.testSuffix=value;
			}
		}

		public string PretifyFixture(string fixtureName)
		{
            if (fixtureName == null)
                throw new ArgumentNullException("fixtureName");
            if (fixtureName.Length == 0)
                throw new ArgumentException("fitureName is empty");
            string name = fixtureName;
            if (name.EndsWith(this.fixtureSuffix))
                name = name.Substring(0, name.LastIndexOf(this.fixtureSuffix));

            return name;
		}

		public string PretifyTest(string testName)
		{
            if (testName == null)
                throw new ArgumentNullException("testName");
            if (testName.Length == 0)
                throw new ArgumentNullException("testName");

            string name = testName;

            StringWriter sw= new StringWriter();
			foreach(string action in name.Split('.'))
			{
                if (this.ignoredTokens.Contains(action))
                    continue;

                string a = action;
                if (a.EndsWith(this.testSuffix))
                    a = a.Substring(0, action.LastIndexOf(this.testSuffix));

                string actionSentence = ConvertToSentence(a);
				sw.Write(",{0}",actionSentence);
			}
			return sw.ToString().TrimStart(',');
		}

		private static string ConvertToSentence(string word)
		{
            if (word.Length == 0)
                return "";

            string sentence = capitalWord.Replace(word, new MatchEvaluator(SplitWord)).TrimStart();
            return sentence;
        }

        private static string SplitWord(Match m)
        {
            return String.Format(" {0}",m.Value.ToLower());
        }

        private static string Capitalize(string name)
		{
			if (name==null)
				return null;
			if (name.Length==0)
				return name;
			string capitalized = String.Format("{0}{1}",
                Char.ToUpper(name[0]), 
                name.Substring(1,name.Length-1)
                );
            return capitalized;
        }
	}
}

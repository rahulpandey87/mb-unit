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
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MbUnit.Core.Reports.Serialization
{
    [Serializable]
    public class ReportResultExplorer
    {
        private ReportResult result;
        private IDictionary fixtureByTypes = new Hashtable();
        private IDictionary runByNames = new Hashtable();

        public ReportResultExplorer(ReportResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result");
            this.result = result;
            this.BuildDictionaries();
        }

        public ReportResult Result
        {
            get
            {
                return this.result;
            }
        }

        public static ReportResultExplorer Load(string fileName)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ReportResult));
                    ReportResult result = ser.Deserialize(reader) as ReportResult;
                    return new ReportResultExplorer(result);
                }
            }
            catch (Exception)
            {
                ReportResult result = new ReportResult();
                return new ReportResultExplorer(result);
            }
        }

        public Fixture GetFixtureByType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return this.fixtureByTypes[type.FullName] as Fixture;
        }

        public ReportRun GetRunByName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            return this.runByNames[name] as ReportRun;
        }

        protected void BuildDictionaries()
        {
            BuildDictionariesReportVisitor vis = new BuildDictionariesReportVisitor(this);
            vis.VisitResult(this.result);
        }

        private class BuildDictionariesReportVisitor : ReportVisitor
        {
            private ReportResultExplorer explorer;
            public BuildDictionariesReportVisitor(ReportResultExplorer explorer)
            {
                this.explorer = explorer;
            }

            public override void VisitFixture(ReportFixture fixture)
            {
                this.explorer.fixtureByTypes[fixture.Type] = fixture;
                base.VisitFixture(fixture);
            }


            public override void VisitRun(ReportRun run)
            {
                this.explorer.runByNames[run.Name]=run;
                base.VisitRun(run);
            }
        }
    }
}

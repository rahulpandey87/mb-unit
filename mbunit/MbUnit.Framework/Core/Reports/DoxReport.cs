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
using System.IO;

namespace MbUnit.Core.Reports
{
	using MbUnit.Core.Reports.Serialization;

	public sealed class DoxReport : ReportBase
	{
		private NamePretifier pretifier = new NamePretifier();

		public DoxReport()
		{
		}

		protected override string DefaultExtension
		{
			get { return ".dox.txt"; }
		}



		public NamePretifier Pretifier
		{
			get
			{
				return this.pretifier;
			}
		}

		public override void Render(ReportResult result, TextWriter writer)
		{
			DoxVisitor vis = new DoxVisitor(this, writer);
			vis.VisitResult(result);
		}

		public static string RenderToDox(ReportResult result)
		{
			return RenderToDox(result,"");
		}

		public static string RenderToDox(ReportResult result, string outputPath)
		{
			return RenderToDox(result, outputPath, "mbunit-result-dox-{0}{1}");
		}

		public static string RenderToDox(ReportResult result, string outputPath, string nameFormat)
		{
            if (result == null)
				throw new ArgumentNullException("result");
            DoxReport textReport = new DoxReport();
            return textReport.Render(result, outputPath, nameFormat);
        }

        public class DoxVisitor : ReportVisitor
		{
			private DoxReport dox;
			private TextWriter writer;
			private string tab="    ";

			public DoxVisitor(DoxReport dox, TextWriter writer)
			{
				this.dox = dox;
				this.writer = writer;
			}

			public override void VisitAssembly(ReportAssembly assembly)
			{
				writer.WriteLine("-- {0}",assembly.Name);
				base.VisitAssembly(assembly);
			}

			public override void VisitNamespace(ReportNamespace ns)
			{
				writer.WriteLine("{0}",ns.Name);
				base.VisitNamespace (ns);
			}

			public override void VisitFixture(ReportFixture fixture)
			{
				string pretty = dox.Pretifier.PretifyFixture(fixture.Name);
				writer.WriteLine("{0}{1}",this.tab,pretty);
				base.VisitFixture(fixture);
			}

			public override void VisitRun(ReportRun run)
			{
				string pretty = dox.Pretifier.PretifyTest(run.Name);
				writer.WriteLine("{0}  - {1}",this.tab,pretty);
				base.VisitRun (run);
			}
		}
	}
}

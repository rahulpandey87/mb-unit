// QuickGraph Library 
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
//		QuickGraph Library HomePage: http://www.dotnetwiki.org
//		Author: Jonathan de Halleux


namespace QuickGraph.Web.Controls
{
	using System;
	using System.IO;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Web.UI.Design;
	using System.ComponentModel;
	using System.Web.Caching;
	using System.Drawing;

	using QuickGraph.Algorithms;
	using QuickGraph.Algorithms.Graphviz;
	using QuickGraph.Representations;
	using QuickGraph.Providers;

	using NGraphviz;
	using NGraphviz.Helpers;


	/// <summary>
	/// A Cached Graphviz Web Control
	/// </summary>
	/// <remarks>
	/// <para>
	/// Only a subset of the Graphviz Ouput are supported:
	/// <list type="bullet">
	/// <item><term>Png</term><description>Png</description></item>
	/// <item><term>Gif</term><description>Gif</description></item>
	/// <item><term>Jpeg</term><description>Jpeg</description></item>
	/// <item><term>Svg</term><description>SVG</description></item>
	/// <item><term>Svgz</term><description>SVGZ</description></item>
	/// </list>
	/// </para>
	/// <para>
	/// Clearly, the best choice is SVGZ if your client supports it: it's 
	/// vectorial, compressed and anti-aliased!
	/// </para>
	/// </remarks>
	[Designer(typeof(QuickGraph.Web.Design.GraphvizDesigner))]	
	public class GraphvizControl 
		: Panel, INamingContainer
	{
		private TimeSpan m_TimeOut;
		private GraphvizAlgorithm m_Algo;
		private String m_RelativePath;
		private Size m_ImageSize;
		private String m_CustomCacheKey;

		/// <summary>
		/// Default constructor
		/// </summary>
		public GraphvizControl()
		{
			m_Algo = new GraphvizAlgorithm(
				new AdjacencyGraph(true)
				);
			RelativePath = "";
			m_TimeOut = new TimeSpan(0,0,0,0,0);
			m_ImageSize = new Size(0,0);
			m_CustomCacheKey = null;
		}

		/// <summary>
		/// Creates the child controls
		/// </summary>
		protected override void CreateChildControls()
		{
			Algo.Renderer.TempDir = ServerPath;
			Controls.Add(new LiteralControl("<table><tr><td>"));

			// getting cache file name
			String fileName = CachedImageFileName;
			if (fileName == null)
			{
				// creating image
				fileName = CachedImageFileName = NewImageFileName;
				Algo.Write(ServerFileName(fileName));
			}
			String relFileName = RelativeFileName(fileName);

			// Svg rendering
			if (Algo.ImageType == GraphvizImageType.Svg)
			{
				String embed = String.Format(
					"<embed src=\"{0}\" pluginspage=\"http://www.adobe.com/svg/viewer/install/\" type=\"image/svg+xml\" {1} />",
					relFileName,
					SizeToString()
					);
				Controls.Add(new LiteralControl(embed));				
			}
			// Zipped SVG rendering
			else if (Algo.ImageType == GraphvizImageType.Svgz)
			{
				String embed = String.Format(
					"<embed src=\"{0}\" pluginspage=\"http://www.adobe.com/svg/viewer/install/\" type=\"image/svgz+xml\" {1} />",
					relFileName,
					SizeToString()
					);
				Controls.Add(new LiteralControl(embed));				
			}
			// Raster image types
			else
			{
				HtmlImage img = new HtmlImage();
				Controls.Add(img);
				img.Src = relFileName;
				if (m_ImageSize.Width != 0)
					img.Width = m_ImageSize.Width;
				if (m_ImageSize.Height != 0)
					img.Height = m_ImageSize.Height;
			}

			// adding info
			Controls.Add(new LiteralControl("</td></tr><tr><td><font size=-3>"));
/*
			DateTime dt = File.GetCreationTime(ServerPath + '\\' + fileName);
			Controls.Add(new LiteralControl(
				String.Format("Graph Creation Time: {0}. Powered by <a href=\"http://www.dotnetwiki.org\">DotNetWiki.org</a>.",dt.ToLocalTime())
				)
				);
*/
			Controls.Add(new LiteralControl("</font></td></tr></table>"));
		}


		/// <summary>
		/// Rendering algorithm
		/// </summary>
		[Description("Graphviz Algorithm")]
		public GraphvizAlgorithm Algo
		{
			get
			{
				return m_Algo;
			}
		}

		/// <summary>
		/// Rendered image caching time out. 0 disables caching.
		/// </summary>
		[Description("Caching timeout of the generated image")]
		public TimeSpan TimeOut
		{
			get
			{
				return m_TimeOut;
			}
			set
			{
				m_TimeOut = value;
			}
		}

		/// <summary>
		/// Path the temporary files are outputted. Map to server location
		/// internaly.
		/// </summary>
		[Description("Temporary files output path")]
		public String RelativePath
		{
			get
			{
				return m_RelativePath;
			}
			set
			{
				m_RelativePath = value;
				//Algo.Renderer.TempDir = m_RelativePath;
			}
		}

		/// <summary>
		/// Image width in pixels. If 0 not used
		/// </summary>
		[Description("Output image width")]
		public int ImageWidth
		{
			get
			{
				return m_ImageSize.Width;
			}
			set
			{
				m_ImageSize.Width = value;
			}
		}

		/// <summary>
		/// Image height in pixels. If 0, not used
		/// </summary>
		[Description("Output image height")]
		public int ImageHeight
		{
			get
			{
				return m_ImageSize.Height;
			}
			set
			{
				m_ImageSize.Height = value;
			}
		}

		/// <summary>
		/// Mapped Server path
		/// </summary>
		protected String ServerPath
		{
			get
			{
				return Context.Server.MapPath(RelativePath);
			}
		}

		/// <summary>
		/// Maps file name to server path
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		protected string ServerFileName(string name)
		{
			if (name==null)
				throw new ArgumentNullException("file name");

			return String.Format("{0}\\{1}",ServerPath,name);
		}

		/// <summary>
		/// Maps file name to relative url path
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		protected string RelativeFileName(string name)
		{
			if (name==null)
				throw new ArgumentNullException("file name");
			if (RelativePath.Length == 0)
				return name;
			else
				return String.Format("{0}\\{1}",RelativePath,name);
		}

		/// <summary>
		/// Resets the cached image
		/// </summary>
		public void ResetCache()
		{
			Context.Cache.Remove( CacheKey );
		}

		/// <summary>
		/// A user defined cache key.
		/// </summary>
		public String CustomCacheKey
		{
			get
			{
				return m_CustomCacheKey;
			}
			set
			{
				ResetCache();
				m_CustomCacheKey = value;				
			}
		}

		/// <summary>
		/// Generates a unique cache key
		/// </summary>
		protected String CacheKey
		{
			get
			{
				if (CustomCacheKey != null)
					return CustomCacheKey;
				else
					return "DotControl" + ID;
			}
		}

		/// <summary>
		/// Provides a new image name
		/// </summary>
		protected String NewImageFileName
		{
			get
			{
				return String.Format("dot_render{0}.{1}",
					DateTime.Now.ToFileTime(),
					Algo.ImageType.ToString().ToLower()
					);
			}
		}

		/// <summary>
		/// Returns the last generated file (cached)
		/// </summary>
		/// <returns>latest generated file path or null if nothing on cache</returns>
		protected String CachedImageFileName
		{
			get
			{
				// getting cache
				Object o = Context.Cache[CacheKey];
				if (o == null)
					return null;
				else
					return (String)o;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("CachedImageFileName");

				Context.Cache.Add(
					CacheKey,
					value,
					null,
					DateTime.Now + TimeOut,
					TimeSpan.Zero,
					CacheItemPriority.Normal,
					null
					);
			}
		}

		/// <summary>
		/// True if the graph is currently on cache
		/// </summary>
		public bool IsCached
		{
			get
			{
				return CachedImageFileName != null;
			}
		}

		/// <summary>
		/// Renders image size to Xml Attributes
		/// </summary>
		/// <returns>attributes representing the size</returns>
		internal String SizeToString()
		{
			StringWriter sw =new StringWriter();
			if (m_ImageSize.Width != 0)
				sw.Write(" width={0} ",m_ImageSize.Width);
			if (m_ImageSize.Height != 0)
				sw.Write(" height={0} ",m_ImageSize.Height);

			return sw.ToString();
		}
	}
}

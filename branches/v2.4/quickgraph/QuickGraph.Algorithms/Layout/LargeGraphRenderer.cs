

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;

namespace QuickGraph.Algorithms.Layout
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;
	
	public class LargeGraphRenderer
	{
		private IVertexAndEdgeListGraph visitedGraph=null;
		private VertexPointFDictionary positions = new VertexPointFDictionary();
				
		private bool edgeVisible=true;
		private IEdgeRenderer edgeRenderer=new EdgeRenderer();

		private bool vertexVisible=true;		
		private IVertexRenderer vertexRenderer = new VertexRenderer();
		
		private Size layoutSize=new Size(800,600);		

		public LargeGraphRenderer()
		{}
		
		public LargeGraphRenderer(IVertexAndEdgeListGraph visitedGraph)
		{
			if (visitedGraph==null)
				throw new ArgumentNullException("visitedGraph");
			this.visitedGraph=visitedGraph;
		}
		
		public IVertexAndEdgeListGraph VisitedGraph
		{
			get
			{
				return this.visitedGraph;
			}
		}

		public IVertexPointFDictionary Positions
		{
			get
			{
				return this.positions;
			}
		}

		public void SetPositions(IVertexPointFDictionary positions)
		{
			if (positions==null)
				throw new ArgumentNullException("positions");
			foreach(DictionaryEntry de in positions)
				this.positions[(IVertex)de.Key]=(PointF)de.Value;
		}
		
		public bool EdgeVisible
		{
			get
			{
				return this.edgeVisible;	
			}
			set
			{
				this.edgeVisible=value;
			}
		}
		
		public IEdgeRenderer EdgeRenderer
		{
			get
			{
				return this.edgeRenderer;
			}
			set
			{
				this.edgeRenderer=value;
			}
		}

		
		public bool VertexVisible
		{
			get
			{
				return this.vertexVisible;
			}
			set
			{
				this.vertexVisible=value;
			}
		}		
		
		public IVertexRenderer VertexRenderer
		{
			get
			{
				return this.vertexRenderer;
			}
			set
			{
				this.vertexRenderer=value;
			}
		}		
		
		
		public Size LayoutSize
		{
			get
			{
				return this.layoutSize;
			}
			set
			{
				this.layoutSize=value;
			}
		}
		
		public RectangleF GetOriginalBox()
		{
			return new RectangleF(0,0,this.LayoutSize.Width, this.LayoutSize.Height);
		}

		public RectangleF GetBoundingBox()
		{			
			ICollection ps = this.Positions.Values;
			
			int index=0;
			float left=0,right=1,top=0,bottom=1;
			foreach(PointF p in ps)
			{
				if (index==0)
				{
					left=Math.Min(0,p.X);
					right=Math.Max(this.LayoutSize.Width,p.X);
					top=Math.Min(0,p.Y);
					bottom=Math.Max(this.LayoutSize.Height,p.Y);
				}
				else
				{
					left = Math.Min(p.X,left);
					right=Math.Max(p.X,right);
					top=Math.Max(p.Y,top);
					bottom=Math.Min(p.Y,bottom);
				}
				index++;
			}
			
			// fix ratio
//			float rw= ((float)right-left)/this.LayoutSize.Width;
//			float rh= ((float)top-bottom)/this.LayoutSize.Height;
			
//			if (rw>rh)		
				return new RectangleF(left,bottom, right-left, top-bottom);
//			else
//				return new RectangleF(left,bottom, right-left, top-bottom);
		}
		
		public void Render(string fileName, ImageFormat imageFormat)
		{
			using (Bitmap bmp = new Bitmap(this.LayoutSize.Width, this.LayoutSize.Height))
			{
				this.Render(bmp);
				bmp.Save(fileName,imageFormat);
			}
		}

		public void Render(Image img)
		{
			using (Graphics gr = Graphics.FromImage(img))
			{
				gr.SmoothingMode = SmoothingMode.AntiAlias;
				this.Render(gr);
			}			
		}
		
		public void Render(Graphics g)
		{
			if (this.VisitedGraph==null)
				return;
			if (this.positions.Count==0)
				return;
			
			GraphicsContainer container = null;
			RectangleF bb = GetBoundingBox();
			
			g.FillRectangle(Brushes.White,g.ClipBounds);
			g.BeginContainer(GetOriginalBox(),bb,GraphicsUnit.Pixel);
			
			if (this.edgeVisible)
			{
				int index = 0;
				foreach(IEdge e in this.VisitedGraph.Edges)
				{
					this.edgeRenderer.Render(g,
					                         e,
					                         this.Positions[e.Source],
					                         this.Positions[e.Target]						                         
					                         );
					++index;
				}							
			}
			
			if (this.vertexVisible)
			{
				this.vertexRenderer.PreRender();
				foreach(DictionaryEntry de in this.Positions)
				{
					this.vertexRenderer.Render(g,(IVertex)de.Key,(PointF)de.Value);
				}			
			}

			if (container!=null)
				g.EndContainer(container);
		}
	}
}

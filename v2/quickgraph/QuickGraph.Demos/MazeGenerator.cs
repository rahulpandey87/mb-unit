using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace QuickGraphTest
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Algorithms.RandomWalks;
	using QuickGraph;
	using QuickGraph.Providers;
	using QuickGraph.Collections;
	using QuickGraph.Representations;

	/// <summary>
	/// Summary description for MazeGenerator.
	/// </summary>
	public class MazeGenerator
	{
		private BidirectionalGraph graph = null;
		private VertexEdgeDictionary successors = null;
		private NamedVertex[,] latice;
		private Hashtable vertexIndices = null;
		private EdgeCollection outPath = null;

		private float pointWidth  = 5;
		private Color pointColor  = Color.Black;
		private float wallWidth = 1;
		private Color wallColor = Color.Black;
		private Color pathColor = Color.Red;

		public MazeGenerator(int rows, int columns)
		{
			this.latice=new NamedVertex[rows,columns];
			GenerateGraph();
		}

		public int Rows
		{
			get
			{
				return this.latice.GetLength(0);
			}
		}

		public int Columns
		{
			get
			{
				return this.latice.GetLength(1);
			}
		}

		public float PointWidth
		{
			get
			{
				return this.pointWidth;
			}
			set
			{
				this.pointWidth = Math.Max(0,value);
			}
		}

		public Color PointColor
		{
			get
			{
				return this.pointColor;
			}
			set
			{
				this.pointColor = value;
			}
		}

		public float WallWidth
		{
			get
			{
				return this.wallWidth;
			}
			set
			{
				this.wallWidth = Math.Max(0,value);
			}
		}

		public Color PathColor
		{
			get
			{
				return this.pathColor;
			}
			set
			{
				this.pathColor = value;
			}
		}
		
		public Color WallColor
		{
			get
			{
				return this.wallColor;
			}
			set
			{
				this.wallColor = value;
			}
		}
		
		public EdgeCollection OutPath
		{
			get
			{
				return this.outPath;
			}
		}

		public void GenerateGraph()
		{
			this.vertexIndices = new Hashtable();

			// create a new adjacency graph
			this.graph = new BidirectionalGraph(
				new NamedVertexProvider(),
				new EdgeProvider(),
				false);			

			int rows = this.Rows;
			int columns = this.Columns;
			// adding vertices
			for(int i=0;i<rows;++i)
			{
				for(int j=0;j<columns;++j)
				{
					NamedVertex v =(NamedVertex)this.graph.AddVertex();
					v.Name = String.Format("{0},{1}",i.ToString(),j.ToString());

					latice[i,j] = v;
					vertexIndices[v]=new DictionaryEntry(i,j);
				}
			}

			// adding edges
			for(int i =0;i<rows-1;++i)
			{
				for(int j=0;j<columns-1;++j)
				{
					this.graph.AddEdge(latice[i,j], latice[i,j+1]);
					this.graph.AddEdge(latice[i,j+1], latice[i,j]);

					this.graph.AddEdge(latice[i,j], latice[i+1,j]);
					this.graph.AddEdge(latice[i+1,j], latice[i,j]);
				}
			}

			for(int j=0;j<columns-1;++j)
			{
				this.graph.AddEdge(latice[rows-1,j], latice[rows-1,j+1]);
				this.graph.AddEdge(latice[rows-1,j+1], latice[rows-1,j]);
			}

			for(int i=0;i<rows-1;++i)
			{
				this.graph.AddEdge(latice[i,columns-1], latice[i+1,columns-1]);
				this.graph.AddEdge(latice[i+1,columns-1], latice[i,columns-1]);
			}
		}

		public void GenerateWalls(int outi, int outj, int ini, int inj)
		{
			// here we use a uniform probability distribution among the out-edges
			CyclePoppingRandomTreeAlgorithm pop = 
				new CyclePoppingRandomTreeAlgorithm(this.graph);

			// we can also weight the out-edges
/*
			EdgeDoubleDictionary weights = new EdgeDoubleDictionary();
			foreach(IEdge e in this.graph.Edges)
				weights[e]=1;
			pop.EdgeChain = new WeightedMarkovEdgeChain(weights);
*/
			IVertex root = this.latice[outi,outj];
			if (root==null)
				throw new ArgumentException("outi,outj vertex not found");
			
			pop.RandomTreeWithRoot(this.latice[outi,outj]);
			this.successors = pop.Successors;
			
			// build the path to ini, inj
			IVertex v = this.latice[ini,inj];
			if (v==null)
				throw new ArgumentException("ini,inj vertex not found");
			this.outPath = new EdgeCollection();
			while (v!=root)
			{
				IEdge e = this.successors[v];
				if (e==null)
					throw new Exception();
				this.outPath.Add(e);
				v = e.Target;
			}
		}

		public void Draw(Graphics g, RectangleF rect)
		{
			Debug.Assert(this.vertexIndices!=null);

			float dx = rect.Width/(this.Columns-1);
			float dy = rect.Width/(this.Rows-1);

			SolidBrush pointBrush = new SolidBrush(this.pointColor);
			Pen linePen = new Pen(this.wallColor, this.wallWidth);
//			linePen.EndCap = LineCap.ArrowAnchor;
			
			Pen pathPen = new Pen(this.pathColor, this.wallWidth+2);
			pathPen.EndCap = LineCap.ArrowAnchor;

			// draw points
			float x,y;
			for(int i=0;i<this.Rows;++i)
			{
				for (int j=0;j<this.Columns;++j)
				{
					x=rect.X+j*dx-pointWidth/2;
					y=rect.Y+i*dy-pointWidth/2;
					// draw vertex
					g.FillEllipse(pointBrush,x,y,pointWidth,pointWidth);
				}
			}

			// draw edges
			if (this.successors!=null)
			{
				foreach(IEdge e in this.successors.Values)
				{
					if (e==null)
						continue;
					DictionaryEntry s = (DictionaryEntry)vertexIndices[e.Source];
					DictionaryEntry t = (DictionaryEntry)vertexIndices[e.Target];
	
					float sx = rect.X + ((int)s.Value)*dx;
					float sy = rect.Y + ((int)s.Key)*dy;
					float tx = rect.X + ((int)t.Value)*dx;
					float ty = rect.Y + ((int)t.Key)*dy;
					g.DrawLine(linePen,sx,sy,tx,ty);
				}				
			}
			else
			{
				foreach(IEdge e in this.graph.Edges)
				{
					if (e==null)
						continue;
					DictionaryEntry s = (DictionaryEntry)vertexIndices[e.Source];
					DictionaryEntry t = (DictionaryEntry)vertexIndices[e.Target];
	
					float sx = rect.X + ((int)s.Value)*dx;
					float sy = rect.Y + ((int)s.Key)*dy;
					float tx = rect.X + ((int)t.Value)*dx;
					float ty = rect.Y + ((int)t.Key)*dy;
					g.DrawLine(linePen,sx,sy,tx,ty);
				}								
			}
			
			if (this.outPath!=null)	
			{
				// draw path
				foreach(IEdge e in this.outPath)
				{
					DictionaryEntry s = (DictionaryEntry)vertexIndices[e.Source];
					DictionaryEntry t = (DictionaryEntry)vertexIndices[e.Target];
	
					float sx = rect.X + ((int)s.Value)*dx;
					float sy = rect.Y + ((int)s.Key)*dy;
					float tx = rect.X + ((int)t.Value)*dx;
					float ty = rect.Y + ((int)t.Key)*dy;
					g.DrawLine(pathPen,sx,sy,tx,ty);
				}				
			}
			
		}

		public  void Save(
			string fileName, 
			ImageFormat format, 
			int width,
			int height)
		{
			Bitmap bmp = new Bitmap(width,height);
			Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.FillRectangle(Brushes.White,0,0,bmp.Width,bmp.Height);
			Draw(g,new RectangleF(pointWidth/2,pointWidth/2,
				bmp.Width-pointWidth, bmp.Height-pointWidth));

			bmp.Save(fileName,format);
		}
	}
}

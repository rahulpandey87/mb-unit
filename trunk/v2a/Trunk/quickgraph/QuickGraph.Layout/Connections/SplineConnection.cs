using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Netron;

namespace QuickGraph.Layout.Connections
{
	[ Connection("Spline") ]
	[ Serializable ]	
	public class SplineConnection : Connection
	{
		private string label = "";
		private Color labelColor = Color.Black;
		private Font labelFont = new Font("Tahoma",8f);

		[Category("Appearance")]
		public String Label
		{
			get	{	return this.label; }
			set	{	this.label = value;}
		}

		[Category("Appearance")]
		public Font LabelFont
		{
			get	{	return this.labelFont; }
			set	{	this.labelFont = value;}
		}

		[Category("Appearance")]
		public Color LabelColor
		{
			get	{	return this.labelColor; }
			set	{	this.labelColor = value;}
		}

		/// <summary>
		/// Paints the polyline.
		/// </summary>
		/// <param name="g">Graphic context</param>
		/// <param name="p">Pen to use</param>
		protected override void PaintPolyline(Graphics g, Pen p)
		{
			if (From == null) 
				return;
			if (To==null || ((this.Points.Count-4)%3!=0))
			{
				base.PaintPolyline(g,p);
				return;
			}

			// Get end points
			PointF s = From.Shape.ConnectionPoint(From);
			PointF e = To.Shape.ConnectionPoint(To);
			
			// Iterate the list of polyline points and paint the lines between them
			SmoothingMode m = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.HighQuality;

			PointF[] points = new PointF[this.Points.Count];
			this.Points.CopyTo(points,0);
			points[0]=s;
			points[points.Length-1]=e;

			g.DrawBeziers(p,points);		
			g.SmoothingMode = m;    	

			// draw label
			if (label!=null && this.label.Length!=0)
			{
				PointF middle = (PointF)this.Points[this.Points.Count/2];
				SolidBrush labelBrush = new SolidBrush(this.LabelColor);
				g.DrawString(this.label,this.labelFont,labelBrush,middle);
			}
		}
	}
}

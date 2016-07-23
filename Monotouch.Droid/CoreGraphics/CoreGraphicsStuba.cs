using System;
//using System.Drawing; 
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics; 

using Foundation; 
using UIKit; 

using System.Linq; 
using System.Collections.Generic; 

namespace CoreGraphics
{
	public enum CGPathDrawingMode
	{
		Fill,
		EOFill,
		Stroke,
		FillStroke,
		EOFillStroke
	}

	public class CGPath 
	{

		public List<List<System.Drawing.PointF>> Paths = null; 
		//System.Drawing.PointF[] Points = null; 
		public void AddLines (System.Drawing.PointF[] points)
		{
			if (Paths == null) {
				Paths = new List<List<System.Drawing.PointF>> (); 
			} 
			var Points = Paths.LastOrDefault (); 

			if (Points == null) {
				Points = new List<System.Drawing.PointF> (); 
				Paths.Add (Points); 
			} 
				
			Points.AddRange (points); 
	
			Paths [Paths.Count - 1] = Points; 
		}

		public void CloseSubpath() 
		{
			if (Paths != null) {
				Paths.Add(new List<System.Drawing.PointF>()); 
			}
		}


	}
	public class CGSize {
		float _W, _H; 

		public CGSize(nfloat w, nfloat h) {
			Width = w; 
			Height = h; 
		}

		public CGSize(int w, int h) {
			Width = w; 
			Height = h; 
		}

		public static CGSize Empty {
			get {
				return new CGSize(0,0); 
			}
		}

		public float Width {
			get {
				return _W; 
			} 
			set {
				_W = value;
			}
		}
		public float Height {
			get {
				return _H; 
			} 
			set {
				_H = value; 
			}
		}
	}

	public class CGRect {

		public CGRect(nfloat x, nfloat y, nfloat width, nfloat height) {
			this.Left = x;
			this.Top = y;
			this.Width = width;
			this.Height = height;

		}

		public static CGRect FromLTRB(nfloat l, nfloat t, nfloat r, nfloat b) {

			return new CGRect (l, t, r - l, b - t); 
			 
		}

		public static implicit operator CGRect (System.Drawing.RectangleF rect) {
			return new CGRect (rect.Left, rect.Top, rect.Right, rect.Bottom); 
		}
			

		float _X, _Y; 
		float _W, _H; 

		public CGSize Size {
			get {
				return new CGSize (Width, Height); 
			} 
			set {
				Width = value.Width;
				Height = value.Height;
			}
		}
		public float Width {
			get {
				return _W; 
			}
			set {
				_W = value; 
			}
		}
		public float Height {
			get {
				return _H; 
			}
			set {
				_H = value; 
			}
		}
		public float Top {
			get {
				return _Y; 
			} 
			set {
				_Y = value; 
			}
		}
		public float Bottom {
			get {
				return _Y + _H; 
			} 
			set {
				_H = value - _Y; 
			}
		}
		public float Left {
			get {
				return _X; 
			} 
			set {
				_X = value; 
			}
		}
		public float Right {
			get {
				return _X + _W; 
			} 
			set {
				_W = value - _X; 
			}
		}
		public float X {
			get {
				return _X; 
			} 
			set {
				_X = value; 
			}
		}
		public float Y {
			get {
				return _Y;
			} 
			set {
				_Y = value; 
			}
		}
	
	}

	public class CGPoint {
		public nfloat _X, _Y;
		public CGPoint(nfloat x, nfloat y) {
			X = x; 
			Y = y; 
		}

		public static CGPoint Empty {
			get {
				return new CGPoint(0,0); 
			}
		}

		public float X {
			get {
				return _X; 
			} 
			set {
				_X = value; 
			}
		}
		public float Y {
			get {
				return _Y;
			} 
			set {
				_Y = value; 
			}
		}
	}


	public class CGContext {

		public View CurrentView; 
		public CGContext(View v) {
			this.CurrentView = v; 
		}

		public Color FillColor; 
		public Color StrokeColor; 

		public void SetFillColor(Color c) {
			SetFillColorWithColor (c); 
		}


		public void SetFillColorWithColor(Color c) {
			FillColor = c; 
		}

		public void SetStrokeColor(Color c) {
			SetStrokeColorWithColor (c); 
		}

		public void SetStrokeColorWithColor(Color c) {
			StrokeColor = c; 
		}

		public float curX = 0 , curY = 0 ; 

		public void TranslateCTM(float x, float y) {
			curX += x; 
			curY += y; 
		}

		public float scaleX = 1, scaleY = 1; 
		public void ScaleCTM(float x, float y) {
			scaleX = x; 
			scaleY = y; 
		}

		public float LineWidth = 1; 
		public void SetLineWidth(float w) {
			LineWidth = w; 
		}

		public List<CGPath> Paths = null; 
		public void AddPath(CGPath p) {
			if (Paths == null) {
				Paths = new List<CGPath> (); 

			} 
			Paths.Add (p); 
		}

		public void DrawPath(CGPathDrawingMode c) 
		{
			Canvas canvas = ((UIConnectingView)this.CurrentView).CurrentCanvas; 
			if (canvas == null)
				return; 

			if (Paths != null) {
				Paint paint = new Paint (); 

				switch (c) {
				case CGPathDrawingMode.FillStroke:
					{
						paint.SetStyle (Paint.Style.FillAndStroke); 
						break; 
					}
				}

				paint.Color = this.StrokeColor; 

				var scale = UIViewController.Scale;
				float sx = scale * this.scaleX; 
				float sy = scale * this.scaleY; 

				//sx = sy = 1; 

				//Console.WriteLine ("{0},{1},{2},{3},{4}", this.CurrentView.GetX(), this.CurrentView.GetY(), this.StrokeColor.R, this.StrokeColor.G, this.StrokeColor.B); 
				paint.StrokeWidth = LineWidth; 
				//paint.PathEffect = null; 

				foreach (var p in Paths) {
					foreach (var sp in p.Paths) {

						if (sp.Count == 0)
							continue; 


						Path path = new Path (); 
						//Console.WriteLine ("{0},{1}", sp [0].X, sp [1].Y); 
						path.MoveTo (sp [0].X * sx, sp [0].Y* sy); 
						foreach (var _p in sp) {
							path.LineTo (_p.X* sx, _p.Y* sy); 
						}
						//path.LineTo(sp [0].X * sx, sp [0].Y* sy);

						path.Close (); 

						canvas.DrawPath (path, paint); 
					}
				}
				Paths = null; 

			}
		}

	}

	public class CoreGraphicsStuba
	{
		public CoreGraphicsStuba ()
		{
		}
	}
}


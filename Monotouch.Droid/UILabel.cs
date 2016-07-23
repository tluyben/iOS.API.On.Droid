using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;
using Android.Text;

using System.Drawing;

using Foundation;

namespace UIKit
{
	public class NSMutableParagraphStyle
	{
		public nfloat LineSpacing {get; set;}
		public UITextAlignment Alignment {get; set;}
	}
	public class UIStringAttributes {
		public UIColor ForegroundColor {get; set; }
		public NSMutableParagraphStyle ParagraphStyle {get; set; }
			
	}

	public enum UITextAlignment {
		Left,
		Right,
		Center
	}
	public enum UILineBreakMode {
		Wrap, WordWrap
	}

	public class AndroidLabel : TextView, UIConnectingView
	{
		public UIView UIOwnerView { get; set;}

		public AndroidLabel(Context c) : base(c)
		{
			
		}

		private Canvas _CurrentCanvas; 
		public Canvas CurrentCanvas {
			get {
				return _CurrentCanvas;
			}
		}

		public Action _BaseDrawAction; 
		public Action BaseDrawAction { get{ return _BaseDrawAction; } } 

		protected override void OnDraw (Canvas canvas)
		{
			this._CurrentCanvas = canvas; 
			UIGraphics._CurrentContext = new CoreGraphics.CGContext(this); 

			_BaseDrawAction = () => {
				base.OnDraw (canvas);
			}; 

			UIOwnerView.Draw (new System.Drawing.RectangleF (
				UIOwnerView.Frame.X, 
				UIOwnerView.Frame.Y,
				UIOwnerView.Frame.Width,
				UIOwnerView.Frame.Height
			)); 
		}
	}


	public class UILabel : UIView
	{

		public string Text {
			get {
				return ((AndroidLabel)__View).Text; 

			} 
			set {
				((AndroidLabel)__View).Text = value; 
			}
		}

		public UILabel (CoreGraphics.CGRect rect) : base(new AndroidLabel (UIViewController.Context)) {
			this.Frame = rect; 
		}

		public UILabel () : base(new AndroidLabel (UIViewController.Context))
		{


		}


		protected NSAttributedString _AttributedText; 
		public NSAttributedString AttributedText {
			get {
				return _AttributedText; 
			}
			set {

			}
		}

		public virtual UIViewAutoresizing AutoresizingMask { get; set;}

		public UIColor TextColor {

			set{
				((AndroidLabel)__View).SetTextColor (value.CGColor); 
			}
		}

		public UIColor ShadowColor {

			set{
				// TODO: ???
				//this.SetShadowLayer
			}
		}

		protected UITextAlignment _TextAlignment = UITextAlignment.Center; 
		public UITextAlignment TextAlignment 
		{
			get {
				/*switch (((AndroidLabel)__View).Gravity) {
				case GravityFlags.Left: 
					return UITextAlignment.Left; 
					break;
				case GravityFlags.Right: 
					return UITextAlignment.Right; 
					break;
				case GravityFlags.Center: 
					return UITextAlignment.Center; 
					break;
				}*/ 
				return _TextAlignment; 
			}

			set {
				switch (value) {
				case UITextAlignment.Left:
					((AndroidLabel)__View).Gravity = GravityFlags.Left;
					break;
				case UITextAlignment.Right:
					((AndroidLabel)__View).Gravity = GravityFlags.Right;
					break;
				case UITextAlignment.Center:
					((AndroidLabel)__View).Gravity = GravityFlags.Center;
					break;
				}
				_TextAlignment = value; 
			}
		}

		bool _AdjustsFontSizeToFitWidth = false; 
		public bool AdjustsFontSizeToFitWidth {
			get {
				return _AdjustsFontSizeToFitWidth;
			} 
			set {
				_AdjustsFontSizeToFitWidth = value;
			}
		}

		int _Lines = 1; 
		public int Lines {
			get {
				return _Lines; 
			}
			set {
				_Lines = value; 
			}
		}

		private UIFont _Font; 
		public UIFont Font {
			get {
				if (_Font == null) {
					_Font = new UIFont (); 
				}
				return _Font; 
			}
			set {
				_Font = value; 
				((AndroidLabel)__View).Typeface = value.Font; 
				((AndroidLabel)__View).SetTextSize (ComplexUnitType.Dip, value.Size ); 
			}
		}

		public UILineBreakMode LineBreakMode = UILineBreakMode.Wrap; 

		// TODO: revisit!


		public static System.Drawing.SizeF SStringSize(string Text, UIFont Font, System.Drawing.SizeF Max, UILineBreakMode lbr) {

			if (Text.Length == 0 ) {
				return new System.Drawing.SizeF (0, 0); 
			}

			TextPaint p = new TextPaint (); 
			p.SetTypeface (Font.Font); 


			float d = UIViewController.Density; 
			p.TextSize = Font.Size; 




			//StaticLayout sl = new StaticLayout (Text, p, (int)UIScreen.MainScreen.Bounds.Width, Layout.Alignment.AlignNormal, 1.0f, 0.0f, false); 

			//return new System.Drawing.SizeF (sl.Width, sl.Height);

			var bounds = new Rect ();

			p.GetTextBounds (Text, 0, Text.Length, bounds); 
			float w = p.MeasureText (Text); 
			return new System.Drawing.SizeF ((float)(bounds.Width()), (float)(bounds.Height()));
		}

		public static System.Drawing.SizeF GetStringSize(string Text, UIFont Font, System.Drawing.SizeF Max, UILineBreakMode lbr) {
			return new System.Drawing.SizeF (Font.Size/2.11134011332f, Font.Size/0.8178913738f); 
		}

		public System.Drawing.SizeF StringSize(string Text, UIFont Font, System.Drawing.SizeF Max, UILineBreakMode lbr) {



			/*var s = SStringSize (Text, Font, Max, lbr); 
			float margin = 10f; 
			return s.Width == 0 ? s : new System.Drawing.SizeF (s.Width+margin, s.Height+margin); */

			// TODO: magic :( 
			return new System.Drawing.SizeF (Font.Size/2.11134011332f, Font.Size/0.8178913738f); 
		}


	}

}


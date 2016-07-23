using System;

using Foundation; 
using CoreGraphics; 
using UIKit;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics; 

namespace CoreText
{
	public class CTLine
	{
		NSAttributedString Str; 

		public CTLine(NSAttributedString str) {
			this.Str = str; 
		}

		public void Draw (CGContext cg) {

			Paint paint = new Paint (); 
			paint.SetTypeface (Str.Attr.Font._Font.Font); 

			var scale = UIViewController.Scale; 

			paint.TextSize = Str.Attr.Font._Font.Size*scale; 

			if (Str.Attr.ForegroundColorFromContext) {
				paint.Color = cg.FillColor; 
			}

			Canvas c = ((UIConnectingView)cg.CurrentView).CurrentCanvas; 
			// iOS draws everything upside-down so -1 ... 
			float x = cg.curX * cg.scaleX * scale; 
			float y = cg.curY * cg.scaleY * -1 * scale; 
			c.DrawText (Str.Text, x, y, paint); 

			var size = UILabel.GetStringSize (Str.Text, 
				Str.Attr.Font._Font, new System.Drawing.SizeF (float.MaxValue, float.MaxValue), UILineBreakMode.Wrap);

			cg.TranslateCTM (size.Width, 0);

		}
	}
	public class CTFont 
	{
		public UIFont _Font; 

		public CTFont(string name, float size) {
			_Font = UIFont.FromName (name, size); 
		}
	}
	public class CTStringAttributes
	{
		public bool ForegroundColorFromContext; 
		public CTFont Font; 
	}

	public class CoreTextStub
	{
		public CoreTextStub ()
		{
		}
	}
}


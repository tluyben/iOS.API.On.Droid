using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;

namespace UIKit
{
	public class UIColor 
	{
		public static UIColor White = new UIColor(Color.White);
		public static UIColor Black = new UIColor(Color.Black);
		public static UIColor LightGray = new UIColor(Color.LightGray); 
		public static UIColor DarkGray = new UIColor(Color.DarkGray); 
		public static UIColor Yellow = new UIColor (new Color (255, 255, 0)); 
		public static UIColor Red = new UIColor (new Color (255, 0, 0)); 
		public static UIColor Green = new UIColor (new Color (0, 255, 0)); 
		public static UIColor Blue = new UIColor (new Color (0, 0, 255)); 
		public static UIColor Transparant = new UIColor(Color.Transparent); 
		public static UIColor Clear = new UIColor(Color.Transparent); 

		public UIColor ()
		{

		}

		public void SetFill() {
			UIGraphics.GetCurrentContext ().SetFillColor (this.CGColor); 
		}
		public void SetStroke() {
			UIGraphics.GetCurrentContext ().SetStrokeColor (this.CGColor); 
		}

		Color _CGColor; 
		public Color CGColor {
			get {
				return _CGColor; 
			} 
			set {
				_CGColor = value; 
			}
		}


		public UIColor(Color c) {
			this.CGColor = c; 
		}

		public static UIColor FromRGB(byte r, byte g, byte b) 
		{
			return new UIColor(new Color (r, g, b)); 
		}
		public static UIColor FromRGBA(byte r, byte g, byte b, byte alpha) 
		{
			return new UIColor(new Color (r, g, b,alpha)); 
		}
		public static UIColor FromRGBA(float r, float g, float b, float alpha) 
		{
			return new UIColor(new Color ((byte)(r*255), (byte)(g*255), 
				(byte)(b*255),(byte)(alpha*255)));
		}

		public static UIColor FromPatternImage(UIImage im) {
			return new UIColor (); 
		}

		public static UIColor FromWhiteAlpha( float white, float alpha) {
			return FromRGBA (white, white, white, alpha); 
		}
	}
}


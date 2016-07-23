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

namespace UIKit
{
	public class UIFont
	{
		public Typeface Font; 
		public float Size;

		public UIFont ()
		{
			Font = Typeface.SansSerif; 
			Size = 14f; 
		}

		public static UIFont SystemFontOfSize(float s) {
			UIFont f = new UIFont (); 
			f.Font = Typeface.DefaultFromStyle (TypefaceStyle.Normal);
			f.Size = s; 
			return f; 
		}

		public UIFont WithSize(float s) {
			UIFont f = new UIFont (); 
			f.Font = Font; 
			f.Size = s; 
			return f; 
		}

		public static UIFont FromName(string n, float s) {
			UIFont f = new UIFont (); 

			string _n = n.ToUpper ().Replace ('_', ' ').Replace ('-', ' ').Replace(" ", string.Empty); 
			//string _n1 = _n.Replace (" ", string.Empty); 
			bool asset = false; 
			foreach (var af in UIViewController.Context.Assets.List("")) {
				var _af = af.ToUpper ().Trim().Replace ('_', ' ').Replace ('-', ' ').Replace(" ", string.Empty); 

				if (_af.EndsWith (".OTF") || _af.EndsWith (".TTF")) {
					if (_af.StartsWith (_n)) {
						n = af; 
						asset = true; 
						break; 
					}
				}
			}

			if (asset) {
				f.Font = Typeface.CreateFromAsset (UIViewController.Context.Assets, n); 
			} else {

				f.Font = Typeface.CreateFromFile (n); 
			}
			f.Size = s; 
			//f.Font = Typeface.
			return f; 
		}
	}
}


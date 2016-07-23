using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;
using Android.Webkit;

using Foundation; 
using System.Collections.Generic;
using CoreGraphics;

namespace UIKit
{
	public enum UIDeviceOrientation
	{
		Unknown,
		Portrait,
		PortraitUpsideDown,
		LandscapeLeft,
		LandscapeRight,
		FaceUp,
		FaceDown
	}

	public class UIWindow
	{
		bool Visible = false; 
		public UIViewController RootViewController { get; set;}

		#if UNIVERSALAPI
		public UIWindow (CGRect Bounds)

		#else 
		public UIWindow (System.Drawing.RectangleF Bounds)
		#endif
		{


		}

		UIColor _BackgroundColor; 
		public UIColor BackgroundColor {
			get {
				return  _BackgroundColor; 
			}
			set{
				_BackgroundColor = value; 
				//this.SetBackgroundColor (value.CGColor); 

			}
		}


		public void MakeKeyAndVisible() {

			Visible = true; 


			if (RootViewController != null) {

				RootViewController.RememberMe (); 
				Intent i = new Intent (UIViewController.Context, RootViewController.GetType ()); 
				UIViewController.Context.StartActivity (i); 

			}
		}
	}
}


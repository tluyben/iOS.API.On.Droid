using System;
using CoreGraphics; 
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
	public static class UIGraphics
	{
		public static CGContext _CurrentContext { get; set; }
		public static CGContext GetCurrentContext() {
			return _CurrentContext;
		}

		public static void BeginImageContextWithOptions (CGSize size, bool opaque, nfloat scale) {

		}

		public static UIImage GetImageFromCurrentImageContext() {
			return null; 
		}

		public static void EndImageContext() {

		}
	}
}


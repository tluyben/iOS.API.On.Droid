using System;
using System.Drawing; 
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using CoreGraphics; 

namespace UIKit
{
	public class UIScreen
	{
		public static UIScreen MainScreen { 
			get;
			set;
		}

		#if UNIVERSALAPI
		public virtual nfloat Scale {
			get {
				return 1; 
			}
		}
		#endif
		#if UNIVERSALAPI
		public CGRect Bounds { 
		#else 
	
		public RectangleF Bounds { 
		#endif
			get;
			set;
		
		} 

		public UIScreen ()
		{
		}
	}
}


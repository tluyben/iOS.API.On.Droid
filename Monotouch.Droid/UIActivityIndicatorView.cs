using System;
using System.Drawing; 
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;

using Foundation; 


namespace UIKit
{
	public enum UIActivityIndicatorViewStyle
	{
		WhiteLarge
	}

	public class UIActivityIndicatorView : UIView
	{

		UIActivityIndicatorViewStyle ActivityIndicatorViewStyle; 

		public UIActivityIndicatorView(UIActivityIndicatorViewStyle ivs) 
		{
			ActivityIndicatorViewStyle = ivs; 
		}

		public bool IsAnimating {
			get {
				return false; 
			}
		}

		public void StartAnimating() {

		}
		public void StopAnimating() {

		}
	}


}


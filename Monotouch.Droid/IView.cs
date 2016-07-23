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
using CoreGraphics; 

namespace UIKit
{
	public class UIEvent
	{

	}

	public interface IView
	{


		UIColor BackgroundColor { get;set;}

		#if UNIVERSALAPI
		CGRect Frame {get;set;}
		#else 
		System.Drawing.RectangleF Frame {get;set;}
		#endif 

		System.Drawing.SizeF Size {get; set; }
		bool Hidden { get; set; }
		void Add (UIView View); 
		void AddSubview(UIView View); 
		// void SizeToFit(); !
		UIViewAutoresizing AutoresizingMask { get; set;}

		// internals, only for Android
		IView GParent { get; set; }
		List<UIView> GChildren { get; }
		void LayoutChildren(float x, float y); 

		#if UNIVERSALAPI
		CGRect AbsFrame {set; }
		#else
		System.Drawing.RectangleF AbsFrame {  set; }
		#endif

		View AndroidView {get;}

	}
}


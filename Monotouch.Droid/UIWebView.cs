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

namespace UIKit
{
	public enum UIWebViewNavigationType
	{
		LinkClicked,
		FormSubmitted,
		BackForward,
		Reload,
		FormResubmitted,
		Other
	}

	public class UIWebViewDelegate
	{
		public virtual void LoadStarted (UIWebView webView)
		{
		}

		public virtual void LoadingFinished (UIWebView webView)
		{
		}
	}

	public class AndroidWebView : WebView, UIConnectingView
	{
		public UIView UIOwnerView { get; set;}

		public AndroidWebView(Context c) : base(c)
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

	public class UIWebView : UIView
	{
		class _WebViewClient : WebViewClient 
		{
			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				view.LoadUrl (url); 
				return false; 
			}
		}

		public UIWebView () : base(new AndroidWebView (UIViewController.Context))
		{

			((AndroidWebView)__View).Settings.JavaScriptEnabled = true; 
			((AndroidWebView)__View).SetWebViewClient (new _WebViewClient ()); 

		}



	
		public void LoadRequest(NSUrlRequest url) {
			this.LoadFinished += (object sender, EventArgs e) => {
				if (ScalesPageToFit) {
					((AndroidWebView)__View).Settings.LoadWithOverviewMode = true;
					((AndroidWebView)__View).Settings.UseWideViewPort = true;
				}
			};
			((AndroidWebView)__View).LoadUrl (url.Url.Url); 

		
		}

		public virtual bool ScalesPageToFit { get; set;}

		/*public virtual bool ScalesPageToFit() 
		{
			Display display = ((WindowManager) UIViewController.Context.getSystemService(Context.WINDOW_SERVICE)).getDefaultDisplay(); 
			int width = display.getWidth(); 
			Double val = new Double(width)/new Double(PIC_WIDTH);
			val = val * 100d;
			this.SetPadding(0, 0, 0, 0);
			this.SetInitialScale((int)val);
			return true; 
		}*/

		public event EventHandler LoadStarted {
			add {

			}
			remove {

			}
		}

		public event EventHandler LoadFinished {
			add {

			}
			remove {

			}
		}

		public new bool CanGoBack {
			get {
				return ((AndroidWebView)__View).CanGoBack ();
			}

		}
		public new bool CanGoForward {
			get {
				return ((AndroidWebView)__View).CanGoForward ();
			}

		}

	}
}


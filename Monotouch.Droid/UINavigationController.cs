using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;

using System.Collections.Generic; 

namespace UIKit
{


	public class UINavigationItem 
	{
		private UIBarButtonItem _RightBarButtonItem; 
		public UIBarButtonItem RightBarButtonItem {
			get {
				return _RightBarButtonItem;
			}
			set {
				_RightBarButtonItem = value; 
			}
		}
	}

	public class UINavigationBar
	{
		public UINavigationBar() {
			// this is always 0 :)
			float h = UIViewController.Context.ActionBar != null ? UIViewController.Context.ActionBar.Height : 0; 
			Frame = new System.Drawing.RectangleF (0, 0, UIScreen.MainScreen.Bounds.Width, h); 
		}


		public System.Drawing.RectangleF Frame {get;set;}
		public System.Drawing.SizeF Size {
			get {
				return new System.Drawing.SizeF (Frame.Width, Frame.Height); 
			}
			set {
				Frame = new System.Drawing.RectangleF (
					Frame.X,
					Frame.Y,
					value.Width,
					value.Height
				); 
			}
		}

	}


	#if __ANDROID__
	[Android.App.Activity (Label = "MonoTouch.UIKit.UINavigationController", MainLauncher = false, Theme="@android:style/Theme.NoTitleBar")]
	#endif
	public class UINavigationController : UIViewController
	{
		private Foundation.NSObject _Delegate; 
		public virtual Foundation.NSObject Delegate
		{
			get { return _Delegate; }
			set { _Delegate = value; }
		}
		protected Stack<UIViewController> Controllers = new Stack<UIViewController> (); 
		public bool Presented { get; set; }

		private bool _NavigationBarHidden = false ;
		public bool NavigationBarHidden {
			get {
				return _NavigationBarHidden; 
			}
			set {
				if (value) {
					UIViewController.Context.ActionBar.Hide (); 
				} else {
					UIViewController.Context.ActionBar.Show (); 
				}
				_NavigationBarHidden = value; 
			}
		}

		public UINavigationController()
		{
			Presented = false; 

			this.NavigationBar = new UINavigationBar (); 
			//this.NavigationItem = new UINavigationItem (); 
		}
		public UINavigationController(UIViewController viewController) 
		{
			// TODO: what happens with this? Push? 
			this.PushViewController(viewController, true); 
		}

		public void _PrepActionBar(UIViewController Context) {

			Context.ActionBar.SetDisplayShowTitleEnabled (true); 
			Context.ActionBar.Show (); 

			this.NavigationBar = new UINavigationBar ();
		}

		public UINavigationBar NavigationBar { get; set; }

		protected override void OnCreate (Bundle bundle)
		{
			if (MyInstance != null) {
				var me = (UINavigationController)MyInstance; 
				this.Controllers = me.Controllers;
			}
				
			base.OnCreate (bundle);

			Presented = true; 

			// now that this one is going, present it
			if (Controllers.Count > 0) {
				PushViewController (Controllers.Pop (), true); 
			}
		}


		public void SetNavigationBarHidden (bool hidden, bool animated) {

		}

		public void PushViewController(UIViewController vc, bool animated)
		{
			vc.NavigationController = this; 
			vc.NavigationItem = new UINavigationItem (); 
			Controllers.Push (vc); 



			if (Presented) {

				vc.RememberMe (); 
				Intent i = new Intent (UIViewController.Context, vc.GetType ()); 
				UIViewController.Context.StartActivity (i); 

			}
		}

		public void PopToRootViewController(bool animated)
		{
			while (Controllers.Count > 1)
				Controllers.Pop (); 
			PopViewControllerAnimated (animated); 
		}

		public void PopViewController(bool animated) 
		{
			PopViewControllerAnimated (animated); 
		}


		public void PopViewControllerAnimated(bool animated) 
		{
			Controllers.Pop ().Finish(); 

			//PushViewController (Controllers.Pop (), true); 
		}
	}
}


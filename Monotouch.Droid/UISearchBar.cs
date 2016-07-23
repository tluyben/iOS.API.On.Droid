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
	public class UISearchBarTextChangedEventArgs : EventArgs 
	{
		public string Text {get;set;}
		public  UISearchBarTextChangedEventArgs(string t) {
			Text = t ; 
		}
	}

	public class AndroidSearchView : SearchView , UIConnectingView
	{
		public UIView UIOwnerView { get; set;}

		public AndroidSearchView(Context c): base(c) 
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

	public class UISearchBar : UIView
	{

		public UISearchBar () : base(new AndroidSearchView (UIViewController.Context))
		{


		}

		private string _Placeholder; 
		public string Placeholder {
			get {
				return _Placeholder; 
			}

			set {
				_Placeholder = value; 
				((AndroidSearchView)__View).SetQueryHint (value); 
			}
		}
		protected bool _IsFirstResponder = false; 
		public bool IsFirstResponder  {
			get {
				return _IsFirstResponder; 
			}
		}
		public event EventHandler SearchButtonClicked {
			add {

			}
			remove {

			}
		
		}

		public string Text {
			get {
				return ((AndroidSearchView)__View).Query; 
			}
			set {
				((AndroidSearchView)__View).SetQuery (value, false); 
			}
		}

		public event EventHandler<UISearchBarTextChangedEventArgs> TextChanged {
			add {
				/*base.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {

					value(sender, new UISearchBarTextChangedEventArgs(e.Text.ToString())); 
				};*/

				((AndroidSearchView)__View).QueryTextChange += ( sender,  e) => {
					value(sender, new UISearchBarTextChangedEventArgs(e.NewText.ToString())); 
				};
			}
			remove {

			}
		}
	}
}


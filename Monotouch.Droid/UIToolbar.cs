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
	public enum UIBarButtonItemStyle 
	{
		Bordered
	}
	public enum UIBarButtonSystemItem
	{
		FlexibleSpace, Refresh, Stop
	}

	public class UIBarButtonItem : UIButton
	{
		public EventHandler ClickHandler; 

		public event EventHandler Clicked {
			add {
				((AndroidButton)__View).Click += value; 
				ClickHandler += value; 
			}
			remove {
				((AndroidButton)__View).Click -= value; 
			}
		}

		public UIBarButtonItem() {

		}
		public int ItemId { get; set; }
		public string Title; 
		public UIBarButtonItem(string title, UIBarButtonItemStyle style, EventHandler action) {
			this.Title = title; 
		}

		public UIBarButtonItem(UIBarButtonSystemItem item, EventHandler action) {

		}
	}

	public class UIToolbar : UIView
	{
		public UIColor TintColor {get; set;}

		public UIBarButtonItem[] Items {get; set;}
	}
}


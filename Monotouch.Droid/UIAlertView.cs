using System;
using System.Drawing; 
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;

namespace UIKit
{

	public class UIAlertViewDelegate
	{
		public virtual void Clicked (UIAlertView alertview, int buttonIndex)
		{
			alertview._GenerateEvent (buttonIndex); 
		}

		public virtual void Clicked (UIAlertView alertview, nint buttonIndex)
		{
			alertview._GenerateEvent (buttonIndex); 
		}

		public virtual void Canceled (UIAlertView alertView)
		{

		}

		public virtual void Dismissed (UIAlertView alertView, nint buttonIndex)
		{
			alertView._GenerateEvent (buttonIndex); 
		}

	}
	public class UIAlertView : AlertDialog.Builder
	{
		public UIAlertViewDelegate Delegate { get; set; }

		public UIAlertView () : base(UIViewController.Context)
		{
			//_Layout = ((UIViewController)this.Context).Layout; 

		}

		// pffffff delegates... 
		public void _GenerateEvent(int buttonIndex) {
			if (Dismissed != null) {
				Dismissed (this, 
					new UIButtonEventArgs {
						ButtonIndex = buttonIndex
					}
				); 
			}
		}

		public event EventHandler<UIButtonEventArgs> Dismissed;

		private void _Init(string title,string description,UIAlertViewDelegate _delegate , string buttonTitle, string secondButton)
		{
			Delegate = _delegate; 

			SetTitle (title); 
			SetMessage (description); 

			SetPositiveButton (buttonTitle, (s, ev) => {
				if (Delegate != null) {
					Delegate.Clicked(this, 0); 
				}
				_GenerateEvent(0); 
			}); 

			if (secondButton != null) {
				AddButton (secondButton); 
			}
		}

		public int CancelButtonIndex = 0; 


		//public UIAlertView (string title, string message, UIAlertViewDelegate del, string cancelButtonTitle, params string[] otherButtons) : this (title, message, del, cancelButtonTitle, (otherButtons != null && otherButtons.Length != 0) ? otherButtons [0] : null, null)
		public UIAlertView (string title,string description,UIAlertViewDelegate _delegate , string buttonTitle, string secondButton) : base(UIViewController.Context)
		{
			_Init (title, description, _delegate, buttonTitle, secondButton); 
		}
		public UIAlertView (string title,string description,UIAlertViewDelegate _delegate , string buttonTitle) : base(UIViewController.Context)
		{

			_Init (title, description, _delegate, buttonTitle, null); 

		}

		//object sender2, UIButtonEventArgs e

		public event EventHandler<UIButtonEventArgs> Clicked {
			add {
				Dismissed += value; 
			}
			remove {
				Dismissed -= value; 
			}
		}

		public void DismissWithClickedButtonIndex(nint index, bool animated)
		{
			Delegate.Clicked (this, index); 
		}


		// TODO: fix, not sure how that works with iOS
		public void AddButton(string title) {
			SetNegativeButton (title, (s, ev) => {
				if (Delegate != null) {
					Delegate.Clicked(this, 1); 

				}
				_GenerateEvent(1); 
			}); 
		}
	}
}


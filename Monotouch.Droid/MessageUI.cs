using System;
using System.Drawing; 
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;

using System.Linq; 
using UIKit;

namespace MessageUI
{

	public interface _MFComposeViewController : IInternalIntent
	{
	}

	public class MFComposeResultEventArgs : EventArgs 
	{

	}

	public class MFMailComposeViewController : UIViewController, _MFComposeViewController
	{

		string[] Recipients; 
		string Subject; 
		string Body; 
		bool IsHtml = false; 
		public void SetToRecipients(string[] rc)
		{
			Recipients = rc; 
		}
		public void SetSubject(string s)
		{
			Subject = s; 
		}
		public void SetMessageBody(string s, bool isHtml) 
		{
			Body = s; 
			IsHtml = isHtml; 
		}

		public event EventHandler<MFComposeResultEventArgs> Finished 
		{
			add {

			} 
			remove {

			}
		}

		public void DoIntent() {
			Intent emailIntent = new Intent(Android.Content.Intent.ActionSend);
			emailIntent.SetType ("plain/text"); 
			emailIntent.PutExtra (Android.Content.Intent.ExtraEmail, Recipients); 
			emailIntent.PutExtra (Android.Content.Intent.ExtraSubject, Subject); 
			emailIntent.PutExtra (Android.Content.Intent.ExtraText, Body); 
			UIViewController.Context.StartActivity (Intent.CreateChooser (emailIntent, "Share via email...")); 
		}

	}

	#if __ANDROID__
	//[Android.App.Activity (Label = "Monotouch.Droid.MFMessageComposeViewController", MainLauncher = false)]
	#endif
	public class MFMessageComposeViewController : UIViewController, _MFComposeViewController
	{
	

		public MFMessageComposeViewControllerDelegate MessageComposeDelegate {get; set;}
		public string Body {get; set;}
		public string[] Recipients {get; set;}

		public void DoIntent() {
			Intent smsIntent = new Intent(Intent.ActionView);
			smsIntent.SetType("vnd.android-dir/mms-sms");
			if (Recipients!=null) smsIntent.PutExtra("address", String.Join(";", Recipients));
			if (Body!=null) smsIntent.PutExtra("sms_body",Body);
			UIViewController.Context.StartActivity(smsIntent);
		}

	}

	public class MFMessageComposeViewControllerDelegate {
		public virtual void Finished (MFMessageComposeViewController controller, MessageComposeResult result)
		{

		}
	}


	public class MessageComposeResult {

	}
}


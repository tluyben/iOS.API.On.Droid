using System;
using System.Drawing; 
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;

using Foundation;

namespace UIKit
{

	public enum UIUserInterfaceIdiom
	{
		Phone 
	}
	public class UIDevice 
	{
		public static UIDevice CurrentDevice {
			get {
				return new UIDevice (); 
			}
		}
		public UIUserInterfaceIdiom UserInterfaceIdiom 
		{
			get {
				return UIUserInterfaceIdiom.Phone; 
			}
		}
	}

	public class UIApplicationDelegate : UIViewController, NSObject 
	{
		protected override void OnCreate (Bundle bundle)
		{
			//this.ActionBar.Hide (); 

			base.OnCreate (bundle);

			UIImage im = UIImage.FromFile ("splash.png");
			if (im != null) {
				Matrix matrix = new Matrix ();
				matrix.PostScale (Scale, Scale); 
				Bitmap scaled = Bitmap.CreateBitmap (im.Bitmap, 0, 0, im.Bitmap.Width, im.Bitmap.Height, matrix, true); 
				UIImageView iv = new UIImageView (); 
				iv.Frame = new RectangleF (0, 0, UIScreen.MainScreen.Bounds.Width, 
					UIScreen.MainScreen.Bounds.Height); 
				iv.SetImageBitmap (scaled); 
				Add (iv); 
			}

			FinishedLaunching (new UIApplication (), new NSDictionary ()); 
		}

		public virtual bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			return false;
		}

		public virtual void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{

		}

		public virtual void FailedToRegisterForRemoteNotifications (UIApplication application , NSError error)
		{

		}

		public virtual void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
		{

		}

		public virtual void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings) {

		}
		public virtual void HandleAction (UIApplication application, string actionIdentifier, NSDictionary remoteNotificationInfo, Action completionHandler) {

		}

		public virtual void DidReceiveRemoteNotification (UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{

		}
		public virtual bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			return true;
		}

		public virtual void OnResignActivation (UIApplication application)
		{

		}
	}

	public class UIApplication
	{
		//UIApplication.SharedApplication.StatusBarFrame.Size.Height

		//UIApplication.Main (args, null, "AppDelegate");
		public static void Main(string[] args, string pclass, string dclass) {

		}

		public UIInterfaceOrientation StatusBarOrientation { get ; set ; }

		public static UIApplication SharedApplication = new UIApplication(); 
		public RectangleF StatusBarFrame = new RectangleF (0, 0, 0, 0); 
		public bool NetworkActivityIndicatorVisible {get; set;}
		public UIApplication ()
		{
			 



		}

		public static string LaunchOptionsRemoteNotificationKey{
			get { return "key"; }
		}
	}
}


//using ArelloMobile.Push; 
using Foundation; 

namespace Pushwoosh 

{
	public class PushNotificationManager
	{
		public static PushNotificationManager PushManager
		{
			get { return null; }
		}

		public string AppCode {get;set;}
		public NSObject Delegate {get;set;}

		public void HandlePushReceived(NSDictionary d) {

		}

		public void RegisterForPushNotifications() {

		}

		public string GetPushToken {
			get { return ""; }
		}

		public void HandlePushRegistration(NSData token) {

		}
		public void HandlePushRegistrationFailure(NSError err) {

		}

		public static void ClearNotificationCenter() {

		}
	}
}
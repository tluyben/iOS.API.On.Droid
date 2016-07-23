using Foundation; 

namespace CoreFoundation
{
	public class CFRunLoop
	{
		public static CFRunLoop Current {get {return new CFRunLoop(); } }
		public static NSString ModeDefault { get { return new NSString(""); } }
	}

}


using CoreFoundation; 
using Foundation; 

namespace SystemConfiguration
{
	public enum StatusCode
	{
		OK,
		Failed = 1001,
		InvalidArgument,
		AccessError,
		NoKey,
		KeyExists,
		Locked,
		NeedLock,
		NoStoreSession = 2001,
		NoStoreServer,
		NotifierActive,
		NoPrefsSession = 3001,
		PrefsBusy,
		NoConfigFile,
		NoLink,
		Stale,
		MaxLink,
		ReachabilityUnknown = 4001,
		ConnectionNoService = 5001,
		ConnectionIgnore
	}

	//[Flags]
	public enum NetworkReachabilityFlags
	{
		TransientConnection = 1,
		Reachable = 2,
		ConnectionRequired = 4,
		ConnectionOnTraffic = 8,
		InterventionRequired = 16,
		ConnectionOnDemand = 32,
		IsLocalAddress = 65536,
		IsDirect = 131072,
		IsWWAN = 262144,
		ConnectionAutomatic = 8
	}
	public class NetworkReachability : System.IDisposable 
	{

		public StatusCode SetNotification (NetworkReachability.Notification callback) 
		{
			return StatusCode.OK; 
		}

		public  NetworkReachability(string host) {
		
		}
		public  NetworkReachability(System.Net.IPAddress ip) {

		}
		public bool TryGetFlags(out NetworkReachabilityFlags flags) {
			flags = NetworkReachabilityFlags.Reachable; 
			return false;
		}
		public delegate void Notification(NetworkReachabilityFlags flags); 

		public void SetCallback(Notification cb) {

		}
		public void Schedule(CFRunLoop loop, NSString mode) {

		}
		public void Dispose() {

		}
	}
}
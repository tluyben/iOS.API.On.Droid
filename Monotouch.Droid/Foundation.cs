using System;
using System.Collections.Generic;
using System.Linq;
using UIKit; 
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using CoreText; 
using CoreGraphics; 
using Android.Graphics; 

namespace Foundation
{

	public enum NSSearchPathDomain
	{
		None = 0,
		User = 1,
		Local = 2,
		Network = 4,
		System = 8,
		All = 65535
	}

	public enum NSSearchPathDirectory
	{
		ApplicationDirectory = 1,
		DemoApplicationDirectory,
		DeveloperApplicationDirectory,
		AdminApplicationDirectory,
		LibraryDirectory,
		DeveloperDirectory,
		UserDirectory,
		DocumentationDirectory,
		DocumentDirectory,
		CoreServiceDirectory,
		AutosavedInformationDirectory,
		DesktopDirectory,
		CachesDirectory,
		ApplicationSupportDirectory,
		DownloadsDirectory,
		InputMethodsDirectory,
		MoviesDirectory,
		MusicDirectory,
		PicturesDirectory,
		PrinterDescriptionDirectory,
		SharedPublicDirectory,
		PreferencePanesDirectory,
		ApplicationScriptsDirectory,
		ItemReplacementDirectory = 99,
		AllApplicationsDirectory,
		AllLibrariesDirectory,
		TrashDirectory
	}

	public class NSFileManager
	{
		public static NSFileManager DefaultManager
		{
			get { return new NSFileManager (); }

		}

		public NSUrl[] GetUrls(NSSearchPathDirectory dir, NSSearchPathDomain dom) {
			return new NSUrl[]{new NSUrl ("")}; 
		}
	}

	public class Outlet : Attribute 
	{

	}

	public class NSNotificationCenter : NSObject
	{
		protected static NSNotificationCenter _DefaultCenter; 
		public static NSNotificationCenter DefaultCenter {
			get {
				if (_DefaultCenter == null) {
					_DefaultCenter = new NSNotificationCenter (); 
				}
				return _DefaultCenter; 
			}
		}


		public void PostNotificationName(string name, NSSet touches)
		{
		}
	}
	public class NSMutableAttributedString : NSAttributedString 
	{
		public NSMutableAttributedString(string text) : base(text) {
			
		}

		public void AddAttributes(UIStringAttributes attr, NSRange range) {
			this.UIAttr = attr; 
			this.Range = range; 
		}

	

	}
	public class NSAttributedString 
	{

		public string Text; 
		public CTStringAttributes Attr; 
		public UIStringAttributes UIAttr; 
		public UIFont Font; 
		public float Kerning; 
		public NSRange Range; 

		public NSAttributedString(string text, CTStringAttributes attr) {
			this.Text = text; 
			this.Attr = attr; 
			this.Font = attr.Font._Font; 

		}

		public NSAttributedString(string text, UIStringAttributes attr) {
			this.Text = text; 
			this.UIAttr = attr; 
			//this.Font = attr.Font._Font; 

		}

		public NSAttributedString(string text) {
			this.Text = text; 

		}

		public NSAttributedString(string text, UIFont font, float kerning) {
			this.Text = text; 
			this.Font = font; 
			this.Kerning = kerning; 
		}

	}

	public class NSRange 
	{
		//
		// Static Fields
		//
		public const int NotFound = 2147483647;

		//
		// Fields
		//
		public int Location;

		public int Length;

		//
		// Constructors
		//
		public NSRange (int start, int len)
		{
			this.Location = start;
			this.Length = len;
		}

		//
		// Methods
		//
		public override string ToString ()
		{
			return string.Format ("[Location={0},Length={1}]", this.Location, this.Length);
		}
	}

	//[AttributeUsage (AttributeTargets.Method)]
	public sealed class ActionAttribute : Attribute //: ExportAttribute
	{
		//
		// Constructors
		//
		public ActionAttribute ()
		{
		}

		public ActionAttribute (string selector) //: base (selector)
		{
		}
	}
	/*public class Action : Attribute
	{
		public Action(string a) {

		}
	}*/
	public class Register : Attribute
	{
		public Register(string Del) {

		}
	}
	public class NSSet : NSObject
	{
		List<NSObject> Content = new List<NSObject>(); 
		public NSObject AnyObject {
			get {
				if (Content.Count == 0)
					return null; 
				return Content [0]; 
			}
		}
	}

	public class NSAction
	{
	}
	public class NSError
	{

	}
	public interface NSData
	{

	}
	public interface NSObject 
	{

		//public static void InvokeInBackground (Action action);

	}

	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class ModelAttribute : Attribute
	{
	}
	public class NSDictionary 
	{
		Dictionary<object,object> _internal; 

		public NSDictionary() {
			_internal = new Dictionary<object, object> (); 

		}

		public bool ContainsKey(object k) {
			return _internal.ContainsKey (k); 
		}

		public object ValueForKey(object k) {
			return _internal.ContainsKey (k) ? _internal [k] : null; 
		}

	}

	public class NSLocale : NSObject
	{
		public static string[] PreferredLanguages
		{
			get { return new string[] { "nl" }; }
		}
	}

	public class NSThread : NSObject 
	{
		public static bool IsMain {
			get {
				return Looper.MyLooper () == Looper.MainLooper; 
			}
		}
	}

	// http://forums.xamarin.com/discussion/comment/13869/#Comment_13869

	public class NSStandardUserDefaults : Dictionary<string, object>
	{
		//NSUserDefaults.StandardUserDefaults.SetString (value, SavedUserNameConfigKey);
		public virtual void SetString(string value, string key) 
		{
			this [key] = value; 
		}
		public virtual string StringForKey (string defaultName)
		{
			object _o = this [defaultName];
			if (_o != null) Console.WriteLine (_o.GetType ()); 
			NSString o = (NSString)_o; 
			//Console.WriteLine (o); 
			return (string)o;
		}
		public virtual void SetInt(int value, string key) {
			this [key] = value; 
		}
		public virtual int IntForKey(string key) {
			if (!base.ContainsKey (key)) {
				return default(int); 
			}
			object o = this [key]; 
			if (o == null)
				return default(int); 

			if (o is NSNumber) {
				return (o as NSNumber).IntValue(); 
			}
			return (int)this [key]; 
		}
		public new object this [string key] {
			get {
				if (!base.ContainsKey (key)) {
					return default(string); 
				}
				return base [key]; 
			}
			set {
				if (value != null && value.GetType () == typeof(System.String)) {
					value = new NSString ((string)value); 
				}

				base [key] = value; 
			}
		}

		public NSStandardUserDefaults() {
			this.Clear (); 
			this.Load (); 
		}

		public void Load() {
			var preferences = 
				UIViewController.Context.GetSharedPreferences ("StandardUserDefaults", FileCreationMode.Private); 
			var a = preferences.All; 
			foreach (var k in a.Keys) {
				//Console.WriteLine (a[k]);
				if (a [k] == null)
					continue; 
				if (a[k].__IsSubClassOf (typeof(string))) {

					this [k] = new NSString( (string) a [k] );
				} else {
					this [k] = a [k];
				}
			}
		}

		// iOS method, it doesn't really sync, it only goes one way
		public void Synchronize() { 
			ISharedPreferences preferences = 
				UIViewController.Context.GetSharedPreferences ("StandardUserDefaults", FileCreationMode.Private); 

			ISharedPreferencesEditor editor = preferences.Edit();
			foreach (var k in this.Keys) {
				var o = this [k]; 
				//Console.WriteLine (o.GetType()); 
				if (o.__IsSubClassOf (typeof(NSString))) {
					editor.PutString (k, ((NSString)o).ToString ()); 
				} else if (o.__IsSubClassOf (typeof(string))) {
					editor.PutString (k, (string)o); 
				} else if (o.__IsSubClassOf (typeof(int))) {
					editor.PutInt (k, (int)o); 
				}
			}
			// TODO: delete as well?
			editor.Apply (); 
		}
	}

	public class NSUserDefaults 
	{
		private static NSStandardUserDefaults _StandardUserDefaults = null; 
		public static NSStandardUserDefaults StandardUserDefaults {
			get {
				if (_StandardUserDefaults == null) {
					_StandardUserDefaults = new NSStandardUserDefaults (); 
				}
					
				return _StandardUserDefaults; 
			}
				
		}


	}

	public class NSNumber
	{
		private double v = 0; 

		public NSNumber(int v) {
			this.v = v; 
		}

		public NSNumber(Int64 v) {
			this.v = v; 
		}

		// this is for casting NSNumber s => (int)s; 
		public static explicit operator NSNumber (int v) {

			return new NSNumber (v);
		}
		public virtual Int64 Int64Value {
			get {
				return (Int64)this.v; 
			}
		}
		public virtual int IntValue() {
			return (int)this.v; 
		}
		public static  NSNumber FromInt64(Int64 v) {
			return new NSNumber (v); 
		}

		public static  NSNumber FromInt32(Int32 v) {
			return new NSNumber (v); 
		}


		public static implicit operator int (NSNumber v) {
			if (v == null) {
				return 0;
			}
			return (int)v.IntValue (); 
		}
	}
	public class NSString : NSObject
	{
		private string s = null; 
		public new string ToString() {
			return s; 
		}
		public NSString(string s) {
			this.s = s; 
		}

		// this is for casting NSString s => (string)s; 
		public static explicit operator NSString (string str) {
			if (str == null ) {
				return null;
			}
			return new NSString (str);
		}

		public static implicit operator string (NSString str) {
			if (str == null) {
				return null;
			}
			return str.ToString ();
		}

		// TODO: ? 
		public CGSize StringSize (UIFont font, CGSize constrainedToSize, UILineBreakMode lineBreakMode)
		{
			Paint p = new Paint (PaintFlags.AntiAlias | PaintFlags.LinearText); 
			p.SetTypeface (font.Font); 

			Rect bounds = new Rect (); 
			p.GetTextBounds (this.s, 0, this.s.Length, bounds);

			return new CGSize (bounds.Width(), bounds.Height()); 
		}


	}
	public class NSBundle
	{
		public static NSBundle MainBundle {
			get {
				return new NSBundle (); 
			}
		}
		public string LocalizedString(string key, string comment) {
			return key; 
		}
	}

	public class UITapGestureRecognizer 
	{
		public UITapGestureRecognizer(Action a) {

		}
	}

	public class NSNotification
	{

	}
	public class NSUrl 
	{
		public string Url; 
		public NSUrl(string url) {
			this.Url = url;
		}

		public string RelativePath 
		{
			get { return Url; }
		}
		// TODO: fix this properly
		public NSUrl Append(string pathComponent, bool isDirectory) 
		{

			return new NSUrl (this.Url + "/" + pathComponent); 
		}

		public string Host 
		{
			get {
				int i = this.Url.IndexOf ("://")+3; 
				if (i >= 0) {
					string host = this.Url.Substring (i); 
					i = host.IndexOf ("/"); 
					if (i >= 0)
						host = host.Substring (0, i); 
					return host; 
				}
				return null; 
			}
		}

	}

	public class NSUrlRequest 
	{
		public NSUrl Url; 
		public NSUrlRequest(NSUrl url) {
			this.Url = url; 
		}
	}
}


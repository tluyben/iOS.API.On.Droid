using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Views.Animations;
using Android.Graphics;


using System.Threading.Tasks; 
using MessageUI; 
using Foundation;
using System.Reflection; 
using System.Collections.Generic;

namespace UIKit
{

	public enum UIRectEdge : ulong
	{
		None,
		Top,
		Left,
		Bottom = 4,
		Right = 8,
		All = 15
	}

	public class Rotate3dAnimation : Animation
	{
		private float from_degrees;
		private float to_degrees;
		private float center_x;
		private float center_y;
		private float depth_z;
		private bool reverse;
		private Camera camera;

		// Creates a new 3D rotation on the Y axis. The rotation is defined by its
		// start angle and its end angle. Both angles are in degrees. The rotation
		// is performed around a center point on the 2D space, definied by a pair
		// of X and Y coordinates, called centerX and centerY. When the animation
		// starts, a translation on the Z axis (depth) is performed. The length
		// of the translation can be specified, as well as whether the translation
		// should be reversed in time.

		// @param fromDegrees the start angle of the 3D rotation
		// @param toDegrees the end angle of the 3D rotation
		// @param centerX the X center of the 3D rotation
		// @param centerY the Y center of the 3D rotation
		// @param reverse true if the translation should be reversed, false otherwise

		public Rotate3dAnimation (float fromDegrees, float toDegrees,
			float centerX, float centerY, float depthZ, bool reverse)
		{
			from_degrees = fromDegrees;
			to_degrees = toDegrees;
			center_x = centerX;
			center_y = centerY;
			depth_z = depthZ;
			this.reverse = reverse;
		}

		public override void Initialize (int width, int height, int parentWidth, int parentHeight)
		{
			base.Initialize (width, height, parentWidth, parentHeight);

			camera = new Camera ();
		}

		protected override void ApplyTransformation (float interpolatedTime, Transformation t)
		{
			float fromDegrees = from_degrees;
			float degrees = fromDegrees + ((to_degrees - fromDegrees) * interpolatedTime);

			float centerX = center_x;
			float centerY = center_y;

			Matrix matrix = t.Matrix;

			camera.Save ();

			if (reverse)
				camera.Translate (0.0f, 0.0f, depth_z * interpolatedTime);
			else
				camera.Translate (0.0f, 0.0f, depth_z * (1.0f - interpolatedTime));

			camera.RotateY (degrees);
			camera.GetMatrix (matrix);
			camera.Restore ();

			matrix.PreTranslate (-centerX, -centerY);
			matrix.PostTranslate (centerX, centerY);
		}
	}

	public interface IInternalIntent {
		void DoIntent(); 
	}

	public enum UIInterfaceOrientation 
	{
		Portrait, Landscape, LandscapeLeft, LandscapeRight
	}

	public enum UIModalTransitionStyle
	{
		FlipHorizontal, CrossDissolve
	}
	/*public interface IUIViewController
	{
		void OnCreate (Bundle bundle); 

	}*/ 

	public class UIViewController : Activity//, IUIViewController
	{

		/*public UIViewController (string name, NSBundle something) : base (name, something)
		{
		}*/
		private UINavigationController _NavigationController;
		public UINavigationController NavigationController { 
			get { 
				if (_NavigationController == null) {

				}
				return _NavigationController;
			}
			set {
				_NavigationController = value; 
			}
		}
		public UINavigationItem NavigationItem { get; set; } 

		//backViewController.ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal;
		public UIModalTransitionStyle ModalTransitionStyle {get; set; }

		public static UIViewController MyInstance = null;
		public void RememberMe() {
			UIViewController.MyInstance = this; 
		}

		public static void InvokeInBackground(Action a) {
			Task.Run(a); 
		}

		public void InvokeOnMainThread(Action a) {
			RunOnUiThread (() => {
				a(); 
			}); 
		}

		public void BeginInvokeOnMainThread(Action a){
			InvokeOnMainThread (a); 
		}

		public void RefreshMe() {
			if (MyInstance != null
				&& this.GetType() != typeof(UIViewController) 
				&& this.GetType() != typeof(UINavigationController)
			) {

				if (MyInstance.GetType () != this.GetType ()) {

					throw new Exception (string.Format ("You made a booboo! You remembered a wrong object! {0} vs {1}", MyInstance.GetType ().Name,
						this.GetType ().Name)); 
				}

				BindingFlags bindFlags = BindingFlags.Public | BindingFlags.NonPublic |
					BindingFlags.Static | BindingFlags.Instance |
					BindingFlags.DeclaredOnly; 

				//BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
				//	| BindingFlags.Static;

				Dictionary<string, FieldInfo> thisFields = new Dictionary<string, FieldInfo> (); 

				// get all fields for this 
				foreach (var f in this.GetType().GetFields(bindFlags)) {
					thisFields.Add (f.Name, f); 
				}

				// copy the fields we need
				foreach (var f in MyInstance.GetType().GetFields(bindFlags)) {


					var f1 = thisFields [f.Name]; 
					var v1 = f.GetValue (MyInstance);
					if (f1 == null || v1 == null)
						continue; 
					try {
						f1.SetValue (this, v1);  
					} catch (FieldAccessException e) {
						Console.WriteLine ("Error: "+e.Message); // probably 'cannot access const field' but I cannot detect if it is? 
					}
				}
			}
		}
		public static void ForgetMe() {
			MyInstance = null; 
		}

		protected static UIViewController _Context; 
		public static UIViewController Context 
		{
			get {
				return _Context;
			}
		}

		private FixedLayout _Layout = null; 
		public FixedLayout Layout {
			get{
				return _Layout; 
			} 
		}


		public virtual void SetBackgroundImage(string i) {

		}


		// this function is here because on Android, besides BringToFront, all views are drawn on top of eachother
		// in the order they are added, meaning that our ordering will usually be off and there is no such thing 
		// as Z-index meaning we will have to just remove and re-do them all... TODO: find something better? won't
		// happen that often (one hopes) 
		// ONLY for internal use... 
		// TODO: this makes it (much) better to first build the view and THEN assign it to the uiviewcontroller in 
		// the end; that might actually be the same in iOS as well? 
		public void _RedoViews() {
			if (this.View != null) { // nothing to redo ?

				_Layout.RemoveAllViews ();  
				AddChildrenToLayout (this.View); 
			}
		}

		protected void AddChildrenToLayout(UIView v) {
			// TODO: let it fail, this should not (Ever) happen
			//if (((View)v.AndroidView).Parent != null) {
			//	return; 
			//}
			Layout.AddView (v.AndroidView); 
			foreach (var _v in v.GChildren) {
				AddChildrenToLayout (_v); 
			}
		}

		private UIView _View; 
		public UIView View {
			get {
				return _View; 
			}
			set {
				value.Frame = new System.Drawing.RectangleF (0, 0, UIScreen.MainScreen.Bounds.Width, 
					UIScreen.MainScreen.Bounds.Height); 


				//AddChildrenToLayout (value); 
				//Layout.AddView ((View)value); 
				//if (this._View != null) 
				//	_Layout.RemoveView (this._View.AndroidView); 
				this._View = value; 
				//this._View.BackgroundColor = UIColor.Red; 
				InvokeOnMainThread (() => {
					_RedoViews (); 
				});
			}
		}

		public void ResignFirstResponder() {

		}


		public UIViewController ()
		{

			RefreshMe (); 
			//RememberMe (); 
			//UIViewController._Context = this; 
			//this.View = new UIView (); 
		}

		protected string BaseNib = null; 

		public UIViewController(string nibName, NSBundle bundle) 
		{

			RefreshMe (); 
			this.BaseNib = nibName; 


		}

		public void Add(UIView view)
		{
			this.View.Add (view); 
		}

		public override Java.Lang.Object OnRetainNonConfigurationInstance ()
		{
			return base.OnRetainNonConfigurationInstance ();
		}

		private static float _Scale = 1; 
		public static float Scale { get { return _Scale; } }
		private static float _Density = 1; 
		public static float Density { get { return _Density; } }
		protected override void OnCreate (Bundle bundle)
		{



			if (MyInstance != null) {
				this.NavigationController = MyInstance.NavigationController; 
				this.NavigationItem = MyInstance.NavigationItem; 
			}

			_Context = this; 

			if (this.NavigationController != null) {
				//	RequestWindowFeature (WindowFeatures.ActionBar); 

				//RequestWindowFeature (WindowFeatures.NoTitle);
			}
			//this.ActionBar.Hide (); 
			//this.Title = ""; 

			base.OnCreate (bundle);

			DisplayMetrics dm = new DisplayMetrics (); 

			UIScreen.MainScreen = new UIScreen (); 
			WindowManager.DefaultDisplay.GetMetrics (dm); 
			UIScreen.MainScreen.Bounds = new System.Drawing.RectangleF (0,0,
				dm.WidthPixels/dm.Density,dm.HeightPixels/dm.Density); 

			_Scale = dm.Density * (UIScreen.MainScreen.Bounds.Width/320.0f); 
			_Density = dm.Density; 
			UIScreen.MainScreen.Bounds = new System.Drawing.RectangleF (0, 0, 320, 480); 

			_Layout = new FixedLayout(this, Scale); 
			SetContentView (Layout); 


			if (this.View == null) {
				this.LoadView (); 


			}
			 

			if (this.NavigationController != null) {
				this.NavigationController._PrepActionBar (this); 
			}

		
 			ViewDidLoad ();
			ViewWillAppear (true); 

			ForgetMe (); 
		}

		public virtual void LoadView () {

			if (BaseNib != null) {
				Internal.XibRenderer xib = new Internal.XibRenderer (this, BaseNib); 
				this.View = xib.Render (); 
			} else {
				this.View = new UIView ();
			}

		}

		public virtual void ViewWillAppear (bool animated)
		{

		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			if (this.NavigationItem != null && this.NavigationItem.RightBarButtonItem != null) {
				var mi = menu.Add (this.NavigationItem.RightBarButtonItem.Title); 
				mi.SetShowAsAction (ShowAsAction.Always); 
				this.NavigationItem.RightBarButtonItem.ItemId = mi.ItemId;
			}
			return base.OnCreateOptionsMenu (menu);

		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (this.NavigationItem != null && this.NavigationItem.RightBarButtonItem != null
				&& this.NavigationItem.RightBarButtonItem.ItemId == item.ItemId) {

				this.NavigationItem.RightBarButtonItem.ClickHandler (this, new EventArgs ()); 
			}

			return base.OnOptionsItemSelected (item);
		}

		public new string Title {
			get {
				return base.Title; 
			}
			set {
				base.Title = value; 
				if (this.NavigationController != null) {
					this.ActionBar.Title = value; 
				}
			}
		}

		protected override void OnStart ()
		{
			_Context = this; 

			base.OnStart ();

			ViewDidAppear (true); 
		}

		public void PresentViewController (UIViewController vc, bool animated, Action done) {

			vc.RememberMe (); 

			if (vc.__IsSubClassOf (typeof(IInternalIntent))) {

				((IInternalIntent)vc).DoIntent (); 

			} else {

				/*if (this.ModalTransitionStyle == UIModalTransitionStyle.FlipHorizontal) {
					Rotate3dAnimation rotation =
						new Rotate3dAnimation (0, 180, 160, 240, 310, false);
					rotation.Duration = 1000;
					rotation.FillAfter = true;
					rotation.Interpolator = new AccelerateInterpolator ();
					Layout.StartAnimation (rotation); 
				} else {*/ 

					Intent i = new Intent (UIViewController.Context, vc.GetType ()); 
					UIViewController.Context.StartActivity (i); 
				//}
			}
		}

		UIView _InputAccessoryView; 

		public virtual UIView InputAccessoryView {
			get {
				return _InputAccessoryView; 
			}
			set {
				_InputAccessoryView = value; 
			}
		}

		public void DismissViewController(bool animated, Action done) {

			Context.Finish (); 
		}

		public virtual void DidReceiveMemoryWarning ()
		{

		}

		public virtual void ViewDidLoad ()
		{

		}

		public virtual void ViewDidAppear (bool animated)
		{

		}
		public virtual void ViewWillDisappear (bool animated)
		{
		}
		public virtual bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return false;
		}

		public UIRectEdge EdgesForExtendedLayout
		{
			get;set;
		}

	}
}


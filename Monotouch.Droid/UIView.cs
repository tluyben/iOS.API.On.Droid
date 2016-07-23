using System;
using System.Collections.Generic;
using System.Linq; 
using System.Reflection; 
//using System.Drawing; 

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Views.Animations; 

using UIKit;
using Foundation;
using CoreGraphics; 

namespace UIKit
{
	public enum UIViewAnimationCurve
	{
		EaseInOut,
		EaseIn,
		EaseOut,
		Linear
	}
	public enum UIViewAutoresizing {
		None,
		FlexibleLeftMargin,
		FlexibleWidth,
		FlexibleRightMargin  ,
		FlexibleTopMargin  ,
		FlexibleHeight  ,
		FlexibleBottomMargin  ,
		FlexibleMargins  ,
		FlexibleDimensions  ,
		All 
	}

	public class UIEdgeInsets 
	{
		public float t,l,b,r; 

		public UIEdgeInsets (float t, float l, float b, float r) {
			this.t = t; 
			this.l = l; 
			this.b = b; 
			this.r = r; 
		}
	}

	// TODO: implement... 
	public class UILayer {

		private IView parent = null; 
		public UILayer(IView p) {
			this.parent = p; 
		}

		private Color _ShadowColor; 
		public Color ShadowColor {
			get {
				return _ShadowColor;
			}
			set {
				_ShadowColor = value; 
			}
		}

		private Color _BorderColor ; 
		public Color BorderColor {
			get {
				return _BorderColor; 
			}
			set {
				_BorderColor = value; 
			}
		}

		float _ShadowOpacity=0;  
		public float ShadowOpacity
		{
			get {
				return _ShadowOpacity; 
			}
			set {
				_ShadowOpacity = value; 
			}
		}

		float _BorderWidth = 0 ; 
		public float BorderWidth {
			get {
				return _BorderWidth; 
			}
			set {
				_BorderWidth = value; 
			}
		}
		float _CornerRadius = 0 ; 
		public float CornerRadius {
			get {
				return _CornerRadius; 
			}
			set {
				PaintDrawable pd = new PaintDrawable (); 
				pd.Paint.Color = parent.BackgroundColor.CGColor; 
				pd.SetCornerRadius (12.5f*UIViewController.Scale); 
				((View)parent).SetBackgroundDrawable (pd); 
			
				_CornerRadius = value; 
			}
		}

		public CGSize ShadowOffset {
			get;
			set; 
		}

		public nfloat ShadowRadius {
			get;
			set;
		}


	}


	[AttributeUsage (AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
	public class ExportAttribute : Attribute
	{
		public string Name; 
		public ExportAttribute(string name) {
			this.Name = name; 
		}
	}

	public interface UIConnectingView {
		UIView UIOwnerView {get; set;}
		Canvas CurrentCanvas { get; }
		Action BaseDrawAction { get; }
	}

	public class AndroidView : View, UIConnectingView
	{
		public UIView UIOwnerView { get; set;}

		public AndroidView(Context c) : base(c) {

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
			UIGraphics._CurrentContext = new CoreGraphics.CGContext(this); 
			this._CurrentCanvas = canvas; 

			_BaseDrawAction = () => {
				base.OnDraw (canvas);
			}; 

			UIOwnerView.Draw (new System.Drawing.RectangleF (
				UIOwnerView.Frame.X, 
				UIOwnerView.Frame.Y,
				UIOwnerView.Frame.Width,
				UIOwnerView.Frame.Height
			)); 

			//if (this.Layer.CornerRadius > 0) {
				/*Path clipPath = new Path (); 
				clipPath.AddRoundRect (new RectF (0, 0, this.Width, this.Height), this.Layer.CornerRadius, 
					this.Layer.CornerRadius, Path.Direction.Cw);
				canvas.ClipPath (clipPath); */
				/*Paint p = new Paint (); 
				p.AntiAlias = true; 
				p.Color = this.BackgroundColor.CGColor; 

				canvas.DrawRoundRect (new RectF (0, 0, Width, Height), 12.5f, 12.5f, p);  */
			//}


		}
	}
	public class UIView : NSObject, IView
	{
		protected View __View; 

		// TODO: move to IView 
		public virtual void TouchesBegan (NSSet touches, UIEvent evt)
		{
		}

		public virtual void TouchesMoved (NSSet touches, UIEvent evt)
		{
		}

		public virtual void TouchesEnded (NSSet touches, UIEvent evt)
		{

		}
		public virtual void Draw(System.Drawing.RectangleF rect) {

			//UIGraphics._CurrentContext = new MonoTouch.CoreGraphics.CGContext(__View); 

			((UIConnectingView)__View).BaseDrawAction (); 
		}

		public virtual void LayoutSubviews ()
		{
			this.LayoutChildren (this.Frame.Left, this.Frame.Top); 
		}

		public virtual void SizeToFit() 
		{


		}

		public virtual System.Drawing.SizeF SizeThatFits(System.Drawing.SizeF size) 
		{
			return this.Size; 
		}

		protected virtual void Dispose (bool disposing)
		{
		}

		public float Alpha {
			get {
				return __View.Alpha; 
			}
			set {
				if (CurrentAnimation != null) {
					AnimationSettings s = Animations [CurrentAnimation]; 
					if (s != null) {
						AlphaAnimation ani = new AlphaAnimation (__View.Alpha, value); 
						ani.Duration = (long)(s.Duration * 1000); 
						ani.RepeatCount = s.Repeat; 
						ani.RepeatMode = RepeatMode.Restart;

						s.Animations.Add (
							new AnimationPair {
								Anim = ani, View = this
							}
						); 
					}
				} else {

					__View.Alpha = value;
				}
			}
		}

		private UILayer _Layer; 
		public UILayer Layer {
			get {
				return _Layer;
			} 
			set {
				_Layer = value; 
			}
		}

		public int Tag { get; set; }

		protected UIViewController Controller; 
		protected virtual void _Init() 
		{
			((UIConnectingView)this.__View).UIOwnerView = this; 
			Controller = UIViewController.Context; 
			_Layout = ((UIViewController)__View.Context).Layout; 
			Layer = new UILayer(this);
		}
		public UIView (View RealView)
		{
			if (RealView != null) {
				__View = RealView; 
				_Init (); 
			}
		}

		public UIView(CoreGraphics.CGRect rect) {
			__View = new AndroidView (UIViewController.Context); 
			_Init (); 
		}
		public UIView () //: base(UIViewController.Context)
		{
			// if we are doing a superclass, do not init. 
			//if (this.GetType () == typeof(UIView)) {
			__View = new AndroidView (UIViewController.Context); 
			_Init (); 
			//}
		}

		public virtual UIViewAutoresizing AutoresizingMask { get; set;}

		public bool UserInteractionEnabled {get; set; }

		public void SetNeedsDisplay() {
			__View.Invalidate (); 
		}


		bool _ClipsToBounds; 
		public bool ClipsToBounds {
			get {
				return _ClipsToBounds; 
			}
			set {
				_ClipsToBounds = value;
			}
		}



		UIColor _BackgroundColor; 
		public virtual UIColor BackgroundColor {
			get {
				return  _BackgroundColor; 
			}
			set{
				_BackgroundColor = value; 
				__View.SetBackgroundColor (value.CGColor); 

			}
		}

		protected bool _Hidden = false; 
		public bool Hidden {
			get {
				return _Hidden; 
			} 
			set {
				_Hidden = value; 
			}
		}


		public CGRect Bounds {
			get {
				return _frame; 
			}
		}

		protected FixedLayout _Layout; 

		#if UNIVERSALAPI
		protected CGRect _frame; 
		public CGRect Frame {
		#else
		protected System.Drawing.RectangleF _frame; 
		public System.Drawing.RectangleF Frame {
		#endif 
			set {

				this._frame = value; 
				AbsFrame = value; 
				this.LayoutChildren (this.Frame.Left, this.Frame.Top); 
			}

			get {

				return this._frame; 
			}

		}

		public System.Drawing.SizeF Size {
			get {
				return new System.Drawing.SizeF (Frame.Width, Frame.Height); 
			}
			set {
				Frame = new System.Drawing.RectangleF (
					Frame.X,
					Frame.Y,
					value.Width,
					value.Height
				); 
			}
		}
		#if UNIVERSALAPI
		public CGRect AbsFrame {
		#else 
		public System.Drawing.RectangleF AbsFrame {
		#endif
			set {

				__View.LayoutParameters = 
					new FixedLayout.LayoutParams ((int)value.Width, (int)value.Height, (int)value.X, (int)value.Y); 

			}

		}

		public IView GParent {get; set;}
		protected List<UIView> _Children = new List<UIView> (); 
		public List<UIView> GChildren {
			get{
				return _Children; 
			} 
		}



		public void LayoutChildren(float x, float y) {
			//this.GChildren.ForEach (delegate(IView Child) {
			foreach(var Child in GChildren) {
				float _x = Child.Frame.Left + x; 
				float _y = Child.Frame.Top + y; 
				Child.AbsFrame = new System.Drawing.RectangleF(_x, _y, Child.Frame.Width, Child.Frame.Height); 
				Child.LayoutChildren(_x, _y); 
			}; 
		}

		public void AddSubview (UIView View) {
			Add (View); 
		}

		public virtual View AndroidView {
			get {
				return __View; 
			}
		}

		public UIView[] Subviews {
			get {
				var cs =  from c in GChildren
					select (UIView)c; 
				return cs.ToArray (); 

			}

		}

		public void Dispose() {
			// ?
		}


		// TODO: rewrite this! currently you cannot dynamically add stuff, see the View {get;set;} in UIViewController!
		public virtual void Add(UIView View) {
			View.GParent = this; 

			GChildren.Add (View); 

			//View.Frame = new RectangleF (View.Frame.Left + this.Frame.Left, View.Frame.Top + this.Frame.Top, View.Frame.Width, View.Frame.Height);  

			//this._Layout.AddView ((View)View); 

			this.LayoutChildren (this.Frame.Left, this.Frame.Top); 

			Controller._RedoViews (); 

		}

		protected bool _IsFirstResponder = false; 
		public bool IsFirstResponder  {
			get {
				return _IsFirstResponder; 
			}
		}

		// move to IView , apparently
		public void InvokeOnMainThread(Action a) {

			((Activity)__View.Context).RunOnUiThread (() => {
				a(); 
			}); 
		}

		public static bool operator ==(UIView a, UIButton b)
		{
			return (IView)a == (IView)b; 
		}
		public static bool operator !=(UIView a, UIButton b)
		{
			return (IView)a != (IView)b; 
		}

	


		// animation crap 

		public static void Animate (double duration, Action animation, Action completion)
		{
			BeginAnimations (animation.ToString ()); 
			if (completion != null) {
				((AnimationSettings)Animations [CurrentAnimation]).OnComplete = completion; 
			}
			SetAnimationDuration (duration); 
			animation (); 
			CommitAnimations (); 
		}
		public static void Animate (double duration, Action animation)
		{
			Animate (duration, animation); 
		}
		public class AnimationPair {
			public Animation Anim; 
			public UIView View; 
		}
		public class AnimationSettings {
			public double Duration; 
			public UIViewAnimationCurve Curve; 
			public int Repeat = 1; 
			public bool Complete = false; 
			public NSObject Delegate = null; 
			public ObjCRuntime.Selector Selector = null; 
			public Action OnComplete = null; 
			public List<AnimationPair> Animations = new List<AnimationPair>(); 
		}

		protected static Dictionary<string, AnimationSettings> Animations = new Dictionary<string, AnimationSettings>(); 
		protected static string CurrentAnimation = null; 
		public static void BeginAnimations (string name) {
			CurrentAnimation = name; 
			Animations.Add (name, new AnimationSettings ()); 
		}
		public static void SetAnimationDuration(double duration) {
			if (CurrentAnimation != null) {
				Animations [CurrentAnimation].Duration = duration; 
			}
		}
		public static void SetAnimationDelegate(NSObject _del) {
			if (CurrentAnimation != null) {
				Animations [CurrentAnimation].Delegate = _del; 
			}
		}
		public static void SetAnimationCurve(UIViewAnimationCurve curve) {
			if (CurrentAnimation != null) {
				Animations [CurrentAnimation].Curve = curve; 
			}
		}

		public static void SetAnimationRepeatCount(int count) {
			if (CurrentAnimation != null) {
				Animations [CurrentAnimation].Repeat = count; 
			}
		}

		public static void SetAnimationDidStopSelector(ObjCRuntime.Selector selector) {
			if (CurrentAnimation != null) {
				Animations [CurrentAnimation].Selector = selector; 
			}
		}


		public void AddGestureRecognizer(UITapGestureRecognizer gr) {

		}

		public virtual void RemoveFromSuperview()
		{
			this.GParent.GChildren.Remove (this); 
			this.GParent.LayoutChildren (this.Frame.Left, this.Frame.Top); 
			Controller._RedoViews (); 
		}

		public virtual CGPoint Center {
			get;
			set; 
		}

		public static void runSelector(NSObject _del, ObjCRuntime.Selector _sel) {
			BindingFlags bindFlags = BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.Static | BindingFlags.Instance |
				BindingFlags.DeclaredOnly; 
			foreach (var method in _del.GetType().GetMethods(bindFlags)) {
				foreach (var attr in method.GetCustomAttributes(true)) {
					if (attr is ExportAttribute) {
						var ea = (ExportAttribute)attr; 
						if (ea.Name == _sel.Name) {
							//try {
								method.Invoke (_del, new object[]{ _del });
							//} catch (TargetInvocationException e) {
							//	Console.WriteLine ("Probably object was null again : " + e.Message); 
							//}
							return; 
						}
					}
				}
			}
		}

		public static void CommitAnimations() {
			if (CurrentAnimation == null)
				return; 




			AnimationSettings s = Animations [CurrentAnimation]; 
			int animCount = s.Animations.Count; 

			Animations [CurrentAnimation].Complete = true; 
			Animations.Remove (CurrentAnimation); 
			CurrentAnimation = null; 

			/*if (animCount > 0) foreach (var a in s.Animations) {
				if (s.Selector != null && s.Delegate!=null) {
					runSelector(s.Delegate, s.Selector); 
				}
				if (s.OnComplete != null) {
					s.OnComplete(); 
				}
			}
*/ 
			if (animCount > 0) foreach (var a in s.Animations) {
				a.Anim.AnimationEnd += (object sender, Animation.AnimationEndEventArgs e) => {
					
						animCount --; 
						if (animCount == 0) {
							if (s.Selector != null && s.Delegate!=null) {
								runSelector(s.Delegate, s.Selector); 
							}
							if (s.OnComplete != null) {
								s.OnComplete(); 
							}
						}
				}; 

				a.View.__View.StartAnimation (a.Anim); 
			}




		}

	}


}


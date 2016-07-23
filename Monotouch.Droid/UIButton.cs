using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;
using Android.Graphics.Drawables; 

using CoreGraphics; 
using Foundation; 

namespace UIKit
{
	public enum UIControlState 
	{
		Normal, 
		Disabled,
		Highlighted
	}
	public enum UIButtonType
	{
		Custom,
		RoundedRect,
		DetailDisclosure,
		InfoLight,
		InfoDark,
		ContactAdd,
		System = 1
	}
	public enum UIControlContentHorizontalAlignment
	{
		Left, Right, Center
	}

	public class UIButtonEventArgs : EventArgs 
	{
		public int ButtonIndex; 
	}

	public class AndroidButton : Button, UIConnectingView
	{
		public UIView UIOwnerView { get; set;}

		public AndroidButton(Context c) : base(c)
		{
			
		}
		private Canvas _CurrentCanvas; 
		public Canvas CurrentCanvas {
			get {
				return _CurrentCanvas;
			}
		}

		public UIImage Image = null; 


		public Action _BaseDrawAction; 
		public Action BaseDrawAction { get{ return _BaseDrawAction; } } 

		protected override void OnDraw (Canvas canvas)
		{
			_CurrentCanvas = canvas; 
			UIGraphics._CurrentContext = new CoreGraphics.CGContext(this); 

			_BaseDrawAction = () => {
				base.OnDraw (canvas);
			};

			if (Image != null) {


				float x = 0; 
				UIButton __Parent = (UIButton)UIOwnerView; 

				if (__Parent.HorizontalAlignment == UIControlContentHorizontalAlignment.Center) {

					var txtSize = UILabel.SStringSize (
						this.Text, 
						__Parent.Font,
						new System.Drawing.SizeF (int.MaxValue, int.MaxValue),
						UILineBreakMode.Wrap
					);

					x = __Parent.Frame.Width / 2 - (this.Image.Size.Width + txtSize.Width) / 2;
				} else if (__Parent.HorizontalAlignment == UIControlContentHorizontalAlignment.Left) {
					this.SetPadding ((int)((this.Image.Size.Width+2)*UIViewController.Scale), 0, 0, 0); 
				} else if (__Parent.HorizontalAlignment == UIControlContentHorizontalAlignment.Right) {
					var txtSize = UILabel.SStringSize (
						this.Text, 
						__Parent.Font,
						new System.Drawing.SizeF (int.MaxValue, int.MaxValue),
						UILineBreakMode.Wrap
					);
					x = __Parent.Frame.Width - txtSize.Width - this.Image.Size.Width; 
				}

				this.SetPadding (
					this.PaddingLeft + (int)(__Parent.ContentEdgeInsets.l * UIViewController.Scale),
					this.PaddingTop + (int)(__Parent.ContentEdgeInsets.t * UIViewController.Scale),
					this.PaddingRight + (int)(__Parent.ContentEdgeInsets.r * UIViewController.Scale),
					this.PaddingBottom + (int)(__Parent.ContentEdgeInsets.b * UIViewController.Scale)
				); 

				float y = (__Parent.Frame.Height / 2 - Image.Size.Height / 2);
				x += __Parent.ImageEdgeInsets.l;
				y += __Parent.ImageEdgeInsets.t; 
				x *= UIViewController.Scale;
				y *= UIViewController.Scale; 
				canvas.DrawBitmap (Image.Bitmap,x, y, null); 
			}

			UIOwnerView.Draw (new System.Drawing.RectangleF (
				UIOwnerView.Frame.X, 
				UIOwnerView.Frame.Y,
				UIOwnerView.Frame.Width,
				UIOwnerView.Frame.Height
			)); 
		}

	}

	public class UIButton : UIView
	{

		public bool Enabled {
			get {
				return ((AndroidButton)__View).Enabled; 
			} 
			set {
				((AndroidButton)__View).Enabled = value; 
			}
		}


		UIControlState ControlState; 


		public void SetBackgroundImage(UIImage image, UIControlState controlState) {
			((AndroidButton)(View)__View).SetBackgroundDrawable (new BitmapDrawable (image.Bitmap)); 
		}

		public void SetImage (UIImage im, UIControlState cs)
		{
			((AndroidButton)(View)__View).Image = im; 
			ControlState = cs; 

			/*UIImageView imv = new UIImageView (); 
			imv.Image = im; 
			imv.BackgroundColor = UIColor.Red; 
			imv.Frame = new System.Drawing.RectangleF (ImageEdgeInsets.l, ImageEdgeInsets.t, imv.Frame.Width,imv.Frame.Height);
			Add (imv); */

			//	BitmapDrawable bd = new BitmapDrawable (im.Bitmap);
			//this.SetBackgroundDrawable (bd); 
		}

		protected override void _Init() {
			base._Init (); 
			BackgroundColor = UIColor.Transparant;

			HorizontalAlignment = UIControlContentHorizontalAlignment.Center;

			__View.Touch += (object sender, View.TouchEventArgs e) => {
				MotionEvent ev = e.Event;
				switch(ev.Action) {
				case MotionEventActions.Down: {
						if (_TouchDown != null) {
							_TouchDown(sender, new EventArgs()); 
						}
						break; 

					}
				case MotionEventActions.Up: {
						if (_TouchUpInside != null) {
							_TouchUpInside(sender, new EventArgs()); 
						}
						break; 
					}
				}
			};
		}

		public UIButton (UIButtonType t) : base(new AndroidButton (UIViewController.Context))
		{


		} 

		public UIButton () : base(new AndroidButton (UIViewController.Context))
		{


		}

		nint _Tag; 
		public virtual nint Tag {
			get {
				return _Tag; 
			}
			set {
				_Tag = value; 
			}

		}


		// TODO: move to IView

		public bool UserInteractionEnabled {get; set; }

		public virtual CGRect TitleRectForContentRect (CGRect rect)
		{
			return rect; 
		}


		private UIControlContentHorizontalAlignment _HorizontalAlignment; 
		public UIControlContentHorizontalAlignment HorizontalAlignment
		{
			set {
				_HorizontalAlignment = value;

				if (value == UIControlContentHorizontalAlignment.Center) {
					((AndroidButton)__View).Gravity = GravityFlags.Center;

				} else if (value == UIControlContentHorizontalAlignment.Left) {
					((AndroidButton)__View).Gravity = GravityFlags.Left|GravityFlags.CenterVertical; 
				} else if (value == UIControlContentHorizontalAlignment.Right) {
					((AndroidButton)__View).Gravity = GravityFlags.Left|GravityFlags.CenterVertical; 
				}
			}
			get {
				return _HorizontalAlignment; 
			}
		}

		private UIEdgeInsets _ImageEdgeInsets = new UIEdgeInsets(0,0,0,0);
		public UIEdgeInsets ImageEdgeInsets {
			set {
				_ImageEdgeInsets = value;
			}
			get {
				return _ImageEdgeInsets; 
			}
		}

		private UIEdgeInsets _ContentEdgeInsets = new UIEdgeInsets(0,0,0,0);
		public UIEdgeInsets ContentEdgeInsets {
			set {
				_ContentEdgeInsets = value;
			}
			get {
				return _ContentEdgeInsets; 
			}
		}



		// TODO: finish / test
		protected event EventHandler _TouchUpInside; 
		public event EventHandler TouchUpInside
		{
			add{
				//this.LongClick += (object sender, LongClickEventArgs e) => {

				//};
				_TouchUpInside += value; 
				//((AndroidButton)__View).Click += value; 
			}
			remove{
				// ? 
				//this.LongClick -= value; 
				_TouchUpInside -= value; 
			}
		}

		/* map the iOS function to click */
		protected event EventHandler _TouchDown; 
		public event EventHandler TouchDown {
			add {
				//((AndroidButton)__View).Click += value;   
				_TouchDown += value; 
			}
			remove {
				//((AndroidButton)__View).Click -= value; 
				_TouchDown -= value; 
			}

		}

		public virtual UIEdgeInsets TitleEdgeInsets {
			get;
			set; 
		}

		public virtual UILabel TitleLabel {
			get {
				var b = ((AndroidButton)__View); 
				return new UILabel (); 
			}
		}

		public void SetTitle(string t, UIControlState state) {
			((AndroidButton)__View).Text = t; 
		}

		// TODO: ?? 
		public void SetTitleShadowColor(UIColor t, UIControlState state) {
			((AndroidButton)__View).SetShadowLayer (1, 1, 1, t.CGColor); 
		}

		public void SetTitleColor(UIColor t, UIControlState state) {
			((AndroidButton)__View).SetTextColor (t.CGColor);
		}

		public string Title {
			get {
				return ((AndroidButton)__View).Text;
			}
			set {
				SetTitle (value, UIControlState.Normal);  
			}
		}

		private UIFont _Font; 
		public UIFont Font {
			get {
				if (_Font == null) {
					_Font = new UIFont (); 
				}
				return _Font; 
			}
			set {
				_Font = value; 
				((AndroidButton)__View).Typeface = value.Font; 
				((AndroidButton)__View).SetTextSize (ComplexUnitType.Sp, value.Size); 
			}
		}

	}
}


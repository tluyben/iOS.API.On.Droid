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
using System.IO;
using System.Drawing; 

using Foundation;
using CoreGraphics; 

namespace UIKit
{
	public enum UIImagePickerControllerQualityType
	{
		High,
		Medium,
		Low,
		At640x480,
		At1280x720,
		At960x540
	}
	public enum UIImagePickerControllerCameraDevice
	{
		Rear,
		Front
	}
	public enum UIImagePickerControllerSourceType
	{
		PhotoLibrary,
		Camera,
		SavedPhotosAlbum
	}

	public class UIImagePickerControllerDelegate 
	{
		public virtual void FinishedPickingMedia (UIImagePickerController picker, NSDictionary info)
		{

		}

		public virtual void Canceled (UIImagePickerController picker)
		{

		}

	}
	public class UIImagePickerController : UINavigationController
	{
		public virtual Foundation.NSObject Delegate
		{
			get { return base.Delegate; }
			set { throw new NotSupportedException(); }
		}
	}
	public class UIImage
	{
		private Bitmap _Bitmap = null;
		public Bitmap Bitmap {
			get{
				return _Bitmap; 
			}
		}

		public System.Drawing.SizeF Size {
			get {
				return new System.Drawing.SizeF (_Bitmap.Width/UIViewController.Scale+1f, _Bitmap.Height/UIViewController.Scale+1f); 
			}

		}

		public static UIImage FromBundle(string b) {
			if (b.IndexOf (".") < 0) // TODO: ? 
				b += ".png"; 
			return UIImage.FromFile (b); 
		}

		public void Draw(CGRect rect) {

			Bitmap.PrepareToDraw (); 
		}



		public static UIImage FromFile(string p) {
			Stream istr;
			UIImage i = new UIImage (); 

			try {
				istr = UIViewController.Context.Assets.Open(p);
				i._Bitmap = BitmapFactory.DecodeStream(istr);
			} catch (Exception e) {

				// try to read with a @2x as it would be usual on iOS
				try {
					int j = p.LastIndexOf("."); 
					if (j>0) {
						p = p.Substring(0, j) + "@2x" + p.Substring(j); 
					}
					istr = UIViewController.Context.Assets.Open(p);
					//Console.WriteLine(p); 
					i._Bitmap = BitmapFactory.DecodeStream(istr);
				}catch (Exception e1){
					return null; 
				}
			}
			return i; 
		}
	}

	public class AndroidImageView : ImageView , UIConnectingView
	{
		public UIView UIOwnerView { get; set;}


		public AndroidImageView(Context c) : base(c)
		{

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
			this._CurrentCanvas = canvas; 
			UIGraphics._CurrentContext = new CoreGraphics.CGContext(this); 

			_BaseDrawAction = () => {
				base.OnDraw (canvas);
			}; 

			UIOwnerView.Draw (new System.Drawing.RectangleF (
				UIOwnerView.Frame.X, 
				UIOwnerView.Frame.Y,
				UIOwnerView.Frame.Width,
				UIOwnerView.Frame.Height
			)); 
		}

	}

	public class UIImageView : UIView
	{


		public void SetImageBitmap(Bitmap b) {
			((AndroidImageView)__View).SetImageBitmap (b); 
		}

		public UIImage Image 
		{

			set {
				this.SetImageBitmap (value.Bitmap);

				// TODO: does iOS does this? :) 
				//this.Frame = new RectangleF (0, 0, value.Size.Width, value.Size.Height); 
			}
		}
		public virtual UIViewAutoresizing AutoresizingMask { get; set;}

		public UIImageView () : base(new AndroidImageView (UIViewController.Context))
		{


		}

		public UIImageView (UIImage image) : base(new AndroidImageView (UIViewController.Context)) {
			Image = image; 
		}

		public UIImageView (CoreGraphics.CGRect frame) : base(new AndroidImageView (UIViewController.Context)) {
			this.Frame = frame; 
		}
	}
}


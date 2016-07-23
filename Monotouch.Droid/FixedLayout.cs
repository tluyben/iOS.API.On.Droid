using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Net;
using Android.Graphics;
using Android.Util; 

using Foundation; 

namespace UIKit
{
	public class FixedLayout : RelativeLayout
	{

		public new class LayoutParams : RelativeLayout.LayoutParams {
			public int x,y;
			public LayoutParams(int width, int height, int x, int y) : base(width, height) {
				this.x = x; 
				this.y = y; 
				MultScale(UIViewController.Scale); 
			}
			public LayoutParams(float width, float height, float x, float y) : base((int)width, (int)height) {
				this.x = (int)x; 
				this.y = (int)y; 
				MultScale(UIViewController.Scale); 
			}
			public FixedLayout.LayoutParams MultScale(float scale) {
				LeftMargin = x = (int)(x * scale); 
				TopMargin = y = (int)(y * scale); 
				Width = (int)(Width * scale); 
				Height = (int)(Height * scale); 
				return this; 
			}
		}

		public FixedLayout(Context context) : base(context)
		{

		}

		public float scale = 1; 
		public FixedLayout(Context context, float scale) : base(context)
		{
			this.scale = scale; 
		}




	}

	/*
	public class _FixedLayout : ViewGroup
	{
		public new class LayoutParams : ViewGroup.LayoutParams {

			public int x,y;

			public LayoutParams(int width, int height, int x, int y) : base(width, height) {
				this.x = x; 
				this.y = y; 
			}

			public LayoutParams(float width, float height, float x, float y) : base((int)width, (int)height) {
				this.x = (int)x; 
				this.y = (int)y; 
			}

			public LayoutParams(ViewGroup.LayoutParams source) : base(source) {

			}
		}

		float scale;
		public _FixedLayout (Context context, float scale) : base(context)
		{

			this.scale = scale; 
		}

		public override void AddView (View child)
		{
			base.AddView (child);
		}

		protected override ViewGroup.LayoutParams GenerateDefaultLayoutParams() {
			return new LayoutParams (LayoutParams.WrapContent, LayoutParams.WrapContent, 0, 0); 
		}

		protected override bool CheckLayoutParams(ViewGroup.LayoutParams p) {
			return p.GetType () == typeof(FixedLayout.LayoutParams); 
		}

		protected override ViewGroup.LayoutParams GenerateLayoutParams (ViewGroup.LayoutParams p)
		{
			return new LayoutParams (p); 
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b) {

			for (int i = 0; i < ChildCount; i++) {
				View child = GetChildAt (i); 
				if (child.Visibility != ViewStates.Gone) {
					FixedLayout.LayoutParams lp = (FixedLayout.LayoutParams)child.LayoutParameters;
					// l, t, r , b
					child.Layout ((int)(lp.x*scale), (int)(lp.y*scale), (int)(lp.x*scale + child.MeasuredWidth*scale), (int)(lp.y*scale + child.MeasuredHeight*scale)); 
				}
			}

		}

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{

			MeasureChildren (widthMeasureSpec, heightMeasureSpec); 

			int width =0 , height=0; 

			for (int i = 0; i < this.ChildCount; i++) {
				View child = this.GetChildAt (i); 
				if (child.Visibility != ViewStates.Gone) {
					FixedLayout.LayoutParams lp = (FixedLayout.LayoutParams)child.LayoutParameters;
					int right = (int)(lp.x*scale + child.MeasuredWidth*scale); 
					int bottom = (int)(lp.y*scale + child.MeasuredHeight*scale);
					width = Math.Max (width, right); 
					height = Math.Max (height, bottom); 
				}
			}

			height = Math.Max (height, SuggestedMinimumHeight); 
			width = Math.Max (width, SuggestedMinimumWidth); 
			width = ResolveSize (width, widthMeasureSpec); 
			height = ResolveSize (height, heightMeasureSpec); 
			SetMeasuredDimension (width, height); 

		}
	}
	*/
}


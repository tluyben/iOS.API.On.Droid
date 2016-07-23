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

namespace UIKit
{
	public enum UITableViewRowAnimation {
		Fade
	}

	public class AndroidListView : ListView , UIConnectingView
	{
		public UIView UIOwnerView { get; set;}

		public AndroidListView (Context c) : base(c)
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

	public class UITableView : UIView
	{


		public void DeleteRows(NSIndexPath[] paths, UITableViewRowAnimation animation) {

		}



		public UITableViewCell DequeueReusableCell(string ident) {
			return (UITableViewCell)new UITableViewCell(Source._CurrentConvertView);
		}


		public UITableView () : base (new AndroidListView (UIViewController.Context))
		{

			((AndroidListView)this.__View).ChoiceMode = ChoiceMode.Single; 


		}


		public void SetEditing (bool editing, bool animated) {

		}
		public void ReloadData() {
			if (((AndroidListView)this.__View).Adapter!=null) 
				((BaseAdapter)((AndroidListView)this.__View).Adapter).NotifyDataSetChanged (); 
		}
		UITableViewCellSeparatorStyle _SeparatorStyle; 
		public UITableViewCellSeparatorStyle SeparatorStyle{
			get {
				return _SeparatorStyle; 
			} 
			set {
				_SeparatorStyle = value; 
			}
		}
		float _RowHeight; 
		public float RowHeight {
			get {
				return _RowHeight; 
			} 
			set {
				_RowHeight = value; 
			}
		}

		UITableViewSource _Source; 
		public UITableViewSource Source {
			get {
				return _Source; 
			} 
			set {
				value._TableView = this;
				((AndroidListView)this.__View).Adapter = value; 
				_Source = value; 


				// TODO: make nicer 
				((AndroidListView)this.__View).ItemClick += ( sender, e) => {
					e.View.Selected = true; 

					((UITableViewSource)this.Source).RowSelected(this, new NSIndexPath(e.Position)); 

					for (int i =0 ; i < ((AndroidListView)this.__View).ChildCount; i++) {
						((AndroidListView)this.__View).GetChildAt(i).SetBackgroundColor(Color.Transparent); 
					}

					e.View.SetBackgroundColor(UIColor.LightGray.CGColor); 
				};
			}
		}


	}

	public enum UITableViewCellSeparatorStyle {
		None
	}
	public class NSIndexPath 
	{
		public NSIndexPath(int Row) {
			this.Row = Row;
		}
		public int Row; 
	}


	public enum UITableViewCellEditingStyle 
	{
		Delete, None
	}

	public class UIRefreshControl : UIView
	{


		public EventHandler ValueChanged; 

		public void EndRefreshing() {

		}
	}

	public class UITableViewSource : BaseAdapter<string>
	{
		public UITableView _TableView = null; 

		public virtual int RowsInSection (UITableView tableview, int section) {
			return -1; 
		}

		 
		// for the android table
		public override long GetItemId(int position)
		{
			return -1;
		}
		public override string this[int position] {  
			get { return "d/c"; }
		}
		public override int Count {
			get { return RowsInSection(_TableView, 0); }
		}

		public View _CurrentConvertView = null; 

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			//View view = convertView; // re-use an existing view, if one is available
			//if (view == null) // otherwise create a new one
				//view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
			//view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];
			//Console.WriteLine ("Adding Row; " + position); 
			_CurrentConvertView = convertView; 
			return  GetCell (_TableView, new NSIndexPath(position)).AndroidView; 

		}

		public virtual UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			return null; 
		}
		public virtual void RowSelected (UITableView tableView, NSIndexPath indexPath) {


		}
		public virtual void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{

		}
		public virtual UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.None; 
		}
		public virtual bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return false;
		}

		public virtual int NumberOfSections (UITableView tableView)
		{
			return 1; 
		}


		public virtual float GetHeightForHeader (UITableView tableView, int section)
		{
			return 50f; 
		}

		public virtual UIView GetViewForHeader (UITableView tableView, int section)
		{
			return null; 
		}
		public virtual string TitleForHeader (UITableView tableView, int section)
		{
			return ""; 
		}

		public virtual string[] SectionIndexTitles (UITableView tableView)
		{
			return new string[]{ };
		}

		public virtual float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 50f; 
		}
	}
	public class AndroidListCell : FixedLayout, UIConnectingView
	{
		public UIView UIOwnerView { get; set;}

		public AndroidListCell(Context c):base(c)
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
	public class UITableViewCell : UIView // TODO: shouldn't this be UIView
	{


	
		public UITableViewCell(View v) : base(v) {

		}

		public UITableViewCell () : base(new AndroidListCell (UIViewController.Context))
		{
	
			Frame = new System.Drawing.RectangleF (0, 0, UIScreen.MainScreen.Bounds.Width, 64); 
		}





	}

}


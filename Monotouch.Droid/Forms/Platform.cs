using System;
using UIKit;
#if __vla__ 
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms; 
using System.Drawing;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.Generic; 

namespace Metaframe.Forms
{
	public static class Forms
	{
		public static void Init() 
		{

		}

		public static UIViewController CreateViewController(this Page page) {
			return null;
		}
	}


}
namespace Xamarin.Forms.Platform.iOS
{



	public abstract class ViewRenderer : ViewRenderer<View, UIView>
	{

	}

	/*public class PropertyChangedEventArgs 
	{

	}*/ 

	public class FocusRequestArgs
	{

	}

	public class ElementChangedEventArgs<TElement> : Xamarin.Forms.Platform.Android.ElementChangedEventArgs<TElement>
	{

	}

	public class VisualElementChangedEventArgs : Xamarin.Forms.Platform.Android.VisualElementChangedEventArgs
	{

	}

	public abstract class ViewRenderer<TView, TNativeView> : UIView
	{

		public Element Element; 
		//
		// Properties
		//
		public TNativeView Control {
			get;
			private set;
		}
		public virtual SizeRequest GetDesiredSize (double widthConstraint, double heightConstraint)
		{
			return base.Control.GetSizeRequest (widthConstraint, heightConstraint, 44, 44);
		}
		//
		// Methods
		//
		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				base.Element.FocusChangeRequested -= new EventHandler<VisualElement.FocusRequestArgs> (this.ViewOnFocusChangeRequested);
				TNativeView control = this.Control;
				control.Dispose ();
				this.Control = default(TNativeView);
			}
			base.Dispose (disposing);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			if (this.Control != null) {
				TNativeView control = this.Control;
				float arg_50_0 = 0;
				float arg_50_1 = 0;
				TView element = base.Element;
				float arg_50_2 = (float)element.Width;
				TView element2 = base.Element;
				control.Frame = new RectangleF (arg_50_0, arg_50_1, arg_50_2, (float)element2.Height);
			}
		}

		protected virtual void OnElementChanged (ElementChangedEventArgs<TView> e)
		{
			base.OnElementChanged (e);
			e.NewElement.FocusChangeRequested += new EventHandler<VisualElement.FocusRequestArgs> (this.ViewOnFocusChangeRequested);
		}
		//protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)

		protected virtual void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (this.Control != null) {
				if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName) {
					if (this.Control is UIControl) {
						UIControl arg_5D_0 = this.Control as UIControl;
						TView element = base.Element;
						arg_5D_0.Enabled = element.IsEnabled;
					}
				}
				else {
					if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName) {
						TView element2 = base.Element;
						this.SetBackgroundColor (element2.BackgroundColor);
					}
				}
			}
			base.OnElementPropertyChanged (sender, e);
		}

		protected virtual void SetBackgroundColor (Color color)
		{
			if (this.Control == null) {
				return;
			}
			if (color == Color.Default) {
				TNativeView control = this.Control;
				control.BackgroundColor = this.defaultColor;
				return;
			}
			TNativeView control2 = this.Control;
			control2.BackgroundColor = color.ToUIColor ();
		}

		protected void SetNativeControl (TNativeView uiview)
		{
			this.defaultColor = uiview.BackgroundColor;
			this.Control = uiview;
			TView element = base.Element;
			if (element.BackgroundColor != Color.Default) {
				TView element2 = base.Element;
				this.SetBackgroundColor (element2.BackgroundColor);
			}
			this.AddSubview (uiview);
			if (this.Control is UIControl) {
				UIControl arg_96_0 = this.Control as UIControl;
				TView element3 = base.Element;
				arg_96_0.Enabled = element3.IsEnabled;
			}
			base.Element.SendViewInitialized (uiview);
		}

		public override SizeF SizeThatFits (SizeF size)
		{
			TNativeView control = this.Control;
			return control.SizeThatFits (size);
		}

		private void ViewOnFocusChangeRequested (object sender, FocusRequestArgs focusRequestArgs)
		{
			if (this.Control == null) {
				return;
			}
			bool arg_41_1;
			if (!focusRequestArgs.Focus) {
				TNativeView control = this.Control;
				arg_41_1 = control.ResignFirstResponder ();
			}
			else {
				TNativeView control2 = this.Control;
				arg_41_1 = control2.BecomeFirstResponder ();
			}
			focusRequestArgs.Result = arg_41_1;
		}
	}

	public class ListViewRenderer : ViewRenderer<ListView, UITableView>
	{
		//
		// Methods
		//
		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				this.insetTracker.Dispose ();
			}
			if (base.Element != null) {
				base.Element.TemplatedItems.CollectionChanged -= new NotifyCollectionChangedEventHandler (this.OnCollectionChanged);
				base.Element.TemplatedItems.GroupedCollectionChanged -= new NotifyCollectionChangedEventHandler (this.OnGroupedCollectionChanged);
			}
			base.Dispose (disposing);
		}




		private NSIndexPath[] GetPaths (int section, int index, int count)
		{
			NSIndexPath[] array = new NSIndexPath[count];
			for (int i = 0; i < array.Length; i++) {
				array [i] = NSIndexPath.FromRowSection (index + i, section);
			}
			return array;
		}

		private void OnCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			this.UpdateItems (e, 0, true);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged (e);
			ListView newElement = e.NewElement;
			newElement.TemplatedItems.CollectionChanged += new NotifyCollectionChangedEventHandler (this.OnCollectionChanged);
			newElement.TemplatedItems.GroupedCollectionChanged += new NotifyCollectionChangedEventHandler (this.OnGroupedCollectionChanged);
			UITableView table = new UITableView ();
			this.SetRowHeight (table);
			table.Source = (this.dataSource = (newElement.HasUnevenRows ? new ListViewRenderer.UnevenListViewDataSource (newElement, table) : new ListViewRenderer.ListViewDataSource (newElement, table)));
			base.SetNativeControl (table);
			if (newElement.SelectedItem != null) {
				this.dataSource.OnItemSelected (null, new SelectedItemChangedEventArgs (newElement.SelectedItem));
			}
			this.insetTracker = new KeyboardInsetTracker (table, () => table.Window, delegate (UIEdgeInsets insets) {
				UIScrollView arg_15_0 = table;
				table.ScrollIndicatorInsets = insets;
				arg_15_0.ContentInset = insets;
			});
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == ListView.RowHeightProperty.PropertyName) {
				this.SetRowHeight (base.Control);
				return;
			}
			if (e.PropertyName == ListView.IsGroupingEnabledProperty.PropertyName) {
				this.dataSource.UpdateGrouping ();
				return;
			}
			if (e.PropertyName == ListView.HasUnevenRowsProperty.PropertyName) {
				base.Control.Source = (this.dataSource = (base.Element.HasUnevenRows ? new ListViewRenderer.UnevenListViewDataSource (this.dataSource) : new ListViewRenderer.ListViewDataSource (this.dataSource)));
			}
		}

		class TemplatedItemsList<A,B> : Dictionary<A,B>
		{
		}

		private void OnGroupedCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			TemplatedItemsList<ItemsView<Cell>, Cell> templatedItemsList = (TemplatedItemsList<ItemsView<Cell>, Cell>)sender;
			int section = base.Element.TemplatedItems.IndexOf (templatedItemsList.HeaderContent);
			this.UpdateItems (e, section, false);
		}

		private void QueueCount (NotifyCollectionChangedEventArgs e, int section)
		{
			NotifyCollectionChangedEventArgsEx notifyCollectionChangedEventArgsEx = (NotifyCollectionChangedEventArgsEx)e;
			Queue<int> countQueue = this.dataSource.GetCountQueue (section);
			countQueue.Enqueue (notifyCollectionChangedEventArgsEx.Count);
		}

		private void SetRowHeight (UITableView table)
		{
			int rowHeight = base.Element.RowHeight;
			table.RowHeight = (float)((rowHeight <= 0) ? 44 : rowHeight);
		}

		private void UpdateItems (NotifyCollectionChangedEventArgs e, int section, bool resetWhenGrouped)
		{
			bool flag = resetWhenGrouped && base.Element.IsGroupingEnabled;
			switch (e.get_Action ()) {
			case 0:
				if (e.get_NewStartingIndex () != -1 && !flag) {
					this.QueueCount (e, section);
					base.Control.BeginUpdates ();
					base.Control.InsertRows (this.GetPaths (section, e.get_NewStartingIndex (), e.get_NewItems ().Count), UITableViewRowAnimation.Automatic);
					base.Control.EndUpdates ();
					return;
				}
				break;
			case 1:
				if (e.get_OldStartingIndex () != -1 && !flag) {
					this.QueueCount (e, section);
					base.Control.BeginUpdates ();
					base.Control.DeleteRows (this.GetPaths (section, e.get_OldStartingIndex (), e.get_OldItems ().Count), UITableViewRowAnimation.Automatic);
					base.Control.EndUpdates ();
					return;
				}
				break;
			case 2:
				if (e.get_OldStartingIndex () != -1 && !flag) {
					this.QueueCount (e, section);
					base.Control.BeginUpdates ();
					base.Control.ReloadRows (this.GetPaths (section, e.get_OldStartingIndex (), e.get_OldItems ().Count), UITableViewRowAnimation.Automatic);
					base.Control.EndUpdates ();
					return;
				}
				break;
			case 3:
				if (e.get_OldStartingIndex () != -1 && e.get_NewStartingIndex () != -1 && !flag) {
					this.QueueCount (e, section);
					base.Control.BeginUpdates ();
					for (int i = 0; i < e.get_OldItems ().Count; i++) {
						int num = e.get_OldStartingIndex ();
						int num2 = e.get_NewStartingIndex ();
						if (e.get_NewStartingIndex () < e.get_OldStartingIndex ()) {
							num += i;
							num2 += i;
						}
						base.Control.MoveRow (NSIndexPath.FromRowSection (num, section), NSIndexPath.FromRowSection (num2, section));
					}
					base.Control.EndUpdates ();
					return;
				}
				break;
			case 4:
				break;
			default:
				return;
			}
			if (!flag) {
				Queue<int> countQueue = this.dataSource.GetCountQueue (section);
				countQueue.Clear ();
			}
			else {
				this.dataSource.ClearCounts ();
			}
			base.Control.ReloadData ();
		}

		//
		// Nested Types
		//
		internal class ListViewDataSource : UITableViewSource
		{
			public ListViewDataSource (ListViewRenderer.ListViewDataSource source) :base()
			{
				this.counts = new Dictionary<int, Queue<int>> ();
			//	base..ctor ();
				this.list = source.list;
				this.uiTableView = source.uiTableView;
				this.defaultSectionHeight = source.defaultSectionHeight;
				this.counts = source.counts;
				this.fromNative = source.fromNative;
			}

			public ListViewDataSource (ListView list, UITableView uiTableView): base()
			{
				this.counts = new Dictionary<int, Queue<int>> ();
			//	base..ctor ();
				this.uiTableView = uiTableView;
				this.defaultSectionHeight = uiTableView.SectionHeaderHeight;
				this.list = list;
				this.list.ItemSelected += new EventHandler<SelectedItemChangedEventArgs> (this.OnItemSelected);
				this.UpdateShortNameListener ();
			}

			public void UpdateGrouping ()
			{
				this.UpdateShortNameListener ();
				this.uiTableView.ReloadData ();
			}

			public Queue<int> GetCountQueue (int section)
			{
				Queue<int> result;
				if (!this.counts.TryGetValue (section, out result)) {
					result = (this.counts [section] = new Queue<int> ());
				}
				return result;
			}

			public void ClearCounts ()
			{
				this.counts.Clear ();
			}

			public override int NumberOfSections (UITableView tableView)
			{
				if (!this.list.IsGroupingEnabled) {
					return 1;
				}
				Queue<int> queue;
				if (this.counts.TryGetValue (0, out queue) && queue.Count > 0) {
					return queue.Dequeue ();
				}
				return this.list.TemplatedItems.Count;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				Queue<int> queue;
				if (this.counts.TryGetValue (section, out queue) && queue.Count > 0) {
					return queue.Dequeue ();
				}
				if (this.list.IsGroupingEnabled) {
					IList list = (IList)((IList)this.list.TemplatedItems) [section];
					return list.Count;
				}
				return this.list.TemplatedItems.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				Cell cellForPath = this.GetCellForPath (indexPath);
				CellRenderer cellRenderer = (CellRenderer)Registrar.Registered.GetHandler (cellForPath.GetType ());
				return cellRenderer.GetCell (cellForPath, tableView);
			}

			public override float GetHeightForHeader (UITableView tableView, int section)
			{
				if (this.list.IsGroupingEnabled) {
					Cell cell = this.list.TemplatedItems [section];
					float num = (float)cell.RenderHeight;
					if (num == -1) {
						num = this.defaultSectionHeight;
					}
					return num;
				}
				return 0;
			}

			public override UIView GetViewForHeader (UITableView tableView, int section)
			{
				if (this.list.IsGroupingEnabled && this.list.GroupHeaderTemplate != null) {
					Cell cell = this.list.TemplatedItems [section];
					CellRenderer cellRenderer = (CellRenderer)Registrar.Registered.GetHandler (cell.GetType ());
					return cellRenderer.GetCell (cell, tableView);
				}
				return null;
			}

			public override string TitleForHeader (UITableView tableView, int section)
			{
				if (!this.list.IsGroupingEnabled) {
					return null;
				}
				TemplatedItemsList<ItemsView<Cell>, Cell> sectionList = this.GetSectionList (section);
				sectionList.PropertyChanged -= new PropertyChangedEventHandler (this.OnSectionPropertyChanged);
				sectionList.PropertyChanged += new PropertyChangedEventHandler (this.OnSectionPropertyChanged);
				return sectionList.Name;
			}

			private void OnSectionPropertyChanged (object sender, PropertyChangedEventArgs e)
			{
				NSIndexPath indexPathForSelectedRow = this.uiTableView.IndexPathForSelectedRow;
				TemplatedItemsList<ItemsView<Cell>, Cell> templatedItemsList = (TemplatedItemsList<ItemsView<Cell>, Cell>)sender;
				int num = ((IList)this.list.TemplatedItems).IndexOf (templatedItemsList);
				if (num == -1) {
					templatedItemsList.PropertyChanged -= new PropertyChangedEventHandler (this.OnSectionPropertyChanged);
					return;
				}
				this.uiTableView.ReloadSections (NSIndexSet.FromIndex (num), UITableViewRowAnimation.Automatic);
				this.uiTableView.SelectRow (indexPathForSelectedRow, false, UITableViewScrollPosition.None);
			}

			public override string[] SectionIndexTitles (UITableView tableView)
			{
				if (this.list.TemplatedItems.ShortNames == null) {
					return null;
				}
				return this.list.TemplatedItems.ShortNames.ToArray<string> ();
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				this.fromNative = true;
				this.list.NotifyRowTapped (indexPath.Section, indexPath.Row);
			}

			public void OnItemSelected (object sender, SelectedItemChangedEventArgs eventArg)
			{
				if (this.fromNative) {
					this.fromNative = false;
					return;
				}
				Tuple<int, int> groupAndIndexOfItem = this.list.TemplatedItems.GetGroupAndIndexOfItem (eventArg.SelectedItem);
				if (groupAndIndexOfItem.get_Item1 () == -1 || groupAndIndexOfItem.get_Item2 () == -1) {
					NSIndexPath indexPathForSelectedRow = this.uiTableView.IndexPathForSelectedRow;
					if (indexPathForSelectedRow != null) {
						this.uiTableView.DeselectRow (indexPathForSelectedRow, true);
					}
					return;
				}
				this.uiTableView.SelectRow (NSIndexPath.FromRowSection (groupAndIndexOfItem.get_Item2 (), groupAndIndexOfItem.get_Item1 ()), true, UITableViewScrollPosition.Middle);
			}

			protected Cell GetCellForPath (NSIndexPath indexPath)
			{
				TemplatedItemsList<ItemsView<Cell>, Cell> templatedItemsList = this.list.TemplatedItems;
				if (this.list.IsGroupingEnabled) {
					templatedItemsList = (TemplatedItemsList<ItemsView<Cell>, Cell>)((IList)templatedItemsList) [indexPath.Section];
				}
				return templatedItemsList [indexPath.Row];
			}

			private TemplatedItemsList<ItemsView<Cell>, Cell> GetSectionList (int section)
			{
				return (TemplatedItemsList<ItemsView<Cell>, Cell>)((IList)this.list.TemplatedItems) [section];
			}

			private void OnShortNamesCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
			{
				this.uiTableView.ReloadSectionIndexTitles ();
			}

			private void UpdateShortNameListener ()
			{
				if (this.list.IsGroupingEnabled) {
					if (this.list.TemplatedItems.ShortNames != null) {
						((INotifyCollectionChanged)this.list.TemplatedItems.ShortNames).add_CollectionChanged (new NotifyCollectionChangedEventHandler (this.OnShortNamesCollectionChanged));
						return;
					}
				}
				else {
					if (this.list.TemplatedItems.ShortNames != null) {
						((INotifyCollectionChanged)this.list.TemplatedItems.ShortNames).remove_CollectionChanged (new NotifyCollectionChangedEventHandler (this.OnShortNamesCollectionChanged));
					}
				}
			}
		}

		internal class UnevenListViewDataSource : ListViewRenderer.ListViewDataSource
		{
			public UnevenListViewDataSource (ListView list, UITableView uiTableView) : base (list, uiTableView)
			{
			}

			public UnevenListViewDataSource (ListViewRenderer.ListViewDataSource source) : base (source)
			{
			}

			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				Cell cellForPath = base.GetCellForPath (indexPath);
				return (float)cellForPath.RenderHeight;
			}
		}
	}

	public class EntryRenderer : ViewRenderer<Entry, UITextField>
	{
		//
		// Constructors
		//
		public EntryRenderer ()
		{
			this.Frame = new RectangleF (0, 20, 320, 40);
		}

		//
		// Methods
		//
		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				MessagingCenter.Unsubscribe<IVisualElementRenderer> (this, "Xamarin.ResignFirstResponder");
			}
			base.Dispose (disposing);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			Entry view = e.NewElement;
			base.OnElementChanged (e);
			UITextField textField;
			base.SetNativeControl (textField = new UITextField (RectangleF.Empty));
			textField.BorderStyle = UITextBorderStyle.RoundedRect;
			this.UpdatePlaceholder ();
			this.UpdatePassword ();
			this.UpdateText ();
			this.UpdateColor ();
			this.UpdateKeyboard ();
			textField.EditingChanged += delegate (object sender, EventArgs a) {
				view.SetValueCore (Entry.TextProperty, textField.Text, false, false, true);
			};
			textField.ShouldReturn = new UITextFieldCondition (this.OnShouldReturn);
			textField.EditingDidBegin += delegate (object sender, EventArgs args) {
				view.IsFocused = true;
			};
			textField.EditingDidEnd += delegate (object sender, EventArgs args) {
				view.IsFocused = false;
			};
			this.defaultTextColor = textField.TextColor;
			MessagingCenter.Subscribe<IVisualElementRenderer> (this, "Xamarin.ResignFirstResponder", delegate (IVisualElementRenderer sender) {
				if (textField.IsFirstResponder) {
					textField.ResignFirstResponder ();
				}
			}, null);
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			Entry arg_06_0 = base.Element;
			if (e.PropertyName == Entry.PlaceholderProperty.PropertyName) {
				this.UpdatePlaceholder ();
			}
			else {
				if (e.PropertyName == Entry.IsPasswordProperty.PropertyName) {
					this.UpdatePassword ();
				}
				else {
					if (e.PropertyName == Entry.TextProperty.PropertyName) {
						this.UpdateText ();
					}
					else {
						if (e.PropertyName == Entry.TextColorProperty.PropertyName) {
							this.UpdateColor ();
						}
						else {
							if (e.PropertyName == Xamarin.Forms.InputView.KeyboardProperty.PropertyName) {
								this.UpdateKeyboard ();
							}
						}
					}
				}
			}
			base.OnElementPropertyChanged (sender, e);
		}

		private bool OnShouldReturn (UITextField view)
		{
			base.Control.ResignFirstResponder ();
			base.Element.SendCompleted ();
			return true;
		}

		private void UpdateColor ()
		{
			if (base.Element.TextColor == Color.Default) {
				base.Control.TextColor = this.defaultTextColor;
				return;
			}
			base.Control.TextColor = base.Element.TextColor.ToUIColor ();
		}

		private void UpdateKeyboard ()
		{
			base.Control.ApplyKeyboard (base.Element.Keyboard);
		}

		private void UpdatePassword ()
		{
			if (base.Element.IsPassword && base.Control.IsFirstResponder) {
				base.Control.Enabled = false;
				base.Control.SecureTextEntry = true;
				base.Control.Enabled = base.Element.IsEnabled;
				base.Control.BecomeFirstResponder ();
				return;
			}
			base.Control.SecureTextEntry = base.Element.IsPassword;
		}

		private void UpdatePlaceholder ()
		{
			base.Control.Placeholder = base.Element.Placeholder;
		}

		private void UpdateText ()
		{
			if (base.Control.Text != base.Element.Text) {
				base.Control.Text = base.Element.Text;
			}
		}
	}

	public class ScrollViewRenderer : UIScrollView, IVisualElementRenderer, IRegisterable
	{
		//
		// Properties
		//
		public VisualElement Element {
			get;
			private set;
		}

		public UIView NativeView {
			get {
				return this;
			}
		}

		public UIViewController ViewController {
			get {
				return null;
			}
		}

		//
		// Constructors
		//
		public ScrollViewRenderer () : base (RectangleF.Empty)
		{
		}

		//
		// Methods
		//
		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				this.packager.Dispose ();
				this.tracker.Dispose ();
				this.events.Dispose ();
				this.insetTracker.Dispose ();
				this.Element.PropertyChanged -= new PropertyChangedEventHandler (this.HandlePropertyChanged);
				this.Element = null;
			}
			base.Dispose (disposing);
		}

		public SizeRequest GetDesiredSize (double widthConstraint, double heightConstraint)
		{
			return this.NativeView.GetSizeRequest (widthConstraint, heightConstraint, 44, 44);
		}

		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == ScrollView.ContentSizeProperty.PropertyName) {
				this.UpdateContentSize ();
				return;
			}
			if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName) {
				this.UpdateBackgroundColor ();
			}
		}

		protected virtual void OnElementChanged (VisualElementChangedEventArgs e)
		{
			EventHandler<VisualElementChangedEventArgs> elementChanged = this.ElementChanged;
			if (elementChanged != null) {
				elementChanged (this, e);
			}
		}

		public void SetElement (VisualElement element)
		{
			this.insetTracker = new KeyboardInsetTracker (this, () => this.Window, delegate (UIEdgeInsets insets) {
				this.ScrollIndicatorInsets = insets;
				this.ContentInset = insets;
			});
			VisualElement element2 = this.Element;
			this.Element = element;
			this.DelaysContentTouches = true;
			this.UpdateContentSize ();
			this.Element.PropertyChanged += new PropertyChangedEventHandler (this.HandlePropertyChanged);
			this.packager = new VisualElementPackager (this);
			this.packager.Load ();
			this.tracker = new VisualElementTracker (this);
			this.events = new EventTracker (this);
			this.events.LoadEvents (this);
			this.UpdateBackgroundColor ();
			this.OnElementChanged (new VisualElementChangedEventArgs (element2, element));
		}

		public void SetElementSize (Size size)
		{
			this.Element.Layout (new Rectangle (this.Element.X, this.Element.Y, size.Width, size.Height));
		}

		private void UpdateBackgroundColor ()
		{
			this.BackgroundColor = this.Element.BackgroundColor.ToUIColor (Color.Transparent);
		}

		private void UpdateContentSize ()
		{
			this.ContentSize = ((ScrollView)this.Element).ContentSize.ToSizeF ();
		}

		//
		// Events
		//
		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
	}

	public class WebViewRenderer : UIWebView, IVisualElementRenderer, IRegisterable
	{
		//
		// Properties
		//
		public VisualElement Element {
			get;
			private set;
		}

		public UIView NativeView {
			get {
				return this;
			}
		}

		public UIViewController ViewController {
			get {
				return null;
			}
		}

		//
		// Constructors
		//
		public WebViewRenderer () : base (RectangleF.Empty)
		{
		}

		//
		// Methods
		//
		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				this.Element.PropertyChanged -= new PropertyChangedEventHandler (this.HandlePropertyChanged);
			}
			base.Dispose (disposing);
		}

		public SizeRequest GetDesiredSize (double widthConstraint, double heightConstraint)
		{
			return this.NativeView.GetSizeRequest (widthConstraint, heightConstraint, 44, 44);
		}

		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == WebView.SourceProperty.PropertyName) {
				this.Load ();
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			this.ScrollView.Frame = this.Bounds;
		}

		private void Load ()
		{
			if (((WebView)this.Element).Source != null) {
				((WebView)this.Element).Source.Load (this);
			}
		}

		public void LoadHtml (string html, string baseUrl)
		{
			if (html != null) {
				this.LoadHtmlString (html, (baseUrl == null) ? null : new NSUrl (baseUrl));
			}
		}

		public void LoadUrl (string url)
		{
			this.LoadRequest (new NSUrlRequest (new NSUrl (url)));
		}

		protected virtual void OnElementChanged (VisualElementChangedEventArgs e)
		{
			EventHandler<VisualElementChangedEventArgs> elementChanged = this.ElementChanged;
			if (elementChanged != null) {
				elementChanged (this, e);
			}
		}

		public void SetElement (VisualElement element)
		{
			VisualElement element2 = this.Element;
			this.Element = element;
			this.Element.PropertyChanged += new PropertyChangedEventHandler (this.HandlePropertyChanged);
			base.Delegate = new WebViewRenderer.CustomWebViewDelegate ();
			this.BackgroundColor = UIColor.Clear;
			this.tracker = new VisualElementTracker (this);
			this.packager = new VisualElementPackager (this);
			this.packager.Load ();
			this.events = new EventTracker (this);
			this.Load ();
			this.OnElementChanged (new VisualElementChangedEventArgs (element2, element));
		}

		public void SetElementSize (Size size)
		{
			this.Element.Layout (new Rectangle (this.Element.X, this.Element.Y, size.Width, size.Height));
		}

		//
		// Events
		//
		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		//
		// Nested Types
		//
		private class CustomWebViewDelegate : UIWebViewDelegate
		{
			public override void LoadStarted (UIWebView webView)
			{
			}

			public override void LoadingFinished (UIWebView webView)
			{
			}
		}
	}

	public class LabelRenderer : ViewRenderer<Label, UILabel>
	{
		//
		// Methods
		//
		public override SizeRequest GetDesiredSize (double widthConstraint, double heightConstraint)
		{
			SizeRequest desiredSize = base.GetDesiredSize (widthConstraint, heightConstraint);
			desiredSize.Minimum = new Size (Math.Min (10, desiredSize.Request.Width), desiredSize.Request.Height);
			return desiredSize;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			if (base.Control == null) {
				return;
			}
			base.Control.SizeToFit ();
			float num = Math.Min (this.Bounds.Height, base.Control.Bounds.Height);
			float y = 0;
			switch (base.Element.YAlign) {
			case TextAlignment.Start:
				y = 0;
				break;
			case TextAlignment.Center:
				y = (float)(base.Element.Height / 2 - (double)(num / 2));
				break;
			case TextAlignment.End:
				y = (float)(base.Element.Height - (double)num);
				break;
			}
			base.Control.Frame = new RectangleF (0, y, (float)base.Element.Width, num);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);
			base.SetNativeControl (new UILabel (RectangleF.Empty) {
				BackgroundColor = UIColor.Clear
			});
			this.UpdateText ();
			this.UpdateLineBreakMode ();
			this.UpdateAlignment ();
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == Label.XAlignProperty.PropertyName) {
				this.UpdateAlignment ();
				return;
			}
			if (e.PropertyName == Label.YAlignProperty.PropertyName) {
				this.LayoutSubviews ();
				return;
			}
			if (e.PropertyName == Label.TextColorProperty.PropertyName) {
				this.UpdateText ();
				return;
			}
			if (e.PropertyName == Label.FontProperty.PropertyName) {
				this.UpdateText ();
				return;
			}
			if (e.PropertyName == Label.TextProperty.PropertyName) {
				this.UpdateText ();
				return;
			}
			if (e.PropertyName == Label.FormattedTextProperty.PropertyName) {
				this.UpdateText ();
				return;
			}
			if (e.PropertyName == Label.LineBreakModeProperty.PropertyName) {
				this.UpdateLineBreakMode ();
			}
		}

		protected override void SetBackgroundColor (Color color)
		{
			if (color == Color.Default) {
				this.BackgroundColor = UIColor.Clear;
				return;
			}
			this.BackgroundColor = color.ToUIColor ();
		}

		private void UpdateAlignment ()
		{
			if (base.Element.XAlign == TextAlignment.Start) {
				base.Control.TextAlignment = UITextAlignment.Left;
				return;
			}
			if (base.Element.XAlign == TextAlignment.End) {
				base.Control.TextAlignment = UITextAlignment.Right;
				return;
			}
			base.Control.TextAlignment = UITextAlignment.Center;
		}

		private void UpdateColor ()
		{
			if (base.Element.TextColor == Color.Default) {
				base.Control.TextColor = UIColor.Black;
				return;
			}
			base.Control.TextColor = base.Element.TextColor.ToUIColor ();
		}

		private void UpdateFont (bool updateDefault = true)
		{
			UIFont font;
			if (base.Element.Font != Font.Default && (font = base.Element.Font.ToUIFont ()) != null) {
				base.Control.Font = font;
				return;
			}
			if (base.Element.Font != Font.Default) {
				Log.Warning ("Font", "Can't load font {0}:{1} on this platform/device. reverting to default", new object[] {
					base.Element.Font.FontFamily,
					base.Element.Font.FontSize
				});
				return;
			}
			if (updateDefault) {
				base.Control.Font = UIFont.SystemFontOfSize (17);
			}
		}

		private void UpdateLineBreakMode ()
		{
			switch (base.Element.LineBreakMode) {
			case LineBreakMode.NoWrap:
				base.Control.LineBreakMode = UILineBreakMode.Clip;
				base.Control.Lines = 1;
				return;
			case LineBreakMode.WordWrap:
				base.Control.LineBreakMode = UILineBreakMode.WordWrap;
				base.Control.Lines = 0;
				return;
			case LineBreakMode.CharacterWrap:
				base.Control.LineBreakMode = UILineBreakMode.CharacterWrap;
				base.Control.Lines = 0;
				return;
			case LineBreakMode.HeadTruncation:
				base.Control.LineBreakMode = UILineBreakMode.HeadTruncation;
				base.Control.Lines = 1;
				return;
			case LineBreakMode.TailTruncation:
				base.Control.LineBreakMode = UILineBreakMode.TailTruncation;
				base.Control.Lines = 1;
				return;
			case LineBreakMode.MiddleTruncation:
				base.Control.LineBreakMode = UILineBreakMode.MiddleTruncation;
				base.Control.Lines = 1;
				return;
			default:
				return;
			}
		}

		private void UpdateText ()
		{
			FormattedString formattedString = base.Element.FormattedText ?? base.Element.Text;
			base.Control.AttributedText = formattedString.ToAttributed (base.Element.Font, base.Element.TextColor);
			this.LayoutSubviews ();
		}
	}
}
#endif
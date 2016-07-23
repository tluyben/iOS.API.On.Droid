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

using Foundation; 

namespace UIKit
{



	public enum UIKeyboardType
	{
		NumberPad, DecimalPad
	}

	public enum UITextSpellCheckingType
	{
		No
	}

	public enum UITextAutocapitalizationType
	{
		None,
		Words,
		Sentences,
		AllCharacters
	}

	public enum UITextAutocorrectionType
	{
		Default,
		No,
		Yes
	}

	public enum UIReturnKeyType
	{
		Default,
		Go,
		Google,
		Join,
		Next,
		Route,
		Search,
		Send,
		Yahoo,
		Done,
		EmergencyCall
	}

	public enum UITextFieldViewMode
	{
		Always
	}

	public delegate bool UITextFieldCondition (UITextField textField);
	public delegate bool UITextFieldChange (UITextField textField, NSRange range, string replacementString);

	public class AndroidEditText : EditText, UIConnectingView
	{
		public UIView UIOwnerView { get; set;}

		public AndroidEditText (Context c) : base (c)
		{
			this.SetPadding (0, -5, 0, 0); 
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

	public class UITextField : UIView 
	{
		public string Text {
			get {
				return ((AndroidEditText)__View).Text; 

			} 
			set {
				((AndroidEditText)__View).Text = value; 
			}
		}


		public UIView LeftView {
			get;
			set;
		}

		public UITextFieldViewMode LeftViewMode {
			get;
			set; 
		}


		Dictionary<EventHandler, EventHandler<Android.Text.TextChangedEventArgs>> EditingChangedHandlers = new Dictionary<EventHandler, EventHandler<Android.Text.TextChangedEventArgs>>(); 
		public event EventHandler EditingChanged 
		{
			add {
				EventHandler x = value; 
				EventHandler<Android.Text.TextChangedEventArgs> h = (object sender, Android.Text.TextChangedEventArgs e) => {
					x(sender, new EventArgs()); 
				};
				((EditText)__View).TextChanged += h; 
				EditingChangedHandlers.Add(x, h); 
			}
			remove {
				EventHandler x = value; 
				EventHandler<Android.Text.TextChangedEventArgs> h = EditingChangedHandlers [x]; 
				((EditText)__View).TextChanged -= h; 

			}
		}

		public void BecomeFirstResponder() {

		}

		public UIColor TextColor 
		{
			set{
				((EditText)__View).SetTextColor (value.CGColor); 
			}
		}

		protected UITextAlignment _TextAlignment = UITextAlignment.Center; 
		public UITextAlignment TextAlignment 
		{
			get {
				/*switch (((AndroidLabel)__View).Gravity) {
				case GravityFlags.Left: 
					return UITextAlignment.Left; 
					break;
				case GravityFlags.Right: 
					return UITextAlignment.Right; 
					break;
				case GravityFlags.Center: 
					return UITextAlignment.Center; 
					break;
				}*/ 
				return _TextAlignment; 
			}

			set {
				switch (value) {
				case UITextAlignment.Left:
					((EditText)__View).Gravity = GravityFlags.Left;
					break;
				case UITextAlignment.Right:
					((EditText)__View).Gravity = GravityFlags.Right;
					break;
				case UITextAlignment.Center:
					((EditText)__View).Gravity = GravityFlags.Center;
					break;
				}
				_TextAlignment = value; 
			}
		}


		public void ResignFirstResponder() {

		}

		public virtual bool SecureTextEntry {
			
			set {
				if (value)
					((EditText)__View).InputType = Android.Text.InputTypes.TextVariationPassword;
				else
					((EditText)__View).InputType = Android.Text.InputTypes.TextVariationNormal; 
			}
		}
		public UITextField () : base(new AndroidEditText (UIViewController.Context))
		{

			__View.SetBackgroundColor (Color.Transparent); 
		}
		public UITextField (CoreGraphics.CGRect rect) : base(new AndroidEditText (UIViewController.Context))
		{

			__View.SetBackgroundColor (Color.Transparent); 
		}

		public NSAttributedString AttributedPlaceholder {
			get; set; 
		}

		string _Placeholder ;
		public string Placeholder {
			get {
				return _Placeholder;
			} 
			set {
				_Placeholder = value; 
			}
		}

		UIReturnKeyType _ReturnKeyType; 
		public UIReturnKeyType ReturnKeyType {
			get {
				return _ReturnKeyType; 
			}
			set {
				_ReturnKeyType = value; 
			}
		}

		UITextAutocorrectionType _AutocorrectionType; 
		public UITextAutocorrectionType AutocorrectionType {
			get {
				return _AutocorrectionType; 
			} 
			set {
				switch (value) {
				case UITextAutocorrectionType.No:
					{
						((AndroidEditText)__View).InputType |= Android.Text.InputTypes.TextFlagNoSuggestions; 
						break; 
					}
				}

				_AutocorrectionType = value; 
			}
		}


		UITextAutocapitalizationType _AutocapitalizationType; 
		public UITextAutocapitalizationType AutocapitalizationType {
			get {
				return _AutocapitalizationType; 
			}
			set {
				switch (value) {
				case UITextAutocapitalizationType.AllCharacters: {

						// apparently this is deprecated and does not work on new (kitkat) phones
						((AndroidEditText)__View).InputType |= Android.Text.InputTypes.TextFlagCapCharacters; 
						((AndroidEditText)__View).InputType |= Android.Text.InputTypes.TextFlagCapWords; 


						bool changing = false; 
						// TODO: Weird  + fix! 
						((AndroidEditText)__View).AfterTextChanged += (object sender, Android.Text.AfterTextChangedEventArgs e) => {
							if (!changing && ((AndroidEditText)__View).Text != ((AndroidEditText)__View).Text.ToUpper()) {
								changing = true; 
								string r = ((AndroidEditText)__View).Text; 
								((AndroidEditText)__View).Text = "";
								((AndroidEditText)__View).Append(r.ToUpper()); 
								changing = false;
							}


						};

						break;
					}
				}



				_AutocapitalizationType = value; 
			}
		}

		UITextSpellCheckingType _SpellCheckingType; 
		public UITextSpellCheckingType SpellCheckingType {
			get {
				return _SpellCheckingType;
			} 
			set {
				switch (value) {
				case UITextSpellCheckingType.No:
					{
						((AndroidEditText)__View).InputType |= Android.Text.InputTypes.TextFlagNoSuggestions; 
						break; 
					}
				}

				_SpellCheckingType = value; 
			}
		}

		private UIKeyboardType _KeyboardType; 
		public UIKeyboardType KeyboardType {
			get {
				return _KeyboardType; 
			}
			set {

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
				((AndroidEditText)__View).Typeface = value.Font; 
				((AndroidEditText)__View).SetTextSize (ComplexUnitType.Dip, value.Size ); 
			}
		}

		public UITextFieldCondition ShouldReturn { get; set; }
		public UITextFieldChange ShouldChangeCharacters { get; set; }

	}
}


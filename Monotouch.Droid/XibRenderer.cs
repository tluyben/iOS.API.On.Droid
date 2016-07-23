using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Drawing; 

using System.Reflection; 

using UIKit; 
using Foundation; 
using ObjCRuntime;

namespace Internal
{


	public class XibRenderer
	{
		protected UIColor CreateColor(XElement c) {
			if (c.Attribute ("white") != null) {
				return UIColor.FromWhiteAlpha ((float)c.Attribute ("white"), (float)c.Attribute ("alpha")); 
			} else {
				return UIColor.FromRGBA ((float)c.Attribute ("red"), (float)c.Attribute ("green"), (float)c.Attribute ("blue"), 
					(float)c.Attribute ("alpha")); 
			}
		}

		protected RectangleF CreateRect(XElement e) {
			return new RectangleF (
				float.Parse((string)e.Attribute("x")), 
				float.Parse((string)e.Attribute("y")),
				float.Parse((string)e.Attribute("width")),
				float.Parse((string)e.Attribute("height"))
			); 
		}

		protected UIFont CreateFont(XElement e) {
			float size = (float)e.Attribute ("pointSize"); 
			UIFont f = null; 
			if (e.Attribute ("type") != null) {
				string type = (string)e.Attribute ("type"); 
			
				switch (type) {
				case "system": {
						f = UIFont.SystemFontOfSize (size); 
						break;
					}
				}

			} else if (e.Attribute ("name") != null) {
				f =  UIFont.FromName ((string)e.Attribute ("name"), size); 
			}

			if (f == null) {
				Console.WriteLine ("Find cannot Create font: "); 
				Console.WriteLine (e); 
			}
			return f; 
		}

		protected void AddFrame(XElement n, IView v) {
			if (n.Element ("rect") != null) {
				var e = n.Element ("rect"); 
				if ((string)e.Attribute ("key") == "frame") { // what are the other options? :) 
					v.Frame = CreateRect (e); 
				}
			}
		}

		protected void AddColor(XElement n, UIView v) {
			if (n.Element ("color") != null) {
				var e = n.Element ("color"); 
				if ((string)e.Attribute ("key") == "backgroundColor") {
					v.BackgroundColor = CreateColor (e); 
				}
			}
		}

		protected UIView CreateUIView(XElement n) {
			var v = new UIView (); 
			AddFrame (n, v); 
			AddColor (n, v); 

			return v; 
		}

		protected UILabel createUILabel(XElement n) {
			UILabel l = new UILabel ();
			AddFrame (n, l); 
			if (n.Elements ("color")!=null) {
				foreach (var c in n.Elements("color")) {
					if ((string)c.Attribute ("key") == "textColor") {
						l.TextColor = CreateColor (c); 
					} else if ((string)c.Attribute ("key") == "shadowColor") {
						l.ShadowColor = CreateColor (c); 

					}

				}
			}
			if (n.Element ("fontDescription") != null) {
				l.Font = CreateFont (n.Element ("fontDescription")); 
			}
			if (n.Attribute ("textAlignment") != null) {
				var align = (string)n.Attribute ("textAlignment"); 
				if (align == "right") {
					l.TextAlignment = UITextAlignment.Right; 
				} else if (align == "left") {
					l.TextAlignment = UITextAlignment.Left; 
				}

			}
			if (n.Attribute ("text") != null) {
				l.Text = (string)n.Attribute ("text"); 
			}
			return l; 
		}

		protected UIButton createUIButton(XElement n) {
			UIButton b = new UIButton (); 
			AddFrame (n, b); 

			if (n.Element ("state") != null) {
				var e = n.Element ("state"); 
				if ((string)e.Attribute ("key") == "normal") {
					if (n.Elements ("color") != null) {
						foreach (var c in n.Elements("color")) {
							if ((string)c.Element ("key") == "titleShadowColor") {
								b.SetTitleShadowColor (CreateColor (c), UIControlState.Normal); 
							}
						}

					}
				}
			}

			BindEvents (n, b); 

			return b; 
		}

		protected void BindEvents(XElement n, object b) {
			if (n.Elements ("connections") != null) {
				foreach (var e in n.Element("connections").Elements("action")) {

					BindEvent (e, b); 
				}
			}
		}

		// 		partial void submitButtonTap (MonoTouch.Foundation.NSObject sender);

		protected void InvokeEvent(string name, Type p, object sender) {
			Type _vc = ViewController.GetType(); 

			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
			                     BindingFlags.Static | BindingFlags.Instance |
				BindingFlags.DeclaredOnly; 

			MethodInfo t = _vc.GetMethod(name, new Type[]{p}); 
			if (t == null)
				t = _vc.GetMethod(name, new Type[]{typeof(NSObject)}); 
			if (t == null)
				t = _vc.GetMethod (name, flags); 

			if (t == null)
				Console.WriteLine ("Cannot find method {0} of type {1} in controller {2}", name, p.Name, _vc.Name ); 

			t.Invoke(ViewController, new object[]{((UIConnectingView)sender).UIOwnerView}); 
		}

		protected void BindEvent(XElement e, object o) {
			string etype = (string)e.Attribute ("eventType"); 
			string name = (string)e.Attribute ("selector"); 
			if (name.EndsWith (":")) {
				name = name.Substring (0, name.Length - 1); 
			}
			if (etype == "touchUpInside") {
				//((UIButton)o).BackgroundColor = UIColor.Red; 
				((UIButton)o).TouchUpInside += (object sender, EventArgs _e) => {
					InvokeEvent(name, typeof(UIButton), sender); 
				};
			}
		}

		protected UIImageView createUIImageView(XElement n) {
			UIImageView im = new UIImageView (); 
			AddFrame (n, im); 

			if (n.Attribute ("image") != null) {
				string name = (string)n.Attribute ("image"); 
				im.Image = UIImage.FromBundle (name); 
			}

			return im; 
		}
			
		protected UITextField createUITextField(XElement n) {
			UITextField tf = new UITextField (); 
			AddFrame (n, tf); 

			if (n.Element ("fontDescription") != null) {
				tf.Font = CreateFont (n.Element ("fontDescription")); 
			}
			if (n.Element ("textInputTraits") != null) {
				var x = n.Element ("textInputTraits");
				if (x.Attribute ("autocapitalizationType") != null) {
					switch ((string)x.Attribute ("autocapitalizationType")) {
					case "allCharacters":
						{
							tf.AutocapitalizationType = UITextAutocapitalizationType.AllCharacters; 
							break; 
						}
					}
				}
				if (x.Attribute ("autocorrectionType") != null) {
					switch ((string)x.Attribute ("autocorrectionType")) {
					case "no":
						{
							tf.AutocorrectionType = UITextAutocorrectionType.No;  
							break; 
						}
					}
				}
				if (x.Attribute ("returnKeyType") != null) {
					switch ((string)x.Attribute ("returnKeyType")) {
					case "done":
						{
							tf.ReturnKeyType = UIReturnKeyType.Done;  
							break; 
						}
					}
				}
			}
				
			return tf; 
		}

		protected UIScrollView createUIScrollView(XElement n) {
			UIScrollView sv = new UIScrollView (); 
			AddFrame (n, sv);

			return sv; 
		}

		protected UIView CreateView(XElement n) {
			var s = n.Name;
			UIView r = null;

			if (s == "view") {
				r = CreateUIView (n); 
			} else if (s == "label") {
				r = createUILabel (n); 
			} else if (s == "imageView") {
				r = createUIImageView (n); 
			} else if (s == "button") {
				r = createUIButton (n); 
			} else if (s == "textField") {
				r = createUITextField (n); 
			} else if (s == "scrollView") {
				r = createUIScrollView (n); 
			}

			//Console.WriteLine (n.Name);
			if (r != null) {

				if (n.Attribute ("id") != null) {
					idMapping.Add ((string)n.Attribute ("id"), r); 
				}

				if (n.Attribute ("tag") != null) {
					r.Tag = (int)n.Attribute ("tag"); 
				} else {
					r.Tag = 0; 
				}
			}
			return r; 
		}

		protected UIView Render(XElement n, UIView parent, UIView root) {
			foreach (var x in n.Elements()) {
				//Console.WriteLine (x); 
				var r = CreateView (x); 
				if (r != null) {
					if (parent == null) {
						parent = root = r; 
					} else {
						parent.Add (r); 
					}
					var svs = x.Elements ("subviews"); 
					if (svs != null && svs.Count () > 0) {
						Render (svs.First(), r, root); 
					}
				}
			}
			return root; 
		}

		// we have to call this at the end as we don't have the idMappings before
		public void ResolveOutlets(XElement _objs) {
			var ph = from _ph in _objs.Elements ("placeholder")
					where (string)_ph.Attribute("placeholderIdentifier")  == "IBFilesOwner" 
				select _ph.Element("connections"); 

			if (ph.Count() > 0) {
				//Console.WriteLine (ph.First ()); 
				foreach (var o in ph.First().Elements("outlet")) {
					string name = (string)o.Attribute ("property");
					string did = (string)o.Attribute ("destination"); 
					if (idMapping.ContainsKey (did)) {
						object _o = idMapping [did]; 


						BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | 
							BindingFlags.Static | BindingFlags.Instance | 
							BindingFlags.DeclaredOnly;

						PropertyInfo pinf = ViewController.GetType ().GetProperty (name, flags); 

						/*foreach (var ps in ViewController.GetType ().GetProperties(flags)) {
							Console.WriteLine (ps.Name); 
						}*/ 

						if (pinf != null) {
							pinf.SetValue (ViewController, _o); 
						}
					} else {
						Console.WriteLine ("Mapping not found! Should not happen.");
					}

				}

			}

		}

		public UIView Render() {


			var v = Render (_objs, null, null); ; 
			ResolveOutlets (_objs); 
			return v; 
			//return Render(_objs.
		}

		protected UIViewController ViewController; 
		protected XDocument xDoc; 
		protected XElement _objs; 
		protected Dictionary<string, IView> idMapping = new Dictionary<string, IView> (); 

		public XibRenderer (UIViewController vc, string path)
		{
			this.ViewController = vc; 

			/*var documents =
				Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			path = Path.Combine (documents, path);
*/

			if (!path.EndsWith (".xib")) {
				path += ".xib"; 
			}

			Stream input = vc.Assets.Open (path);

			string text;
			using (StreamReader sr = new StreamReader (input))
				text = sr.ReadToEnd ();

			xDoc = XDocument.Parse (text); 

			//xDoc = XDocument.Load (path); 


			_objs = xDoc.Descendants ("objects").First(); 

		


			//Console.WriteLine (_objs.Document.ToString ()); 
		}
	}
}


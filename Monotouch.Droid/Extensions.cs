using System;
using System.Json; 

/*

namespace System.Json
{
	public static class Extensions
	{
		public static bool IsJson(this string str) {
			try {
				JsonValue.Parse (str); 
				return true; 
			} catch(Exception e) {

			}
			return false; 
		}
	}
}

*/

namespace Foundation
{
	public static class _Extensions
	{



		public static bool __IsSubClassOf(this Object o1, Type o2) {
			return (o2.IsAssignableFrom (o1.GetType ()));

		}


		public static string __Reverse(this string s )
		{
			char[] charArray = s.ToCharArray();
			Array.Reverse( charArray );
			return new string( charArray );
		}
	}
}


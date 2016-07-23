# iOS.API.On.Droid
A P.O.C. implementation of a part of the iOS API implemented on top of Android. For Xamarin. 

# What?

This library allows a subset of iOS apps to be recompiled, almost without changes, to Android apps. 

# Why?

People kept asking if this would be possible and I kept thinking; why not. Also most clients wanted just exactly the same app on iOS on Android (which is not a good plan, but if there is no(t) (enough)  budget to do it properly...). 

# Status

This implementation is a proof of concept and aging a bit (legal stuff made me not release it until now). Although it was P.O.C. and written by one person (me) in a very short period of time, it has been used to recompile production iOS apps to Android without change to any of the core classes.

A subset of the iOS API was implemented only; not only because that API is huge; it also was a POC goal to convert a few apps to show it was possible. Most common controls and animations are implemented. 

# Future

I'm not continuing this project. If someone is interested, just drop me a note. 

I relased it because I myself find it annoying when companies or projects stop, the code usually just disappears. Maybe someone can use something and if not, then it'll just 'be'. 

# Documentation

I do not know what needs to be changed for newer versions of iOS; for older versions of iOS however, all that is required should be adding the MainActivity to the Activity classes and recompile. 

If any questions, just ask; I was will try to help out. 

# License

MIT License (matching open source Xamarin)


#if __vla__ 
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
using Android.Animation;
using Android.Views.Animations;


using Foundation; 

namespace UIKit.Helpers {

	public enum FlipDirection {
		LEFT_RIGHT, RIGHT_LEFT, NULL
	}

	public static class _FlipDirection {
		//public const int LEFT_RIGHT = 1; 
		//public const int RIGHT_LEFT = 2;

		public static float getStartDegreeForFirstView(this FlipDirection _this) {
			return 0;
		}

		public static float getStartDegreeForSecondView(this FlipDirection _this) {
			switch(_this) {
			case FlipDirection.LEFT_RIGHT:
				return -90;
			case FlipDirection.RIGHT_LEFT:
				return 90;
			default:
				return 0;
			}
		}

		public static  float getEndDegreeForFirstView(this FlipDirection _this) {
			switch(_this) {
			case FlipDirection.LEFT_RIGHT:
				return 90;
			case FlipDirection.RIGHT_LEFT:
				return -90;
			default:
				return 0;
			}
		}

		public static float getEndDegreeForSecondView(this FlipDirection _this) {
			return 0;
		}

		public static FlipDirection theOtherDirection(this FlipDirection _this) {
			switch(_this) {
			case FlipDirection.LEFT_RIGHT:
				return FlipDirection.RIGHT_LEFT;
			case FlipDirection.RIGHT_LEFT:
				return FlipDirection.LEFT_RIGHT;
			default:
				return FlipDirection.NULL;
			}
		}
	};



public class AnimationFactory {
	
	/**
	 * The {@code FlipDirection} enumeration defines the most typical flip view transitions: left-to-right and right-to-left. {@code FlipDirection} is used during the creation of {@link FlipAnimation} animations.
	 * 
	 * @author Ephraim A. Tekle
	 *
	 */
	
	/**
	 * Create a pair of {@link FlipAnimation} that can be used to flip 3D transition from {@code fromView} to {@code toView}. A typical use case is with {@link ViewAnimator} as an out and in transition.
	 * 
	 * NOTE: Avoid using this method. Instead, use {@link #flipTransition}.
	 *  
	 * @param fromView the view transition away from
	 * @param toView the view transition to
	 * @param dir the flip direction
	 * @param duration the transition duration in milliseconds
	 * @param interpolator the interpolator to use (pass {@code null} to use the {@link AccelerateInterpolator} interpolator) 
	 * @return
	 */
			public static Animation[] flipAnimation( View fromView,  View toView, FlipDirection dir, long duration, IInterpolator interpolator) {
		Animation[] result = new Animation[2];
		float centerX;
		float centerY;
		
			centerX = fromView.Width / 2.0f;
			centerY = fromView.Height / 2.0f; 
		
		Animation outFlip= new FlipAnimation(dir.getStartDegreeForFirstView(), dir.getEndDegreeForFirstView(), centerX, centerY, FlipAnimation.SCALE_DEFAULT, FlipAnimation.ScaleUpDownEnum.SCALE_DOWN);


			outFlip.Duration = duration;
			outFlip.FillAfter = true; 
			outFlip.Interpolator = interpolator == null ? (IInterpolator) (new AccelerateInterpolator ()) : interpolator; 

		AnimationSet outAnimation = new AnimationSet(true);
			outAnimation.AddAnimation (new AnimationSet (true)); 
		result[0] = outAnimation; 
		
		// Uncomment the following if toView has its layout established (not the case if using ViewFlipper and on first show)
		//centerX = toView.getWidth() / 2.0f;
		//centerY = toView.getHeight() / 2.0f; 
		
		Animation inFlip = new FlipAnimation(dir.getStartDegreeForSecondView(), dir.getEndDegreeForSecondView(), centerX, centerY, FlipAnimation.SCALE_DEFAULT, FlipAnimation.ScaleUpDownEnum.SCALE_UP);
			inFlip.Duration = duration; 
			inFlip.FillAfter = true; 
			inFlip.Interpolator = interpolator == null ? (IInterpolator) (new AccelerateInterpolator ()) : interpolator;
			inFlip.StartOffset = duration; 
		
		AnimationSet inAnimation = new AnimationSet(true); 
			inAnimation.AddAnimation (inFlip); 
		result[1] = inAnimation; 
		
		return result;
		
	}
	
	/**
	 * Flip to the next view of the {@code ViewAnimator}'s subviews. A call to this method will initiate a {@link FlipAnimation} to show the next View.  
	 * If the currently visible view is the last view, flip direction will be reversed for this transition.
	 *  
	 * @param viewAnimator the {@code ViewAnimator}
	 * @param dir the direction of flip
	 */
	public static void flipTransition(ViewAnimator viewAnimator, FlipDirection dir) {   
		
			View fromView = viewAnimator.CurrentView;
			int currentIndex = viewAnimator.DisplayedChild;
			int nextIndex = (currentIndex + 1) % viewAnimator.ChildCount; 
		
			View toView = viewAnimator.GetChildAt(nextIndex);

		Animation[] animc = AnimationFactory.flipAnimation(fromView, toView, (nextIndex < currentIndex?dir.theOtherDirection():dir), 500, null);
  
			viewAnimator.OutAnimation = animc [0]; 
			viewAnimator.InAnimation = animc [1]; 
		
			viewAnimator.ShowNext();   
	}
	
	//////////////

 
	/**
	 * Slide animations to enter a view from left.
	 * 
	 * @param duration the animation duration in milliseconds
	 * @param interpolator the interpolator to use (pass {@code null} to use the {@link AccelerateInterpolator} interpolator) 	
	 * @return a slide transition animation
	 */
			public static Animation inFromLeftAnimation(long duration, IInterpolator interpolator) {



			Animation inFromLeft = new TranslateAnimation(

				  -1.0f,   0.0f,
				 0.0f,  0.0f
		);
			inFromLeft.Duration = duration;
			inFromLeft.Interpolator = interpolator==null? (IInterpolator) new AccelerateInterpolator():interpolator; //AccelerateInterpolato
		return inFromLeft;
	}
 
	/**
	 * Slide animations to hide a view by sliding it to the right
	 * 
	 * @param duration the animation duration in milliseconds
	 * @param interpolator the interpolator to use (pass {@code null} to use the {@link AccelerateInterpolator} interpolator) 	
	 * @return a slide transition animation
	 */
		public static Animation outToRightAnimation(long duration, IInterpolator interpolator) {

		Animation outtoRight = new TranslateAnimation(
				  0.0f,   +1.0f,
				  0.0f,    0.0f
		);
			outtoRight.Duration = duration;
			outtoRight.Interpolator = interpolator == null ? new AccelerateInterpolator () : interpolator; 
		return outtoRight;
	}
 
	/**
	 * Slide animations to enter a view from right.
	 * 
	 * @param duration the animation duration in milliseconds
	 * @param interpolator the interpolator to use (pass {@code null} to use the {@link AccelerateInterpolator} interpolator) 	
	 * @return a slide transition animation
	 */
		public static Animation inFromRightAnimation(long duration, IInterpolator interpolator) {

		Animation inFromRight = new TranslateAnimation(
				  +1.0f,   0.0f,
				  0.0f,    0.0f
		);
			inFromRight.Duration = duration; 
			inFromRight.Interpolator = interpolator == null ? new AccelerateInterpolator () : interpolator; 
		return inFromRight;
	}
 
	/**
	 * Slide animations to hide a view by sliding it to the left.
	 * 
	 * @param duration the animation duration in milliseconds
	 * @param interpolator the interpolator to use (pass {@code null} to use the {@link AccelerateInterpolator} interpolator) 	
	 * @return a slide transition animation
	 */
		public static Animation outToLeftAnimation(long duration, IInterpolator interpolator) {
		Animation outtoLeft = new TranslateAnimation(
				 0.0f,   -1.0f,
				  0.0f,   0.0f
		);
			outtoLeft.Duration = duration;
			outtoLeft.Interpolator = interpolator == null ? new AccelerateInterpolator () : interpolator; 
		return outtoLeft;
	} 
 
	/**
	 * Slide animations to enter a view from top.
	 * 
	 * @param duration the animation duration in milliseconds
	 * @param interpolator the interpolator to use (pass {@code null} to use the {@link AccelerateInterpolator} interpolator) 	
	 * @return a slide transition animation
	 */
		public static Animation inFromTopAnimation(long duration, IInterpolator interpolator) {
		Animation infromtop = new TranslateAnimation(
				0.0f,  0.0f,
				 -1.0f,  0.0f
		);
			infromtop.Duration = duration;
			infromtop.Interpolator = interpolator==null?new AccelerateInterpolator():interpolator;
		return infromtop;
	} 
 
	/**
	 * Slide animations to hide a view by sliding it to the top
	 * 
	 * @param duration the animation duration in milliseconds
	 * @param interpolator the interpolator to use (pass {@code null} to use the {@link AccelerateInterpolator} interpolator) 	
	 * @return a slide transition animation
	 */
		public static Animation outToTopAnimation(long duration, IInterpolator interpolator) {
		Animation outtotop = new TranslateAnimation(
				  0.0f,   0.0f,
				  0.0f,  -1.0f
		);
			outtotop.Duration = duration; 
			outtotop.Interpolator = interpolator==null?new AccelerateInterpolator():interpolator; 
		return outtotop;
	} 

	/**
	 * A fade animation that will fade the subject in by changing alpha from 0 to 1.
	 * 
	 * @param duration the animation duration in milliseconds
	 * @param delay how long to wait before starting the animation, in milliseconds
	 * @return a fade animation
	 * @see #fadeInAnimation(View, long)
	 */
	public static Animation fadeInAnimation(long duration, long delay) {  
		
		Animation fadeIn = new AlphaAnimation(0, 1);
			fadeIn.Interpolator = (new DecelerateInterpolator());  
			fadeIn.Duration = duration;
			fadeIn.StartOffset = delay;
		
		return fadeIn;
	}

	/**
	 * A fade animation that will fade the subject out by changing alpha from 1 to 0.
	 * 
	 * @param duration the animation duration in milliseconds
	 * @param delay how long to wait before starting the animation, in milliseconds
	 * @return a fade animation
	 * @see #fadeOutAnimation(View, long)
	 */
	public static Animation fadeOutAnimation(long duration, long delay) {   

		Animation fadeOut = new AlphaAnimation(1, 0);
			fadeOut.Interpolator = (new AccelerateInterpolator());
			fadeOut.StartOffset = delay;
			fadeOut.Duration = duration;

		return fadeOut;
	} 

	/**
	 * A fade animation that will ensure the View starts and ends with the correct visibility
	 * @param view the View to be faded in
	 * @param duration the animation duration in milliseconds
	 * @return a fade animation that will set the visibility of the view at the start and end of animation
	 */
		public class FadeInAnimationListener : Animation {


			public  void OnAnimationEnd(Animation animation) {

				view.setVisibility(View.VISIBLE);
			} 


			public  void OnAnimationRepeat(Animation animation) { 
			}  


			public  void OnAnimationStart(Animation animation) {
				view.setVisibility(View.GONE); 
			} 
		}


	public static Animation fadeInAnimation(long duration,  View view) { 
		Animation animation = fadeInAnimation(500, 0); 

			animation.setAnimationListener(new FadeInAnimationListener());
	    
	    return animation;
	}

		public class FadeOutAnimationListener : Animation.IAnimationListener 
		{
			public  void OnAnimationEnd(Animation animation) {

				view.setVisibility(View.GONE);
			} 

			public  void OnAnimationRepeat(Animation animation) { 
			}  

			public  void OnAnimationStart(Animation animation) {
				view.setVisibility(View.VISIBLE); 
			} 
		}

	/**
	 * A fade animation that will ensure the View starts and ends with the correct visibility
	 * @param view the View to be faded out
	 * @param duration the animation duration in milliseconds
	 * @return a fade animation that will set the visibility of the view at the start and end of animation
	 */
	public static Animation fadeOutAnimation(long duration,  View view) {
		
		Animation animation = fadeOutAnimation(500, 0); 

			animation.setAnimationListener(new FadeOutAnimationListener());
	    
	    return animation;
		
	}

	/**
	 * Creates a pair of animation that will fade in, delay, then fade out
	 * @param duration the animation duration in milliseconds
	 * @param delay how long to wait after fading in the subject and before starting the fade out
	 * @return a fade in then out animations
	 */
	public static Animation[] fadeInThenOutAnimation(long duration, long delay) {  
		return new Animation[] {fadeInAnimation(duration,0), fadeOutAnimation(duration, duration+delay)};
	}  
	
	/**
	 * Fades the view in. Animation starts right away.
	 * @param v the view to be faded in
	 */
	public static void fadeOut(View v) { 
		if (v==null) return;  
	    v.startAnimation(fadeOutAnimation(500, v)); 
	} 
	
	/**
	 * Fades the view out. Animation starts right away.
	 * @param v the view to be faded out
	 */
	public static void fadeIn(View v) { 
		if (v==null) return;
		
	    v.startAnimation(fadeInAnimation(500, v)); 
	}
	
		public class FadeInOutAnimationListener : Animation.IAnimationListener
		{
			public  void OnAnimationEnd(Animation animation) {
				v.setVisibility(View.GONE);
			} 
			public  void OnAnimationRepeat(Animation animation) { 
			}  
			public  void OnAnimationStart(Animation animation) {
				v.setVisibility(View.VISIBLE); 
			} 
		}

	/**
	 * Fades the view in, delays the specified amount of time, then fades the view out
	 * @param v the view to be faded in then out
	 * @param delay how long the view will be visible for
	 */
	public static void fadeInThenOut( View v, long delay) {
		if (v==null) return;
		 
		v.setVisibility(View.VISIBLE);
		AnimationSet animation = new AnimationSet(true);
		Animation[] fadeInOut = fadeInThenOutAnimation(500,delay); 
	    animation.addAnimation(fadeInOut[0]);
	    animation.addAnimation(fadeInOut[1]);
			animation.setAnimationListener(new FadeInOutAnimationListener());
	    
	    v.startAnimation(animation); 
	}

}

}
#endif

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



	public  static class _ScaleUpDownEnum {
		/**
		 * The intermittent zoom level given the current or desired maximum zoom level for the specified iteration
		 * 
		 * @param max the maximum desired or current zoom level
		 * @param iter the iteration (from 0..1).
		 * @return the current zoom level
		 */
		public static float getScale(this FlipAnimation.ScaleUpDownEnum _this, float max, float iter) {
			switch(_this) {
			case ScaleUpDownEnum.SCALE_UP:
				return max +  (1-max)*iter;

			case ScaleUpDownEnum.SCALE_DOWN:
				return 1 - (1-max)*iter;

			case ScaleUpDownEnum.SCALE_CYCLE: { 
					boolean halfWay = (iter > 0.5);  

					if (halfWay) {
						return max +  (1-max)*(iter-0.5f)*2;
					} else {
						return 1 - (1-max)*(iter*2);
					}
				}

			default:
				return 1;
			}
		}
	}

	public class FlipAnimation : Animation { 

		public   enum ScaleUpDownEnum {
			SCALE_UP, SCALE_DOWN, SCALE_CYCLE, SCALE_NONE

		}
	private  float mFromDegrees;
	private  float mToDegrees;
	private  float mCenterX;
	private  float mCenterY;
	private Camera mCamera;
	
	private  ScaleUpDownEnum scaleType;
	 
	/**
	 * How much to scale up/down. The default scale of 75% of full size seems optimal based on testing. Feel free to experiment away, however.
	 */ 
	public static  float SCALE_DEFAULT = 0.75f;
	
	private float scale;

	/**
	 * Constructs a new {@code FlipAnimation} object.Two {@code FlipAnimation} objects are needed for a complete transition b/n two views. 
	 * 
	 * @param fromDegrees the start angle in degrees for a rotation along the y-axis, i.e. in-and-out of the screen, i.e. 3D flip. This should really be multiple of 90 degrees.
	 * @param toDegrees the end angle in degrees for a rotation along the y-axis, i.e. in-and-out of the screen, i.e. 3D flip. This should really be multiple of 90 degrees.
	 * @param centerX the x-axis value of the center of rotation
	 * @param centerY the y-axis value of the center of rotation
	 * @param scale to get a 3D effect, the transition views need to be zoomed (scaled). This value must be b/n (0,1) or else the default scale {@link #SCALE_DEFAULT} is used.
	 * @param scaleType flip view transition is broken down into two: the zoom-out of the "from" view and the zoom-in of the "to" view. This parameter is used to determine which is being done. See {@link ScaleUpDownEnum}.
	 */
	public FlipAnimation(float fromDegrees, float toDegrees, float centerX, float centerY, float scale, ScaleUpDownEnum scaleType) {
		mFromDegrees = fromDegrees;
		mToDegrees = toDegrees;
		mCenterX = centerX;
		mCenterY = centerY;
		this.scale = (scale<=0||scale>=1)?SCALE_DEFAULT:scale;
		this.scaleType = scaleType==null?ScaleUpDownEnum.SCALE_CYCLE:scaleType;
	}

	public void initialize(int width, int height, int parentWidth, int parentHeight) {
		super.initialize(width, height, parentWidth, parentHeight);
		mCamera = new Camera();
	}

	protected void applyTransformation(float interpolatedTime, Transformation t) {
		 float fromDegrees = mFromDegrees;
		float degrees = fromDegrees + ((mToDegrees - fromDegrees) * interpolatedTime);

		 float centerX = mCenterX;
		 float centerY = mCenterY;
		 Camera camera = mCamera;

		 Matrix matrix = t.getMatrix();

		camera.save();

		camera.rotateY(degrees); 

		camera.getMatrix(matrix);
		camera.restore();

		matrix.preTranslate(-centerX, -centerY);
		matrix.postTranslate(centerX, centerY); 
		
		matrix.preScale(scaleType.getScale(scale, interpolatedTime), scaleType.getScale(scale, interpolatedTime), centerX, centerY);

	}

	
	/**
	 * This enumeration is used to determine the zoom (or scale) behavior of a {@link FlipAnimation}.
	 * 
	 * @author Ephraim A. Tekle 
	 *
	 */
	
	
	
}
}
#endif
namespace UIKit 
{
	public class UIPopoverController 
	{

	}

	public class UIPopoverControllerDelegate
	{
		public virtual bool ShouldDismiss (UIPopoverController popoverController)
		{
			return false; 
		}

		public virtual void DidDismiss (UIPopoverController popoverController)
		{

		}
	}
}
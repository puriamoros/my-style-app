using MyStyleApp.Enums;
using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public class AccountDetailsView : AccountViewBase
    {
        protected override bool OnBackButtonPressed()
        {
            if(this.GetEditButton().IsVisible)
            {
                return base.OnBackButtonPressed();
            }
            else
            {
                // We are on edit mode and back button has been tapped, so we want to go back to view mode

                // Send message to change mode
                MessagingCenter.Send<string>(AccountModeEnum.View.ToString(), "changeAccountMode");

                // Ignore hardware back button tap
                return true;
            }
        }
    }
}

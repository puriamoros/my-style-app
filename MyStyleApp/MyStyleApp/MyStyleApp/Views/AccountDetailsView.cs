using MyStyleApp.Enums;
using System;
using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public class AccountDetailsView : AccountViewBase
    {
        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send<string>(AccountModeEnum.View.ToString(), "changeAccountMode");

            // Disable hardware back button
            return true;
        }
    }
}

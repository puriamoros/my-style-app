using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class BookView : CustomContentPage
    {
        public BookView()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, "slotsRefreshed", (ignored) =>
            {
                this.SlotList.IsRefreshing = false;
            });
        }
    }
}

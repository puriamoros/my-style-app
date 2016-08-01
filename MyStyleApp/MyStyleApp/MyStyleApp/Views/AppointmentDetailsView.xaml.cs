using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class AppointmentDetailsView : CustomContentPage
    {
        public AppointmentDetailsView()
        {
            InitializeComponent();
            this.NotesLayout.SizeChanged += OnNotesLayoutSizeChanged;
        }

        private void OnNotesLayoutSizeChanged(object sender, EventArgs e)
        {
            if(NotesLayout.HeightRequest == -1)
            {
                NotesLayout.HeightRequest = NotesLayout.Height;
            }
        }
    }
}

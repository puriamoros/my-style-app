﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class ClientAppointmentsView : CustomContentPage
    {
        public ClientAppointmentsView()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, "clientAppointmentsRefreshed", (ignored) =>
            {
                this.AppointmentList.IsRefreshing = false;
            });
        }
    }
}

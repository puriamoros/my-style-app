﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public partial class OwnerEstablishmentsView : CustomContentPage
    {
        public OwnerEstablishmentsView()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, "ownerEstablishmentsRefreshed", (ignored) =>
            {
                this.EstablishmentsList.IsRefreshing = false;
            });
        }
    }
}

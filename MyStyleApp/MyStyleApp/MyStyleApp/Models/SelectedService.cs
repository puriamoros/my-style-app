using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Models
{
    public class SelectedService : Service
    {
        public string PriceStr { get; set; }

        public bool Selected { get; set; }
    }
}

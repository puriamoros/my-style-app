using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Models
{
    public class ServiceNameAndPrice
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string NameAndPrice
        {
            get { return Name + " - " + Price.ToString("0.00") + "€"; }   
        }
        
        public int IdServiceCategory { get; set; }
        
        public float Price { get; set; }
    }
}

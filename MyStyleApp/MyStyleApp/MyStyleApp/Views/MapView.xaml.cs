using MyStyleApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MyStyleApp.Views
{
    public partial class MapView : CustomContentPage
    {
        public MapView()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<Establishment>(this, "showEstablishmentOnMap", (establishment) =>
            {
                Position position = new Position(establishment.Latitude, establishment.Longitude);
                this.MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.3)));

                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = establishment.Name,
                    Address = establishment.Address
                };
                this.MyMap.Pins.Add(pin);
            });
        }
    }
}

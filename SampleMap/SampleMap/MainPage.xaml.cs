using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SampleMap
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Task.Factory.StartNew(CheckLocationPermission);
        }

        private async void CheckLocationPermission()
        {

            PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        //await DisplayAlert("Need location Permission", "Location permission not available", "OK");
                    }
                });
                

                Device.BeginInvokeOnMainThread(async() =>
                {
                    var result = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = result.FirstOrDefault().Value;
                });
            }

            if (status == PermissionStatus.Granted)
            {
                // turn on location
                DependencyService.Get<ILocation>().turnOnGps();
            }
            else if (status != PermissionStatus.Unknown)
            {
                //location denied
            }
        }
        private async void MoveToCurrentLocation()
        {
            CurrentPosition position = await GetPosition();

            Position mapPosition = new Position(position.Latitude,position.Longitude);

            Device.BeginInvokeOnMainThread(() => {

                formMap.MoveToRegion(MapSpan.FromCenterAndRadius(mapPosition, Distance.FromMiles(3)));

                var mapPin = new Pin
                {
                    Type = PinType.Place,
                    Position = mapPosition,
                    Label = "Subrata",
                    Address = "South Dumdum"
                };

                formMap.Pins.Add(mapPin);

            });
        }
        private async Task<CurrentPosition> GetPosition()
        {
            CurrentPosition p = new CurrentPosition();
            if (CrossGeolocator.Current.IsGeolocationAvailable)
            {
                if (CrossGeolocator.Current.IsGeolocationEnabled)
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                    p.Latitude = position.Latitude;
                    p.Longitude = position.Longitude;

                }
                else
                {
                    await DisplayAlert("Message", "GPS Not Enabled", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Message", "GPS Not Available", "Ok");
            }
            return p;
        }
        private void MoveToCurrentlocation(object sender, EventArgs e)
        {
            MoveToCurrentLocation();
        }
    }

    public class CurrentPosition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

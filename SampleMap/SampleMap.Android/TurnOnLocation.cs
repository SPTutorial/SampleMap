using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SampleMap.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(TurnOnLocation))]
namespace SampleMap.Droid
{
    public class TurnOnLocation : ILocation
    {
        public TurnOnLocation()
        {

        }
        public async void turnOnGps()
        {
            try
            {
                MainActivity activity = global::Xamarin.Forms.Forms.Context as MainActivity;

                GoogleApiClient googleApiClient = new GoogleApiClient.Builder(activity).AddApi(LocationServices.API).Build();
                googleApiClient.Connect();
                LocationRequest locationRequest = LocationRequest.Create();
                locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
                locationRequest.SetInterval(10000);
                locationRequest.SetFastestInterval(10000 / 2);

                LocationSettingsRequest.Builder
                    locationSettingsRequestBuilder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
                locationSettingsRequestBuilder.SetAlwaysShow(false);
                LocationSettingsResult locationSettingsResult = await LocationServices.SettingsApi.CheckLocationSettingsAsync(
                  googleApiClient, locationSettingsRequestBuilder.Build());

                
                if (locationSettingsResult.Status.StatusCode == LocationSettingsStatusCodes.ResolutionRequired)
                {
                    locationSettingsResult.Status.StartResolutionForResult(activity, 0);
                }

                var result = await LocationServices.SettingsApi.CheckLocationSettingsAsync(googleApiClient, locationSettingsRequestBuilder.Build());

            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
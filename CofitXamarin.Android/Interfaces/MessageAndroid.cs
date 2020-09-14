using Android.Widget;
using CofitXamarin.Droid.Interfaces;
using CofitXamarin.Interfaces;
using Xamarin.Forms;


[assembly: Dependency(typeof(MessageAndroid))]
namespace CofitXamarin.Droid.Interfaces
{
    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}
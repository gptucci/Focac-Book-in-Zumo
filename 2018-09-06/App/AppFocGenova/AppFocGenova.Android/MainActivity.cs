using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using AppFocGenova.Interface;

namespace AppFocGenova.Droid
{
    [Activity(Label = "AppFocGenova", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ((DroidLoginProvider)DependencyService.Get<ILoginProvider>()).Init(this);


            LoadApplication(new App());
        }

        // Define a authenticated user.
        //private MobileServiceUser user;

        //public async Task<bool> Authenticate()
        //{
        //    var success = false;
        //    var message = string.Empty;
        //    try
        //    {

        //        //App.AzureClient.

        //        //MainPageViewModel.
        //        //// Sign in with Facebook login using a server-managed flow.
        //        //user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(this,
        //        //    MobileServiceAuthenticationProvider.Google, "{url_scheme_of_your_app}");
        //        //if (user != null)
        //        //{
        //        //    message = string.Format("you are now signed-in as {0}.",
        //        //        user.UserId);
        //        //    success = true;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        message = ex.Message;
        //    }

        //    // Display the success or failure message.
        //    AlertDialog.Builder builder = new AlertDialog.Builder(this);
        //    builder.SetMessage(message);
        //    builder.SetTitle("Sign-in result");
        //    builder.Create().Show();

        //    return success;
        //}


    }



}


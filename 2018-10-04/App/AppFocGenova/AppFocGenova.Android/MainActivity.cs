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
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using AppFocGenova;
using AppFocGenova.Droid;
using Android.Content;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace AppFocGenova.Droid
{
    [Activity(Label = "AppFocGenova", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        ICallbackManager callbackManager;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            //((DroidLoginProvider)DependencyService.Get<ILoginProvider>()).Init(this);

            FacebookSdk.SdkInitialize(ApplicationContext);
            callbackManager = CallbackManagerFactory.Create();


            //var loginCallback = new FacebookCallback<LoginResult>
            //{
            //    HandleSuccess = loginResult => {
            //        var tck =AccessToken.CurrentAccessToken.Token;
            //        //CoffeeCups.Helpers.Settings.FacebookAccessToken = AccessToken.CurrentAccessToken.Token;
            //        App.GoToMainPage();
            //    },
            //    HandleCancel = () => {

            //    },
            //    HandleError = loginError => {

            //    }
            //};

            

            LoginManager.Instance.RegisterCallback(callbackManager, new facebookCallBack());
            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            //      Facebook
            callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }


        private class facebookCallBack : Java.Lang.Object, IFacebookCallback, IDisposable
        {
            #region IFacebookCallback implementation
            public void OnCancel()
            {

            }
            public void OnError(FacebookException p0)
            {

            }
            public void OnSuccess(Java.Lang.Object p0)
            {
                LoginResult loginResult = (LoginResult)p0;
                Settings.AuthToken = loginResult.AccessToken.Token;
                App.GoToMainPage();
            }
            #endregion
        }

        //class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
        //{
        //    public Action HandleCancel { get; set; }
        //    public Action<FacebookException> HandleError { get; set; }
        //    public Action<TResult> HandleSuccess { get; set; }

        //    public void OnCancel()
        //    {
        //        HandleCancel?.Invoke();
        //    }

        //    public void OnError(FacebookException error)
        //    {
        //        HandleError?.Invoke(error);
        //    }

        //    public void OnSuccess(Java.Lang.Object result)
        //    {
        //        HandleSuccess?.Invoke(result.JavaCast<TResult>());
        //    }
        //}



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


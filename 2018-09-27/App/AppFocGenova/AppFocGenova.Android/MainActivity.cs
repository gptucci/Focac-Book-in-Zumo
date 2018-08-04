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
using Java.Security;

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

            ////Per ottenere la hash key da inserire in facebook
            //PackageInfo info = this.PackageManager.GetPackageInfo("it.trilogik.AppFocGenova", PackageInfoFlags.Signatures);
            //foreach (Android.Content.PM.Signature signature in info.Signatures)
            //{
            //    MessageDigest md = MessageDigest.GetInstance("SHA");
            //    md.Update(signature.ToByteArray());

            //    string keyhash = Convert.ToBase64String(md.Digest());
            //    Console.WriteLine("KeyHash:", keyhash);
            //}


            FacebookSdk.SdkInitialize(ApplicationContext);
            callbackManager = CallbackManagerFactory.Create();



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

        

    }



}


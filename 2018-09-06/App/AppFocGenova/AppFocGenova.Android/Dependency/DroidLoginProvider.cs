using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppFocGenova;
using Microsoft.WindowsAzure.MobileServices;
using AppFocGenova.Interface;
using Android.Webkit;

[assembly: Xamarin.Forms.Dependency(typeof(DroidLoginProvider))]
namespace AppFocGenova
{

    
    public class DroidLoginProvider : ILoginProvider
    {
        Context context;

        public void Init(Context context)
        {
            this.context = context;
        }

        //private async Task Logout(MobileServiceClient client)
        //{
        //    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
        //        Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);
        //    else
        //        Android.Webkit.CookieManager.Instance.RemoveAllCookie();

        //    await client.LogoutAsync();
        //}


        public async Task LoginAsync(MobileServiceClient client)
        {

            //await Logout(client);
            
            //await client.LoginAsync(MobileServiceAuthenticationProvider.Facebook, token);
            var res = await client.LoginAsync(context, MobileServiceAuthenticationProvider.Facebook, "apnm");
            
        }
    }
}
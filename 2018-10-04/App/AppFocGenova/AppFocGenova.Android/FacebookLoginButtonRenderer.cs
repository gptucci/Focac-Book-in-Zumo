using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Forms;

namespace AppFocGenova.Droid
{
    public class FacebookLoginButtonRenderer : ViewRenderer<FacebookLoginButton, LoginButton>
    {
        LoginButton facebookLoginButton;
        protected override void OnElementChanged(ElementChangedEventArgs<FacebookLoginButton> e)
        {
            base.OnElementChanged(e);
            if (Control == null || facebookLoginButton == null)
            {
                facebookLoginButton = new LoginButton(Forms.Context);
                SetNativeControl(facebookLoginButton);
            }
        }

    }
}
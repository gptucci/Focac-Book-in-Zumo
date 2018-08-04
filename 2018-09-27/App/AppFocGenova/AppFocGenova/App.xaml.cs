using AppFocGenova.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AppFocGenova
{
	public partial class App : Application
	{

        public static AzureMobileClient AzureClient { get; set; }
        public App ()
		{
			InitializeComponent();
            AzureClient = new AzureMobileClient();

            //MainPage =new NavigationPage(new MainPage());

            MainPage = new LoginPage();
            //MainPage = new AppFocGenova.MainPage();
        }


        public static void GoToMainPage()
        {
            //Current.MainPage = new AppFocGenova.MainPage();
            Current.MainPage = new NavigationPage(new AppFocGenova.MainPage());

        }

        protected async override void OnStart ()
		{
            // Handle when your app starts
            await AzureClient.Initialize();
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

using AppFocGenova.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFocGenova
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
	{
        private MainPageViewModel vm;
        private bool LogikìnOK = false;
        public MainPage()
		{
			InitializeComponent();
            BindingContext = vm =new MainPageViewModel();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (!LogikìnOK)
            {
                LogikìnOK = true;

            }
            vm.LoadFocaccePostCommand.Execute(null);
            
        }
    }
}

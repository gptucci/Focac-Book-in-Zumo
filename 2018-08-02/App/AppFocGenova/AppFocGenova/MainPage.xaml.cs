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
        public MainPage()
		{
			InitializeComponent();
            BindingContext = vm =new MainPageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.LoadFocaccePostCommand.Execute(null);
            
        }
    }
}

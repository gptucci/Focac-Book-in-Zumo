using AppFocGenova.Models;
using AppFocGenova.Service;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppFocGenova.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        AzureMobileClient azureclient;
        ICommand loadfocaccepostCommand;
        ICommand aggiungipostfocacciaCommand;


        public MainPageViewModel()
        {
            azureclient = App.AzureClient;
            this.Title = "Lista Post";
        }


        public ObservableRangeCollection<FocaccePost> CollectionFocaccePost { get; } = new ObservableRangeCollection<FocaccePost>();


        
        public ICommand LoadFocaccePostCommand =>
            loadfocaccepostCommand ?? (loadfocaccepostCommand = new Command(async () => await LoadFocaccePostAsync()));

        public ICommand AggiungiPostFocacciaPostCommand =>
            aggiungipostfocacciaCommand ?? (aggiungipostfocacciaCommand = new Command(async () => await AggiungiFocacciPostAsync()));



        FocaccePost focaccePostselectedItem;
        public FocaccePost SelectedItem
        {
            get { return focaccePostselectedItem; }
            set
            {
                SetProperty(ref focaccePostselectedItem, value);
                if (focaccePostselectedItem != null)
                {
                    Application.Current.MainPage.Navigation.PushAsync(new FocacciaPostEditPage(focaccePostselectedItem));
                    SelectedItem = null;
                }
            }
        }


        private async Task LoadFocaccePostAsync()
        {
            if (IsBusy)
                return;

            try
            {
                
                IsBusy = true;
                var focaccepost = await azureclient.ReadAllItemsAsync();
                Settings.UserId = await azureclient.GetIdentityAsync();

                CollectionFocaccePost.ReplaceRange(focaccepost);
      
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Errore nel Sync - verificare connettività", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AggiungiFocacciPostAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new FocacciaPostEditPage(null));
        }

    }
}

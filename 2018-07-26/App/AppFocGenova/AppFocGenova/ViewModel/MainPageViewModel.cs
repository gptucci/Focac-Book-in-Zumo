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
        Command loadfocaccepostCommand;
        Command aggiungipostfocacciaCommand;
        Command pushfocaccePostCommand;
        Command pullfocaccepostCommand;
  


        public MainPageViewModel()
        {
            azureclient = App.AzureClient;
            this.Title = "Lista Post";
        }


        public ObservableRangeCollection<FocaccePost> CollectionFocaccePost { get; } = new ObservableRangeCollection<FocaccePost>();


        
        public ICommand LoadFocaccePostCommand =>
            loadfocaccepostCommand ?? (loadfocaccepostCommand = new Command(async () => await LoadFocaccePostAsync(), () => !IsBusy));

        public ICommand AggiungiPostFocacciaPostCommand =>
            aggiungipostfocacciaCommand ?? (aggiungipostfocacciaCommand = new Command(async () => await AggiungiFocaccePostAsync(), () => !IsBusy));

        public ICommand PushFocacciaPostCommand =>
            pushfocaccePostCommand ?? (pushfocaccePostCommand = new Command(async () => await PushFocacciPostAsync(), () => !IsBusy));

        public ICommand SyncFocacciaPostCommand =>
            pullfocaccepostCommand ?? (pullfocaccepostCommand = new Command(async () => await SyncFocaccePost(), () => !IsBusy));



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

        private async Task AggiungiFocaccePostAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new FocacciaPostEditPage(null));
        }

        private async Task PushFocacciPostAsync()
        {
            IsBusy = true;
            ChangeCanExecute();

            var azureService = App.AzureClient;
            await azureclient.PushModifiche();

            IsBusy = false;
            ChangeCanExecute();
        }

        private async Task SyncFocaccePost()  {

            IsBusy = true;
            ChangeCanExecute();

            var azureService = App.AzureClient;
            await azureclient.SyncFocacceDB();
            var focaccepost = await azureclient.ReadAllItemsAsync();
            CollectionFocaccePost.ReplaceRange(focaccepost);
            IsBusy = false;
            ChangeCanExecute();
        }

        private void ChangeCanExecute()
        {

            loadfocaccepostCommand.ChangeCanExecute();
            aggiungipostfocacciaCommand.ChangeCanExecute();
            pushfocaccePostCommand.ChangeCanExecute();
            pullfocaccepostCommand.ChangeCanExecute();


        }


    }
}

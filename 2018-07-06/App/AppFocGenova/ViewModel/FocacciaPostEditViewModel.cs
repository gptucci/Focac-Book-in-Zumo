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
    public class FocacciaPostEditViewModel : BaseViewModel
    {
        AzureMobileClient azureService;
        ICommand salvapostfocacciaCommand;
        ICommand cancellapostfocacciaCommand;
        ICommand annullaCommand;
        private FocaccePost focaccePostInEditing = null;
        


        public FocacciaPostEditViewModel(FocaccePost focaccePost)
        {
            azureService = App.AzureClient;
            if (focaccePost!=null)
            {
                //Siamo in modifica
                this.Title = "Modifica Post";
                NomeUtente = focaccePost.NomeUtente;
                Luogo = focaccePost.Luogo;
                DataPost = focaccePost.DataOra.Date;
                OraPost = focaccePost.DataOra.TimeOfDay;
                Voto = focaccePost.Voto;
                VisualizzaCancella= true;
                focaccePostInEditing = focaccePost;
            }
            else
            {
                //siamo in nuovo
                this.Title = "Inserimento Post";

            }
        }


        public ICommand SalvaFocacciaPostCommand =>
            salvapostfocacciaCommand ?? (salvapostfocacciaCommand = new Command(async () => await SalvaFocacciPostAsync()));
        public ICommand CancellaFocacciaPostCommand =>
            cancellapostfocacciaCommand ?? (cancellapostfocacciaCommand = new Command(async () => await CancellaFocacciPostAsync()));

        public ICommand AnnullaCommand =>
            annullaCommand ?? (annullaCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync()));

        private bool visualizzaCancella = false;
        public bool VisualizzaCancella
        {
            get => visualizzaCancella;
            set => SetProperty(ref visualizzaCancella, value);
        }


        private string nomeutente = string.Empty;
        public string NomeUtente
        {
            get => nomeutente;
            set => SetProperty(ref nomeutente, value);
        }

        private string luogo = string.Empty;
        public string Luogo
        {
            get => luogo;
            set => SetProperty(ref luogo, value);
        }

        private int voto =0;
        public int Voto
        {
            get => voto;
            set
            {
                if (value<0 || value>10)
                {
                    SetProperty(ref voto, 0);
                    return;
                }
                SetProperty(ref voto, value);
            }
            
        }

        private DateTime datapost = DateTime.Now;
        public DateTime DataPost
        {
            get => datapost;
            set => SetProperty(ref datapost, value);
        }

        private TimeSpan orapost = DateTime.Now.TimeOfDay;
        public TimeSpan OraPost
        {
            get => orapost;
            set => SetProperty(ref orapost, value);
        }

        private async Task SalvaFocacciPostAsync()
        {
            if (IsBusy)
                return;

            try
            {

                if (string.IsNullOrWhiteSpace(Luogo)||
                    string.IsNullOrWhiteSpace(NomeUtente))
                {
                    await Application.Current.MainPage.DisplayAlert("Aggiunta Focacceria", "Occorre compilare nome utente e luogo", "OK");
                    return;
                }

                IsBusy = true;

                if (focaccePostInEditing==null)
                {
                    //Siamo in nuovo
                    var focacciapost = new FocaccePost
                    {
                        NomeUtente = this.NomeUtente,
                        Luogo = this.Luogo,
                        DataOra = new DateTime(DataPost.Year, DataPost.Month, DataPost.Day, OraPost.Hours, OraPost.Minutes, OraPost.Seconds),
                        Voto = this.Voto
                    };

                    await azureService.AddUpdateItemAsync(focacciapost);
                }else
                {
                    //Siamo in editing
                    focaccePostInEditing.NomeUtente = this.NomeUtente;
                    focaccePostInEditing.Luogo = this.Luogo;
                    focaccePostInEditing.DataOra = new DateTime(DataPost.Year, DataPost.Month, DataPost.Day, OraPost.Hours, OraPost.Minutes, OraPost.Seconds);
                    focaccePostInEditing.Voto = this.Voto;

                    await azureService.AddUpdateItemAsync(focaccePostInEditing);
                }


                
                this.NomeUtente = string.Empty;
                this.Luogo = string.Empty;
                this.Voto = 0;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("====>Errore: " + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Errore nel Sync - verificare connettività", "OK");
            }
            finally
            {
                IsBusy = false;
                await Application.Current.MainPage.Navigation.PopAsync();
            }


        }
        private async Task CancellaFocacciPostAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;


                if (focaccePostInEditing != null)
                {
                    await azureService.DeleteItemAsync(focaccePostInEditing);
                }
                                

                

            }
            catch (Exception ex)
            {
                Debug.WriteLine("====>Errore: " + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Errore nel Sync - verificare connettività", "OK");
            }
            finally
            {
                IsBusy = false;
                await Application.Current.MainPage.Navigation.PopAsync();
            }


        }
    }
}

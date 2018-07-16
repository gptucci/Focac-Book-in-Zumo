using AppFocGenova.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AppFocGenova.Service
{
    public class AzureMobileClient
    {
        MobileServiceClient client;

        public AzureMobileClient()
        {
            client = new MobileServiceClient("<URL backend Aure>");
            
        }

        public async Task Initialize()
        {

            if (client?.SyncContext?.IsInitialized ?? false)
                return;


            string DbPath = Path.Combine(MobileServiceClient.DefaultDatabasePath, "focac-book.db");

            var store = new MobileServiceSQLiteStore(DbPath);

            //definisco la tabella
            store.DefineTable<FocaccePost>();

            //Initialize SyncContext
            //Crea tutte le tabelle di supporto che servono per l'offline sync
            //e che non saranno mai visibili all'utilizzatore
            //ma saranno usate in fase di push e pull
            await client.SyncContext.InitializeAsync(store);
        }

        public async Task<ICollection<FocaccePost>> ReadAllItemsAsync()
        {
            
            try
            {
                IMobileServiceSyncTable<FocaccePost> Focacciatable = client.GetSyncTable<FocaccePost>();
                //await SyncFocacceDB();
                return await Focacciatable.ToListAsync();
            }
            catch (Exception e)
            {

                throw;
            }

        }


        public async Task SyncFocacceDB()
        {
            try
            {

                IMobileServiceSyncTable<FocaccePost> Focacciatable = client.GetSyncTable<FocaccePost>();

                //Caso semplice
                await Focacciatable.PullAsync("allFocacce", Focacciatable.CreateQuery());



                //Caso: il suo uso potrebbe essere relativo a sync complessi
                //sync più veloce per i soli dati dlel'utente
                //sync più lento con i dati degli altri utenti
                //Da osservare che il filtro viene passato come argomento per cui è appklicato lato server
                //var query = Focacciatable.CreateQuery().Where(c => c.NomeUtente == "gpt");
                //await Focacciatable.PullAsync("allFocacceMioGruppo", query);

            }


            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync - Errore: " + ex);
            }

        }

        public async Task PushModifiche()
        {
            try
            {

                await client.SyncContext.PushAsync();


            }

            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync, that is alright as we have offline capabilities: " + ex);
            }

        }

        public async Task AddUpdateItemAsync(FocaccePost focaccePost)
        {

            try
            {
                IMobileServiceSyncTable<FocaccePost> Focacciatable = client.GetSyncTable<FocaccePost>();
                if (string.IsNullOrEmpty(focaccePost.Id))
                {
                    //e' un inserimento
                   
                    await Focacciatable.InsertAsync(focaccePost);
                }
                else
                {
                    //è una modifica
                    await Focacciatable.UpdateAsync(focaccePost);
                }

                //await PushModifiche();
            }

            catch (Exception e)
            {
                throw;
            }
        }
        public async Task DeleteItemAsync(FocaccePost focaccePost)
        {

            try
            {
                IMobileServiceSyncTable<FocaccePost> Focacciatable = client.GetSyncTable<FocaccePost>();
                await Focacciatable.DeleteAsync(focaccePost);



            }
            catch (Exception e)
            {

                throw;
            }

        }

    }


}

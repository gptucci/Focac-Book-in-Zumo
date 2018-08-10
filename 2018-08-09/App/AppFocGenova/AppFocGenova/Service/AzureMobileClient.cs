using AppFocGenova.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppFocGenova.Service
{
    public class AzureMobileClient
    {
        MobileServiceClient client;

        public AzureMobileClient()
        {
            client = new MobileServiceClient("<URL Servizio Backend>");
            
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
            await client.SyncContext.InitializeAsync(store);



        }

        public async Task<ICollection<FocaccePost>> ReadAllItemsAsync()
        {
            
            try
            {
                IMobileServiceSyncTable<FocaccePost> Focacciatable = client.GetSyncTable<FocaccePost>();
                await SyncFocacceDB();
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
                await Focacciatable.PullAsync("allFocacce", Focacciatable.CreateQuery());


            }

            catch (MobileServicePushFailedException conflict)
            {
                if (conflict.PushResult != null)
                {
                    foreach (var error in conflict.PushResult.Errors)
                    {
                        await ResolveConflictAsync(error);
                    }
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync focacce, that is alright as we have offline capabilities: " + ex);
            }

        }

        public async Task PushModifiche()
        {
            try
            {

                await client.SyncContext.PushAsync();

            }


            catch (MobileServicePushFailedException conflict)
            {
                if (conflict.PushResult != null)
                {
                    foreach (var error in conflict.PushResult.Errors)
                    {
                        await ResolveConflictAsync(error);
                    }
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync focacce, that is alright as we have offline capabilities: " + ex);
            }

        }

        private async Task ResolveConflictAsync(MobileServiceTableOperationError error)
        {

            //error.Result è il valore presente nel backend
            if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
            {
                //error.Result -> Il record sul server che ha dato l'errore in formato oggetto Json
                //error.Item -> Il recod locale che ha dato provocato l'errore in formato oggetto Json


                //Vince il server !!!
                //Modifico la tupla locale con quanto ricevuto dal server
                await error.CancelAndUpdateItemAsync(error.Result);


                //Riforzo la tupla locale con i suoi stessi valori e le modifiche verranno riproposte al backend
                //await error.UpdateOperationAsync(error.Item);
            }
            else
            {
                //In alcuni casi (per esempio per proxy interposti o altri casi sfortunati) error.result è null
                await error.CancelAndDiscardItemAsync();

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

                await PushModifiche();
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

using AppFocGenova.Interface;
using AppFocGenova.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppFocGenova.Service
{

   

    public class AzureMobileClient
    {
        MobileServiceClient client;

        public AzureMobileClient()
        {
            client = new MobileServiceClient("https://focac-book.azurewebsites.net");
            
            
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


        //public Task LoginAsync()
        //{

        //    var loginProvider = DependencyService.Get<ILoginProvider>();
        //    return loginProvider.LoginAsync(client);
        //}

        public async Task<ICollection<FocaccePost>> ReadAllItemsAsync()
        {
            try
            {

                var token = new JObject();
                token["access_token"] = Settings.AuthToken;
                var user = await client.LoginAsync(MobileServiceAuthenticationProvider.Facebook, token);

                //user contiene un oggetto di tipo MobileServiceUser che contiene il token rilasciato da azure
                //a fronte del token ricevuto da facebook
                //In app reali il token deve essere salvato per non dover fare tutte le volte il login

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

            //catch (MobileServicePushFailedException conflict)
            //{
            //    if (conflict.PushResult != null)
            //    {
            //        foreach (var error in conflict.PushResult.Errors)
            //        {
            //            await ResolveConflictAsync(error);
            //        }
            //    }
            //}

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


            //catch (MobileServicePushFailedException conflict)
            //{
            //    if (conflict.PushResult != null)
            //    {
            //        foreach (var error in conflict.PushResult.Errors)
            //        {
            //            await ResolveConflictAsync(error);
            //        }
            //    }
            //}

            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync coffees, that is alright as we have offline capabilities: " + ex);
            }

        }

        //private async Task ResolveConflictAsync(MobileServiceTableOperationError error)
        //{


        //    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
        //    {

        //        await error.CancelAndUpdateItemAsync(error.Result);
   
        //    }
        //    else
        //    {
        //        await error.CancelAndDiscardItemAsync();

        //    }
        //}

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

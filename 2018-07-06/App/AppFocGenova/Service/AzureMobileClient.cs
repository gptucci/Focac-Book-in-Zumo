using AppFocGenova.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppFocGenova.Service
{
    public class AzureMobileClient
    {
        MobileServiceClient client;
        

        public AzureMobileClient()
        {
            client = new MobileServiceClient("https://>URL Azure Mobile App Service>");
            
        }

        public async Task<ICollection<FocaccePost>> ReadAllItemsAsync()
        {
            
            try
            {
                //--> On line Sync
                //Rappresenta una tablla temporanea !!
                IMobileServiceTable<FocaccePost> Focacciatable = client.GetTable<FocaccePost>();
                return await Focacciatable.ToListAsync();
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task AddUpdateItemAsync(FocaccePost focaccePost)
        {
            IMobileServiceTable<FocaccePost> Focacciatable = client.GetTable<FocaccePost>();
            try
            {
                
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

                
            }

            //Gestione concorrenza
            catch (MobileServicePreconditionFailedException<FocaccePost> conflict)
            {
                FocaccePost versioneserver = conflict.Item;
                //Qui si può decidere che fare


                //Vince il server
                //Non devo fare nulla


                //Vince il client
                focaccePost.Version = versioneserver.Version;
                await Focacciatable.UpdateAsync(focaccePost);

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

                IMobileServiceTable<FocaccePost> Focacciatable = client.GetTable<FocaccePost>();
                await Focacciatable.DeleteAsync(focaccePost);



            }
            catch (Exception e)
            {

                throw;
            }

        }

    }


}

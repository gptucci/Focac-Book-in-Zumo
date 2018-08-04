using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppFocGenova.Interface
{
    public interface ILoginProvider
    {
        Task LoginAsync(MobileServiceClient client);
    }
}

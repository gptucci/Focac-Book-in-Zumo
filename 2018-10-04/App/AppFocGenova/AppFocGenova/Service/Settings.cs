using System;
using System.Collections.Generic;
using System.Text;

namespace AppFocGenova
{
    public class Settings
    {

        private static string authToken = string.Empty;
        public static string AuthToken
        {
            get
            {
                return authToken;
            }
            set
            {

                authToken = value;
            }
        }


        private static string userId = string.Empty;
        public static string UserId
        {
            get
            {
                return userId;
            }
            set
            {

                userId = value;
            }
        }

    }
}

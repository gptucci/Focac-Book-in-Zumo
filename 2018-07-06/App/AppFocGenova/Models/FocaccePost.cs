using System;
using System.Collections.Generic;
using System.Text;

namespace AppFocGenova.Models
{
    public class FocaccePost
    {
        public string Id { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public byte[] Version { get; set; }

        public string NomeUtente { get; set; }

        public string Luogo { get; set; }

        public DateTime DataOra { get; set; }

        public int Voto { get; set; }




    }
}

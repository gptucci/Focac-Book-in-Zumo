using Microsoft.Azure.Mobile.Server;
using System;

namespace Focac_BookService.DataObjects
{
    public class FocaccePost : EntityData
    {
        public string NomeUtente { get; set; }

        public string Luogo { get; set; }

        public DateTime DataOra { get; set; }

        public int Voto { get; set; }
    }
}
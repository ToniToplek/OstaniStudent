using System;
using System.Collections.Generic;

namespace OstaniStudent.Database.Models
{
    public partial class KorisniciZeljeniModuliDto
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public int IdKorisnik { get; set; }
        public string PrviIzbor { get; set; }
        public int PrviIzborModulId { get; set; }
        public string DrugiIzbor { get; set; }
        public int DrugiIzborModulId { get; set; }
    }
}
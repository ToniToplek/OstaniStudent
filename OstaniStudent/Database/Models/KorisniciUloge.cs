// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace OstaniStudent.Database.Models
{
    public partial class KorisniciUloge
    {
        public int Id { get; set; }
        public int IdKorisnik { get; set; }
        public int IdUloge { get; set; }
        public bool JeAktivan { get; set; }

        public virtual Korisnici IdKorisnikNavigation { get; set; }
        public virtual Uloge IdUlogeNavigation { get; set; }
    }
}
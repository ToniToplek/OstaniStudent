﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace OstaniStudent.Database.Models
{
    public partial class Korisnici
    {
        public Korisnici()
        {
            KorisniciPredmetis = new HashSet<KorisniciPredmeti>();
            KorisniciUloges = new HashSet<KorisniciUloge>();
            KorisnikZeljeniModuls = new HashSet<KorisnikZeljeniModul>();
        }

        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Jmbag { get; set; }
        public Guid? BulkId { get; set; }
        public bool JeAktivan { get; set; }

        public virtual ICollection<KorisniciPredmeti> KorisniciPredmetis { get; set; }
        public virtual ICollection<KorisniciUloge> KorisniciUloges { get; set; }
        public virtual ICollection<KorisnikZeljeniModul> KorisnikZeljeniModuls { get; set; }
    }
}
﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace OstaniStudent.Database.Models
{
    public partial class Predmeti
    {
        public Predmeti()
        {
            KorisniciPredmetis = new HashSet<KorisniciPredmeti>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public int Kapacitet { get; set; }
        public int IdModul { get; set; }
        public int IdSifrarnik { get; set; }
        public bool JeAktivan { get; set; }

        public virtual Moduli IdModulNavigation { get; set; }
        public virtual Sifrarnik IdSifrarnikNavigation { get; set; }
        public virtual ICollection<KorisniciPredmeti> KorisniciPredmetis { get; set; }
    }
}
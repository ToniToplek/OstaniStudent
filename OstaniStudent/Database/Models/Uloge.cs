﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace OstaniStudent.Database.Models
{
    public partial class Uloge
    {
        public Uloge()
        {
            KorisniciUloges = new HashSet<KorisniciUloge>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public bool JeAktivan { get; set; }

        public virtual ICollection<KorisniciUloge> KorisniciUloges { get; set; }
    }
}
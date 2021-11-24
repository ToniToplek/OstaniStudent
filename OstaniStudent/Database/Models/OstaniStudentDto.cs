using System;
using System.Collections.Generic;

namespace OstaniStudent.Database.Models
{
    public partial class OstaniStudentDto
    {
        public int IdKorisnik { get; set; }
        public int IdPredmet { get; set; }
        public int IdModul { get; set; }
        public int Rang { get; set; }
        public int BrojIzbora { get; set; }
    }
}
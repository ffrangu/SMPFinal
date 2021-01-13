using System;
using System.Collections.Generic;

namespace SMP.Data
{
    public partial class Kompania
    {
        public Kompania()
        {
            Departamenti = new HashSet<Departamenti>();
            Pozita = new HashSet<Pozita>();
            Punetori = new HashSet<Punetori>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int KomunaId { get; set; }
        public string Kodi { get; set; }
        public string Emri { get; set; }

        public int Niveli { get; set; }

        public virtual Komuna Komuna { get; set; }
        public virtual ICollection<Departamenti> Departamenti { get; set; }
        public virtual ICollection<Pozita> Pozita { get; set; }
        public virtual ICollection<Punetori> Punetori { get; set; }
    }
}

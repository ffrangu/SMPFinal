using System;
using System.Collections.Generic;

namespace SMP.Data
{
    public partial class Komuna
    {
        public Komuna()
        {
            Kompania = new HashSet<Kompania>();
            Punetori = new HashSet<Punetori>();
        }

        public int Id { get; set; }
        public string Emri { get; set; }

        public virtual ICollection<Kompania> Kompania { get; set; }
        public virtual ICollection<Punetori> Punetori { get; set; }
    }
}

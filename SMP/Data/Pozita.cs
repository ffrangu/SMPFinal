using System;
using System.Collections.Generic;

namespace SMP.Data
{
    public partial class Pozita
    {
        public Pozita()
        {
            Punetori = new HashSet<Punetori>();
        }

        public int Id { get; set; }
        public int KompaniaId { get; set; }
        public int DepartamentiId { get; set; }
        public string Emri { get; set; }
        public bool Status { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }

        public virtual Departamenti Departamenti { get; set; }
        public virtual Kompania Kompania { get; set; }
        public virtual ICollection<Punetori> Punetori { get; set; }
    }
}

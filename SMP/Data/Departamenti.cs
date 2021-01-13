using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMP.Data
{
    public partial class Departamenti
    {
        public Departamenti()
        {
            Pozita = new HashSet<Pozita>();
            Punetori = new HashSet<Punetori>();
        }

        //[Key]
        //public int testid { get; set; }
        [Key]
        public int Id { get; set; }
        public int KompaniaId { get; set; }
        public string Emri { get; set; }
        public string Shkurtesa { get; set; }
        public bool Status { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }

        public virtual Kompania Kompania { get; set; }
        public virtual ICollection<Pozita> Pozita { get; set; }
        public virtual ICollection<Punetori> Punetori { get; set; }
    }
}

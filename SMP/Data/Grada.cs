using System;
using System.Collections.Generic;

namespace SMP.Data
{
    public partial class Grada
    {
        public Grada()
        {
            Paga = new HashSet<Paga>();
            Punetori = new HashSet<Punetori>();
        }

        public int Id { get; set; }
        public string Emri { get; set; }
        public decimal PagaMujore { get; set; }
        public decimal PagaVjetore { get; set; }

        public virtual ICollection<Paga> Paga { get; set; }
        public virtual ICollection<Punetori> Punetori { get; set; }
    }
}

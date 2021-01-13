using System;
using System.Collections.Generic;

namespace SMP.Data
{
    public partial class Bonuset
    {
        public int Id { get; set; }
        public int Muaji { get; set; }
        public int Viti { get; set; }
        public int PunetoriId { get; set; }
        public string Pershkrimi { get; set; }
        public decimal Vlera { get; set; }
        public bool Bruto { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }

        public virtual Punetori Punetori { get; set; }
    }
}

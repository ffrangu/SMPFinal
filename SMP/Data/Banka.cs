using System;
using System.Collections.Generic;

namespace SMP.Data
{
    public partial class Banka
    {
        public Banka()
        {
            Punetori = new HashSet<Punetori>();
        }

        public int Id { get; set; }
        public string Kodi { get; set; }
        public string Emri { get; set; }

        public virtual ICollection<Punetori> Punetori { get; set; }
    }
}

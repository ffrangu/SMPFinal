using SMP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.PunetoriKontrata
{
    public class PunetoriKontrataRepository : GenericRepository<Data.PunetoriKontrata>, IPunetoriKontrataRepository
    {
        public PunetoriKontrataRepository(ApplicationDbContext context) : base(context)
        {

        }

    }
}

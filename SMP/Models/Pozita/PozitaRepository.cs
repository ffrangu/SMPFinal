using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Pozita
{
    public class PozitaRepository : GenericRepository<Data.Pozita>, IPozitaRepository
    {
        public PozitaRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Data.Pozita>> GetPozitat()
        {


            var pozitat = await context.Pozita.Include(x => x.Departamenti)
                .Include(x => x.Kompania)
                .OrderByDescending(x => x.Id).ToListAsync();

            return pozitat;
        }

        public async Task<SelectList> PozitaSelectList(int? PozitaId, bool isList, bool isEdit)
        {
            var pozitat = await GetPozitat();

            return new SelectList(pozitat, "Id", "Emri");
        }


    }
}

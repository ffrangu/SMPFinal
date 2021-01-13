using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Grada
{
    public class GradaRepository :GenericRepository<Data.Grada>, IGradaRepository
    {
        public GradaRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Data.Grada>> GetGradat()
        {


            var gradat = await context.Grada
                .OrderByDescending(x => x.Id).ToListAsync();

            return gradat;
        }

        public async Task<SelectList> GradaSelectList(int? BankaId, bool isList, bool isEdit)
        {
            var bankat = await GetGradat();

            return new SelectList(bankat, "Id", "Emri");
        }

    }
}

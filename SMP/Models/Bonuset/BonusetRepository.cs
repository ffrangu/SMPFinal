using Microsoft.EntityFrameworkCore;
using SMP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Bonuset
{
    public class BonusetRepository :GenericRepository<Data.Bonuset>, IBonusetRepository
    {
        public BonusetRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Data.Bonuset>> getBonuset()
        {
            var bonuset = await context.Bonuset.Include(x => x.Punetori).ToListAsync();

            return bonuset;
        }

        public async Task<IEnumerable<Data.Bonuset>> getBonusetbyKompaniaId(int? kompaniaId, string Role)
        {
            var bonuset = await getBonuset();
            var filteredBonuse = new List<Data.Bonuset>();

            if(Role == "HR")
            {
                filteredBonuse = bonuset.Where(x => x.Punetori.KompaniaId == kompaniaId).ToList();
                return filteredBonuse;
            }
            else
            {
                return bonuset;
            }

        }
    }
}

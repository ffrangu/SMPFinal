using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Bank
{
    public class BankRepository : GenericRepository<Data.Banka> , IBankRepository
    {

        public BankRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Data.Banka>> GetBankat()
        {


            var bankat = await context.Banka
                .OrderByDescending(x => x.Id).ToListAsync();

            return bankat;
        }

        public async Task<SelectList> BankaSelectList(int? BankaId, bool isList, bool isEdit)
        {
            var bankat = await GetBankat();

            return new SelectList(bankat, "Id", "Emri");
        }
    }
}

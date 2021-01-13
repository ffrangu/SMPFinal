using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Bank
{
    public interface IBankRepository : IGenericRepository<Data.Banka>
    {
        Task<IEnumerable<Data.Banka>> GetBankat();

        Task<SelectList> BankaSelectList(int? BankaId, bool isList, bool isEdit);
    }
}

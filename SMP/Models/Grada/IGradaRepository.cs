using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Grada
{
    public interface IGradaRepository : IGenericRepository<Data.Grada>
    {
        Task<IEnumerable<Data.Grada>> GetGradat();

        Task<SelectList> GradaSelectList(int? GradaId, bool isList, bool isEdit);
    }
}

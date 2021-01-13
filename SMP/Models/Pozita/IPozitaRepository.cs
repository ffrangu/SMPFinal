using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Pozita
{
    public interface IPozitaRepository : IGenericRepository<Data.Pozita>
    {

        Task<IEnumerable<Data.Pozita>> GetPozitat();

        Task<SelectList> PozitaSelectList(int? PozitaId, bool isList, bool isEdit);

        

    }
}

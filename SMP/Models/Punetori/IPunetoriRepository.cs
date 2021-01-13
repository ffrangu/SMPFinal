using Microsoft.AspNetCore.Mvc.Rendering;
using SMP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Punetori
{
    public interface IPunetoriRepository : IGenericRepository<Data.Punetori>
    {
        Task<IEnumerable<Data.Punetori>> GetPuntor(int? id);

        Task<SelectList> PunetoretSelectList(int? KompaniaId, string Role);

        Task<Data.Punetori> GetPuntoriDetails(int? id);

        Task<IEnumerable<Data.Punetori>> Search(string value);

        Task<Data.Punetori> GetPunetoriByUserId(string UserId);

        SelectList LoadPunetoret(int? KompaniaId);

    }
}

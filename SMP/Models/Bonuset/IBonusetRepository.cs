using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Bonuset
{
    public interface IBonusetRepository : IGenericRepository<Data.Bonuset> 
    {
        Task<IEnumerable<Data.Bonuset>> getBonuset();
        
        Task<IEnumerable<Data.Bonuset>> getBonusetbyKompaniaId(int? kompaniaId,string Role);
    }
}

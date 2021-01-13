using SMP.Data;
using SMP.ViewModels.Paga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Paga
{
    public interface IPagaRepository : IGenericRepository<Data.Paga>
    {
        Task<IList<Data.Paga>> BulkInsertPaga(IList<Data.Paga> pagat);

        Task<bool> IsPagaInserted(int KompaniaId, int Viti, int Muaji, string role);

        Task KompaniaSubTreeAsync(IEnumerable<Data.Kompania> companies, Data.Kompania company, IList<Data.Punetori> punetoret);

        Task<IList<Data.Punetori>> GetPunetoret(int KompaniaId);

        Task<decimal> Tatimi(decimal paganeto, bool primare);

        List<PagaViewModel> GetPagat(string role, int? KompaniaId);

        Task<List<AllPagat>> GetAllPagat(int m, int v, int k);

        Task<Data.Paga> GetPaga(int Id);

        Task<ApplicationUser> GetPunetoriUser(int PunetoriId);

        Task<decimal> GetBonus(int PunetoriId, int Muaji, int Viti, bool Bruto);
    }
}

using SMP.ViewModels.Paga;
using SMP.ViewModels.Punetori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Raport
{
    public interface IRaportRepository
    {
        Task<List<AllPagat>> GetAllPagat(int? PunetoriId, int? KompaniaId, int? Viti, int? Muaji, int? BankaId, int? GradaId);

        Task<List<PunetoriListViewModel>> GetAllPunetoret(int? PunetoriId, int? KompaniaId, int? BankaId, int? GradaId);

        Task<List<AllPagat>> Payslip(int? PunetoriId, int? KompaniaId, int? Viti, int? Muaji, int? BankaId, int? GradaId);
    }
}

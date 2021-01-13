using SMP.ViewModels.Home;
using SMP.ViewModels.Kompania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Home
{
    public interface IHomeRepository
    {
        Task<int> CountPuntoret(int? KompaniaId);

        Task<int> CountKompanite();

        Task<int> CountKontratat(int? KompaniaId, string UserId);

        Task<int> CountPozitat(int? KompaniaId);

        Task<decimal> TotalBrutoPaga(int? KompaniaId, string UserId);

        Task<decimal> TotalBonuse(int? KompaniaId, string UserId);

        Task<List<Data.Kompania>> ListaKompanive(int? KompaniaId);
        Task<List<Data.Punetori>> ListaPunetorve(int? KompaniaId);
        Task<List<Data.Pozita>> ListaPozitave(int? KompaniaId);

        Task<List<Data.Paga>> ListaPagave(int? KompaniaId, string UserId);
        Task<List<Data.Bonuset>> ListaBonuseve(int? KompaniaId, string UserId);
    }
}

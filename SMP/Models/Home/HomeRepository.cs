using Microsoft.EntityFrameworkCore;
using SMP.Data;
using SMP.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Home
{
    public class HomeRepository : IHomeRepository
    {
        protected readonly ApplicationDbContext context;

        public HomeRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<int> CountKompanite()
        {
            return await context.Kompania.CountAsync();
        }

        public async Task<int> CountKontratat(int? KompaniaId, string UserId)
        {
            if(!string.IsNullOrEmpty(UserId))
            {
                var punetori = await context.Punetori.FirstOrDefaultAsync(q => q.UserId == UserId);

                return await context.PunetoriKontrata.Where(q=>q.PunetoriId == punetori.Id).CountAsync();
            }
            else
            {
                if(KompaniaId.HasValue)
                {
                    return await context.PunetoriKontrata.Include(q => q.Punetori.Kompania)
                                                         .Include(q => q.Punetori).Where(q => q.Punetori.KompaniaId == KompaniaId.Value).CountAsync();
                }
                else
                {
                    return await context.PunetoriKontrata.CountAsync();
                }
            }
        }

        public async Task<int> CountPozitat(int? KompaniaId)
        {
            if(KompaniaId.HasValue)
            {
                return await context.Pozita.Where(q=>q.KompaniaId == KompaniaId).CountAsync();
            }
            else
            {
                return await context.Pozita.CountAsync();
            }
        }

        public async Task<int> CountPuntoret(int? KompaniaId)
        {
            if(KompaniaId.HasValue)
            {
                return await context.Punetori.Where(q=>q.KompaniaId == KompaniaId.Value).CountAsync();
            }
            else
            {
                return await context.Punetori.CountAsync();
            }
        }

        public async Task<List<Data.Kompania>> ListaKompanive(int? KompaniaId)
        {
            var kompanite = await context.Kompania.Where(q=>!q.ParentId.HasValue).ToListAsync();

            return kompanite;
        }

        public async Task<List<Data.Pozita>> ListaPozitave(int? KompaniaId)
        {
            if(KompaniaId.HasValue)
            {
                var pozitat = await context.Pozita.Include(q => q.Kompania)
                                                  .Include(q => q.Departamenti).Where(q => q.KompaniaId == KompaniaId).ToListAsync();
                return pozitat;
            }
            else
            {
                var pozitat = await context.Pozita.Include(q => q.Kompania)
                                                  .Include(q => q.Departamenti).ToListAsync();
                return pozitat;
            }
        }

        public async Task<List<Data.Punetori>> ListaPunetorve(int? KompaniaId)
        {
            if (KompaniaId.HasValue)
            {
                var punetoret = await context.Punetori.Include(q => q.Kompania)
                                                      .Include(q => q.Pozita)
                                                      .Include(q => q.Grada).Where(q => q.KompaniaId == KompaniaId).ToListAsync();
                return punetoret;
            }
            else
            {
                var punetoret = await context.Punetori.Include(q => q.Kompania)
                                                      .Include(q => q.Pozita)
                                                      .Include(q => q.Grada).ToListAsync();
                return punetoret;
            }
        }

        public async Task<decimal> TotalBonuse(int? KompaniaId, string UserId)
        {
            decimal? shuma;
            if(!string.IsNullOrEmpty(UserId))
            {
                var punetori = await context.Punetori.FirstOrDefaultAsync(q => q.UserId == UserId);

                shuma = await context.Paga.Where(q => q.PunetoriId == punetori.Id).SumAsync(q => q.Bonuse);
            }
            else
            {
                if(KompaniaId.HasValue)
                {
                    shuma = await context.Paga.Where(q => q.KompaniaId == KompaniaId).SumAsync(q => q.Bonuse);
                }
                else
                {
                    shuma = await context.Paga.SumAsync(q => q.Bonuse);
                }
            }

            if (!shuma.HasValue)
                return 0;
            else
                return shuma.Value;
        }

        public async Task<decimal> TotalBrutoPaga(int? KompaniaId, string UserId)
        {
            decimal? shuma;
            if (!string.IsNullOrEmpty(UserId))
            {
                var punetori = await context.Punetori.FirstOrDefaultAsync(q => q.UserId == UserId);

                shuma = await context.Paga.Where(q => q.PunetoriId == punetori.Id).SumAsync(q => q.Bruto);
            }
            else
            {
                if (KompaniaId.HasValue)
                {
                    shuma = await context.Paga.Where(q => q.KompaniaId == KompaniaId).SumAsync(q => q.Bruto);
                }
                else
                {
                    shuma = await context.Paga.SumAsync(q => q.Bruto);
                }
            }

            if (!shuma.HasValue)
                return 0;
            else
                return shuma.Value;
        }

        public async Task<List<Data.Paga>> ListaPagave(int? KompaniaId, string UserId)
        {
            List<Data.Paga> pagat = new List<Data.Paga>();
            if (!string.IsNullOrEmpty(UserId))
            {
                var punetori = await context.Punetori.FirstOrDefaultAsync(q => q.UserId == UserId);

                pagat = await context.Paga.Include(q => q.Punetori)
                                          .Include(q => q.Punetori.Kompania).Where(q => q.PunetoriId == punetori.Id).OrderByDescending(q=>q.Id).Take(10).ToListAsync();
            }
            else
            {
                if (KompaniaId.HasValue)
                {
                    pagat = await context.Paga.Include(q => q.Punetori)
                                              .Include(q => q.Punetori.Kompania).Where(q => q.Punetori.KompaniaId == KompaniaId).OrderByDescending(q => q.Id).Take(10).ToListAsync();
                }
                else
                {
                    pagat = await context.Paga.Include(q => q.Punetori)
                                              .Include(q => q.Punetori.Kompania).OrderByDescending(q => q.Id).Take(10).ToListAsync();
                }
            }

            return pagat;
        }

        public async Task<List<Data.Bonuset>> ListaBonuseve(int? KompaniaId, string UserId)
        {
            List<Data.Bonuset> bonuset = new List<Data.Bonuset>();
            if (!string.IsNullOrEmpty(UserId))
            {
                var punetori = await context.Punetori.FirstOrDefaultAsync(q => q.UserId == UserId);

                bonuset = await context.Bonuset.Include(q => q.Punetori)
                                          .Include(q => q.Punetori.Kompania).Where(q => q.PunetoriId == punetori.Id).OrderByDescending(q => q.Id).Take(10).ToListAsync();
            }
            else
            {
                if (KompaniaId.HasValue)
                {
                    bonuset = await context.Bonuset.Include(q => q.Punetori)
                                              .Include(q => q.Punetori.Kompania).Where(q => q.Punetori.KompaniaId == KompaniaId).OrderByDescending(q => q.Id).Take(10).ToListAsync();
                }
                else
                {
                    bonuset = await context.Bonuset.Include(q => q.Punetori)
                                              .Include(q => q.Punetori.Kompania).OrderByDescending(q => q.Id).Take(10).ToListAsync();
                }
            }

            return bonuset;
        }
    }
}

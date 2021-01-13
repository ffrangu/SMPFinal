using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMP.Data;
using SMP.Models.Kompania;
using SMP.ViewModels.Paga;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Paga
{
    public class PagaRepository : GenericRepository<Data.Paga>, IPagaRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public PagaRepository(UserManager<ApplicationUser> _userManager, ApplicationDbContext context) : base(context)
        {
            userManager = _userManager;
        }


        public async Task<IList<Data.Paga>> BulkInsertPaga(IList<Data.Paga> pagat)
        {
            await context.BulkInsertAsync(pagat);

            await context.SaveChangesAsync();

            return pagat;
        }

        public async Task<bool> IsPagaInserted(int KompaniaId, int Viti, int Muaji, string role)
        {
            if(role == "HR")
            {
                return await context.Paga.Include(q=>q.Punetori.Kompania).AnyAsync(q => q.Punetori.KompaniaId == KompaniaId && q.Viti == Viti && q.Muaji == Muaji);

            }
            else
            {
                return await context.Paga.AnyAsync(q => q.KompaniaId == KompaniaId && q.Viti == Viti && q.Muaji == Muaji);
            }
        }

        public async Task KompaniaSubTreeAsync(IEnumerable<Data.Kompania> companies, Data.Kompania company, IList<Data.Punetori> punetoret)
        {
            var subCompanies = companies.Where(q => q.ParentId == company.Id);

            foreach (var item in subCompanies)
            {
                var punetoris = await context.Punetori.Where(q => q.KompaniaId == item.Id).Include(q => q.Grada).ToListAsync();

                foreach (var punetori in punetoris)
                {
                    punetoret.Add(punetori);
                }

                await KompaniaSubTreeAsync(companies, item, punetoret);
            }
        }

        public async Task<IList<Data.Punetori>> GetPunetoret(int KompaniaId)
        {
            var kompania = await context.Kompania.Where(q=>q.Id == KompaniaId).ToListAsync();
            var kompanite = await context.Kompania.ToListAsync();
            IList<Data.Punetori> punetoret = new List<Data.Punetori>();

            foreach (var item in kompania)
            {
                var punetoris = await context.Punetori.Where(q => q.KompaniaId == item.Id).Include(q=>q.Grada).ToListAsync();


                foreach (var punetori in punetoris)
                {
                    punetoret.Add(punetori);
                }

                await KompaniaSubTreeAsync(kompanite, item, punetoret);
            }

            return punetoret;
        }

        public async Task<decimal> Tatimi(decimal paganeto, bool primare)
        {
            decimal tatimi = 0m;

            var vleratTatimore = await context.Tatimi.FirstOrDefaultAsync();

            decimal perqindja1 = 0m;
            decimal perqindja2 = 0m;
            decimal perqindja3 = 0m;
            decimal perqindja4 = 0m;

            if (primare)
            {
                if (paganeto < vleratTatimore.VleraPare)
                {
                    perqindja1 = 0m;
                }
                if (paganeto > vleratTatimore.VleraPare && paganeto < vleratTatimore.VleraDyte)
                {
                    perqindja2 = (paganeto - vleratTatimore.VleraPare) * (vleratTatimore.PerqindjaPare / 100);
                }
                else if (paganeto > vleratTatimore.VleraPare && paganeto > vleratTatimore.VleraDyte)
                {
                    perqindja2 = (vleratTatimore.VleraDyte - vleratTatimore.VleraPare) * (vleratTatimore.PerqindjaPare / 100);
                }
                if (paganeto > vleratTatimore.VleraDyte && paganeto < vleratTatimore.VleraTrete)
                {
                    perqindja3 = (paganeto - vleratTatimore.VleraDyte) * (vleratTatimore.PerqindjaDyte / 100);
                }
                else if (paganeto > vleratTatimore.VleraDyte && paganeto > vleratTatimore.VleraTrete)
                {
                    perqindja3 = (vleratTatimore.VleraTrete - vleratTatimore.VleraDyte) * (vleratTatimore.PerqindjaDyte / 100);
                    perqindja4 = (paganeto - vleratTatimore.VleraTrete) * (vleratTatimore.PerqindjaTrete / 100);
                }
            }
            else
            {
                var tatimi_10 = paganeto * (vleratTatimore.PerqindjaTrete / 100);
                perqindja4 = tatimi_10;
            }

            tatimi = perqindja1 + perqindja2 + perqindja3 + perqindja4;

            return tatimi;
        }

        public List<PagaViewModel> GetPagat(string role, int? KompaniaId)
        {
            var pagat = new List<PagaViewModel>();

            if (role == "Administrator")
            {
                var pagatGrouped = context.Paga.Include(q => q.Kompania).OrderByDescending(q => q.Id).ToList().GroupBy(q => new { q.Viti, q.Muaji, q.KompaniaId });
                pagat = (from p in pagatGrouped
                         select new PagaViewModel
                         {
                             Muaji = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(p.FirstOrDefault().Muaji),
                             Month = p.FirstOrDefault().Muaji,
                             Viti = p.FirstOrDefault().Viti,
                             Kompania = p.FirstOrDefault().Kompania.Emri,
                             Data = p.FirstOrDefault().DataEkzekutimit.Day + "/" + p.FirstOrDefault().DataEkzekutimit.Month + "/" + p.FirstOrDefault().DataEkzekutimit.Year,
                             Pershkrimi = p.FirstOrDefault().Pershkrimi,
                             KompaniaId = p.FirstOrDefault().KompaniaId
                         }).ToList();
            }
            else
            {
                var pagatGrouped = context.Paga.Where(q => q.KompaniaId == KompaniaId).Include(q => q.Kompania).OrderByDescending(q => q.Id).ToList().GroupBy(q => new { q.Viti, q.Muaji, q.KompaniaId });

                pagat = (from p in pagatGrouped
                         select new PagaViewModel
                         {
                             Muaji = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(p.FirstOrDefault().Muaji),
                             Month = p.FirstOrDefault().Muaji,
                             Viti = p.FirstOrDefault().Viti,
                             Kompania = p.FirstOrDefault().Kompania.Emri,
                             Data = p.FirstOrDefault().DataEkzekutimit.Day + "/" + p.FirstOrDefault().DataEkzekutimit.Month + "/" + p.FirstOrDefault().DataEkzekutimit.Year,
                             Pershkrimi = p.FirstOrDefault().Pershkrimi,
                             KompaniaId = p.FirstOrDefault().KompaniaId
                         }).ToList();
            }

            return pagat;
        }

        public async Task<List<AllPagat>> GetAllPagat(int m, int v, int k)
        {
            var allPagat = new List<AllPagat>();

            var pagat = await context.Paga.Where(q => q.Viti == v && q.Muaji == m && q.KompaniaId == k)
                              .Include(q => q.Punetori)
                              .Include(q => q.Punetori.Pozita)
                              .Include(q => q.Grada)
                              .Include(q => q.Kompania).OrderByDescending(q=>q.Id).ToListAsync();

            allPagat = (from p in pagat
                        select new AllPagat { 
                            Id = p.Id,
                            Punetori = p.Punetori.Emri + " " + p.Punetori.Mbiemri,
                            Kompania = p.Kompania.Emri,
                            Pozita = p.Punetori.Pozita.Emri,
                            Grada = p.Grada.Emri,
                            Viti = p.Viti,
                            Muaji = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(p.Muaji),
                            Bruto = p.Bruto,
                            Kontributi = p.KontributiPunetori,
                            PagaPaTatimuar = p.PagaTatim,
                            Tatimi = p.Tatimi,
                            PagaNeto = p.PagaNeto,
                            Bonuse = p.Bonuse,
                            PagaFinale = p.PagaFinale
                        }).ToList();

            return allPagat;
        }

        public async Task<Data.Paga> GetPaga(int Id)
        {
            var paga = await context.Paga.Where(q => q.Id == Id).Include(q => q.Punetori)
                                                              .Include(q => q.Kompania)
                                                              .Include(q => q.Punetori.Pozita)
                                                              .Include(q => q.Grada).FirstOrDefaultAsync();

            return paga;
                                                              
        }

        public async Task<ApplicationUser> GetPunetoriUser(int PunetoriId)
        {
            var punetori = await context.Punetori.FindAsync(PunetoriId);

            var user = await userManager.FindByIdAsync(punetori.UserId);

            return user;
        }

        public async Task<decimal> GetBonus(int PunetoriId, int Muaji, int Viti, bool Bruto)
        {
            decimal shuma = 0m;
            if(Bruto)
            {
                var brutoBonuset = await context.Bonuset.Where(q => q.PunetoriId == PunetoriId && q.Muaji == Muaji && q.Viti == Viti && q.Bruto).ToListAsync();

                if(brutoBonuset.Count > 0)
                {
                    shuma = brutoBonuset.Sum(q => q.Vlera);
                }
            }
            else
            {
                var netoBonuset = await context.Bonuset.Where(q => q.PunetoriId == PunetoriId && q.Muaji == Muaji && q.Viti == Viti && !q.Bruto).ToListAsync();

                if(netoBonuset.Count > 0)
                {
                    shuma = netoBonuset.Sum(q => q.Vlera);
                }
            }

            return shuma;
        }
    }
}

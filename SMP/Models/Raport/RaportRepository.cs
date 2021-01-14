using Microsoft.EntityFrameworkCore;
using SMP.Data;
using SMP.ViewModels.Paga;
using SMP.ViewModels.Punetori;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Raport
{
    public class RaportRepository : IRaportRepository
    {
        protected readonly ApplicationDbContext context;

        public RaportRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<List<AllPagat>> GetAllPagat(int? PunetoriId, int? KompaniaId, int? Viti, int? Muaji, int? BankaId, int? GradaId)
        {
            List<Data.Paga> pagat = new List<Data.Paga>();
            if(PunetoriId.HasValue)
            {
                pagat = await context.Paga.Where(q => q.PunetoriId == PunetoriId.Value)
                                          .Include(q => q.Punetori)
                                          .Include(q => q.Punetori.Pozita)
                                          .Include(q => q.Grada)
                                          .Include(q => q.Punetori.Banka)
                                          .Include(q => q.Kompania).OrderByDescending(q => q.Id).ToListAsync();
            }
            else
            {
                if(KompaniaId.HasValue)
                {
                    pagat = await context.Paga.Where(q => q.KompaniaId == KompaniaId)
                                              .Include(q => q.Punetori)
                                              .Include(q => q.Punetori.Pozita)
                                              .Include(q => q.Grada)
                                              .Include(q => q.Punetori.Banka)
                                              .Include(q => q.Kompania).OrderByDescending(q => q.Id).ToListAsync();
                }
                else
                {
                    pagat = await context.Paga
                          .Include(q => q.Punetori)
                          .Include(q => q.Punetori.Pozita)
                          .Include(q => q.Grada)
                          .Include(q => q.Punetori.Banka)
                          .Include(q => q.Kompania).OrderByDescending(q => q.Id).ToListAsync();
                }
            }

            if(Viti.HasValue)
            {
                pagat = pagat.Where(q => q.Viti == Viti.Value).ToList();
            }

            if (Muaji.HasValue)
            {
                pagat = pagat.Where(q => q.Muaji == Muaji.Value).ToList();
            }

            if (GradaId.HasValue)
            {
                pagat = pagat.Where(q => q.GradaId == GradaId.Value).ToList();
            }

            if (BankaId.HasValue)
            {
                pagat = pagat.Where(q => q.Punetori.Banka.Id == BankaId.Value).ToList();
            }

            var allPagat = (from p in pagat
                            select new AllPagat
                            {
                                Id = p.Id,
                                PunetoriId = p.PunetoriId,
                                Punetori = p.Punetori.Emri + " " + p.Punetori.Mbiemri,
                                Kompania = p.Kompania.Emri,
                                Pozita = p.Punetori.Pozita.Emri,
                                Grada = p.Grada.Emri,
                                Viti = p.Viti,
                                Muaji = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(p.Muaji),
                                MuajiInt = p.Muaji,
                                Bruto = p.Bruto,
                                Kontributi = p.KontributiPunetori,
                                PagaPaTatimuar = p.PagaTatim,
                                Tatimi = p.Tatimi,
                                PagaNeto = p.PagaNeto,
                                Bonuse = p.Bonuse,
                                BonuseNeto = p.BonuseNeto,
                                PagaFinale = p.PagaFinale,
                                Banka = p.Punetori.Banka.Emri,
                                NumriPersonal = p.Punetori.NumriPersonal
                            }).ToList();

            return allPagat;
        }

        public async Task<List<PunetoriListViewModel>> GetAllPunetoret(int? PunetoriId, int? KompaniaId, int? BankaId, int? GradaId)
        {
            List<Data.Punetori> punetoret = new List<Data.Punetori>();

            if(KompaniaId.HasValue)
            {
                punetoret = await context.Punetori.Where(q => q.KompaniaId == KompaniaId.Value)
                                                  .Include(q => q.Pozita)
                                                  .Include(q => q.Banka)
                                                  .Include(q => q.Kompania)
                                                  .Include(q => q.Komuna)
                                                  .Include(q => q.Departamenti)
                                                  .Include(q => q.Grada).ToListAsync();
            }
            else
            {
                punetoret = await context.Punetori
                                  .Include(q => q.Pozita)
                                  .Include(q => q.Banka)
                                  .Include(q => q.Kompania)
                                  .Include(q => q.Komuna)
                                  .Include(q => q.Departamenti)
                                  .Include(q => q.Grada).ToListAsync();
            }

            if(PunetoriId.HasValue)
            {
                punetoret = punetoret.Where(q => q.Id == PunetoriId).ToList();
            }

            if (BankaId.HasValue)
            {
                punetoret = punetoret.Where(q => q.BankaId == BankaId).ToList();
            }

            if (GradaId.HasValue)
            {
                punetoret = punetoret.Where(q => q.GradaId == GradaId).ToList();
            }

            var lista = (from p in punetoret
                        select new PunetoriListViewModel
                        {
                            Id = p.Id,
                            Emri = p.Emri + " " + p.Mbiemri,
                            Kompania = p.Kompania.Emri,
                            Pozita = p.Pozita.Emri,
                            Grada = p.Grada.Emri,
                            Banka = p.Banka.Emri,
                            NumriPersonal = p.NumriPersonal,
                            Telefoni = p.Telefoni,
                            Email = p.Email,
                            Datelindja = p.Datelindja,
                            Ditelindja = p.Datelindja.Day + "/" + p.Datelindja.Month + "/" + p.Datelindja.Year,
                            Adresa = p.Adresa,
                            Komuna = p.Komuna.Emri,
                            Departamenti = p.Departamenti.Emri,
                            Xhirollogaria = p.Xhirollogaria
                        }).ToList();

            return lista;
        }

        public async Task<List<AllPagat>> Payslip(int? PunetoriId, int? KompaniaId, int? Viti, int? Muaji, int? BankaId, int? GradaId)
        {
            List<AllPagat> payslip = new List<AllPagat>();

            var pagat = await GetAllPagat(PunetoriId, KompaniaId, Viti, Muaji, BankaId, GradaId);

            var pagatGrouped = from p in pagat
                               group p by new { p.PunetoriId } into g
                               select g;

            foreach (var item in pagatGrouped)
            {
                var add = new AllPagat();

                var pagabruto = item.Sum(q => q.Bruto);
                var bonusebruto = item.Sum(q => q.Bonuse);
                var kontributi = item.Sum(q => q.Kontributi);
                var pagapatatimuar = item.Sum(q => q.PagaPaTatimuar);
                var tatimi = item.Sum(q => q.Tatimi);
                var paganeto = item.Sum(q => q.PagaNeto);
                var bonuseneto = item.Sum(q => q.BonuseNeto);
                var pagafinale = item.Sum(q => q.PagaFinale);

                if (!bonusebruto.HasValue)
                    bonusebruto = 0;
                if (!bonuseneto.HasValue)
                    bonuseneto = 0;

                add.NumriPersonal = item.FirstOrDefault().NumriPersonal;
                add.PunetoriId = item.Key.PunetoriId;
                add.Punetori = item.FirstOrDefault().Punetori;
                add.NumriPersonal = item.FirstOrDefault().NumriPersonal;
                add.Kompania = item.FirstOrDefault().Kompania;
                add.Bruto = pagabruto;
                add.Bonuse = bonusebruto;
                add.BonuseNeto = bonuseneto;
                add.Kontributi = kontributi;
                add.PagaPaTatimuar = pagabruto - kontributi;
                add.Tatimi = tatimi;
                add.PagaNeto = pagabruto - kontributi - tatimi;
                add.PagaFinale = pagabruto - kontributi - tatimi + bonuseneto.Value;
                add.Muaji = item.FirstOrDefault().Muaji;
                add.Viti = item.FirstOrDefault().Viti;

                payslip.Add(add);

            }

            return payslip;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMP.Data;
using SMP.Helpers;
using SMP.Models.Kompania;
using SMP.Models.Paga;
using SMP.Models.Punetori;
using SMP.ViewModels.Paga;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Controllers
{
    [Authorize(Roles = "Administrator, HR")]
    public class PagaController : BaseController
    {
        protected UserManager<ApplicationUser> userManager;
        protected RoleManager<IdentityRole> roleManager;
        public AlertService alertService { get; }

        private IKompaniaRepository kompaniaRepository;
        private IPunetoriRepository punetoriRepository;
        private IPagaRepository pagaRepository;

        public PagaController(IKompaniaRepository _kompaniaRepository, IPagaRepository _pagaRepository, IPunetoriRepository _punetoriRepository, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager, AlertService _alertService)
        : base(_roleManager, _userManager)
        {
            alertService = _alertService;
            userManager = _userManager;
            roleManager = _roleManager;
            kompaniaRepository = _kompaniaRepository;
            pagaRepository = _pagaRepository;
            punetoriRepository = _punetoriRepository;
        }

        // GET: PagaController
        public ActionResult Index()
        {
            string role = User.IsInRole("HR") ? "HR" : "Administrator";
            int? KompaniaId = User.IsInRole("HR") ? user.KompaniaId : (int?)null;

            var pagat = pagaRepository.GetPagat(role, KompaniaId);

            return View(pagat);
        }

        // GET: PagaController/Details/5
        public async Task<ActionResult> DetailsAsync(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorTitle = $"Id nuk mund të jetë null";
                return View("_NotFound");
            }

            var paga = await pagaRepository.GetPaga(id.Value);

            if(paga == null)
            {
                ViewBag.ErrorTitle = $"Paga nuk mund të gjendet";
                return View("_NotFound");
            }

            if(User.IsInRole("HR"))
            {
                if(paga.KompaniaId != user.KompaniaId)
                {
                    alertService.Information("Nuk keni qasje në këtë pagë");
                    return RedirectToAction("All", new { m = paga.Muaji, v = paga.Viti, k = paga.KompaniaId });
                }   
            }

            return View(paga);
        }

        public async Task<ActionResult> All(int? m,int? v, int? k)
        {
            if(m == null || v == null || k == null)
            {
                ViewBag.ErrorTitle = $"Parametrat nuk mund të jenë null";
                return View("_NotFound");
            }

            if(User.IsInRole("HR"))
            {
                k = user.KompaniaId;
            }

            var allPagat = await pagaRepository.GetAllPagat(m.Value, v.Value, k.Value);

            return View(allPagat);
        }

        [Authorize(Roles = "Administrator, HR")]
        // GET: PagaController/Create
        public async Task<ActionResult> CreateAsync()
        {
            string role = User.IsInRole("HR") ? "HR" : "Administrator";

            ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectListBasedOnRole(role, user.KompaniaId);
            //ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectList(null,false,false);

            ViewBag.Muaji = new SelectList(Enumerable.Range(1, 12).Select(x =>
                                new SelectListItem()
                                {
                                    Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1] + " (" + x + ")",
                                    Value = x.ToString()
                                }), "Value", "Text", DateTime.Today.Month.ToString());


            ViewBag.Viti = new SelectList(Enumerable.Range(DateTime.Today.Year - 1, 2).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.ToString(),
                               Value = x.ToString()
                           }), "Value", "Text", DateTime.Today.Year.ToString());

            return View();
        }

        // POST: PagaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(PagaCreateViewModel model)
        {
            string role = User.IsInRole("HR") ? "HR" : "Administrator";

            if (ModelState.IsValid)
            {
                try
                {
                    //check if paga is inserted for the selected month;
                    if (await pagaRepository.IsPagaInserted(model.KompaniaId, model.Viti, model.Muaji, role))
                    {
                        alertService.Information("Pagat për këtë kompani për këtë muaj dhe vit janë ekzekutuar");
                        return RedirectToAction("Create");
                    }

                    var punetoret = await pagaRepository.GetPunetoret(model.KompaniaId);

                    IList<Data.Paga> pagas = new List<Data.Paga>();

                    foreach (var item in punetoret)
                    {
                        var brutoBonuset = await pagaRepository.GetBonus(item.Id, model.Muaji, model.Viti, true);
                        var netoBonuset = await pagaRepository.GetBonus(item.Id, model.Muaji, model.Viti, false);

                        decimal perqindja = (5.00m / 100.00m);

                        var pagaBruto = item.Grada.PagaMujore + brutoBonuset;

                        decimal kontributi = (perqindja * pagaBruto);

                        decimal paganeto = pagaBruto - kontributi;

                        decimal tatimi = await pagaRepository.Tatimi(paganeto: paganeto, primare: true);

                        decimal pagatatimuar = paganeto - tatimi;

                        decimal pagafinale = pagatatimuar + netoBonuset;

                        pagas.Add(new Paga { 
                            PunetoriId = item.Id,
                            GradaId = item.GradaId,
                            Viti = model.Viti,
                            Muaji = model.Muaji,
                            Bruto = pagaBruto,
                            KontributiPunetori = kontributi,
                            KontributiPunedhenesi = kontributi,
                            PagaTatim = paganeto,
                            Tatimi = tatimi,
                            PagaNeto = pagatatimuar,
                            Bonuse = brutoBonuset,
                            BonuseNeto = netoBonuset,
                            PagaFinale = pagafinale,
                            MenyraEkzekutimit = 1, // automatike
                            DataEkzekutimit = DateTime.Now,
                            CreatedBy = user.UserId,
                            Pershkrimi = model.Pershkrimi,
                            KompaniaId = model.KompaniaId
                        });
                    }

                    var inserted = await pagaRepository.BulkInsertPaga(pagas);

                    alertService.Success("Pagat janë ekzekutuar me sukses");

                    return RedirectToAction("Index");
                }
                catch
                {
                    alertService.Information("Ka ndodhur një gabim, provoni përsëri");

                    ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectListBasedOnRole(role, user.KompaniaId);
                    //ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectList(null,false,false);

                    ViewBag.Muaji = new SelectList(Enumerable.Range(1, 12).Select(x =>
                                        new SelectListItem()
                                        {
                                            Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1] + " (" + x + ")",
                                            Value = x.ToString()
                                        }), "Value", "Text", DateTime.Today.Month.ToString());


                    ViewBag.Viti = new SelectList(Enumerable.Range(DateTime.Today.Year, 1).Select(x =>
                                   new SelectListItem()
                                   {
                                       Text = x.ToString(),
                                       Value = x.ToString()
                                   }), "Value", "Text");
                    return View(model);
                }
            }

            ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectListBasedOnRole(role, user.KompaniaId);
            //ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectList(null,false,false);

            ViewBag.Muaji = new SelectList(Enumerable.Range(1, 12).Select(x =>
                                new SelectListItem()
                                {
                                    Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1] + " (" + x + ")",
                                    Value = x.ToString()
                                }), "Value", "Text", DateTime.Today.Month.ToString());


            ViewBag.Viti = new SelectList(Enumerable.Range(DateTime.Today.Year, 1).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.ToString(),
                               Value = x.ToString()
                           }), "Value", "Text");
            return View(model);
        }

        // GET: PagaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PagaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PagaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PagaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

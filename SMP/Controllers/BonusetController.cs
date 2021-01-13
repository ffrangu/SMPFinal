using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMP.Data;
using SMP.Helpers;
using SMP.Models.Bonuset;
using SMP.Models.Punetori;
using SMP.ViewModels.Bonuset;

namespace SMP.Controllers
{
    public class BonusetController : BaseController
    {

        protected UserManager<ApplicationUser> userManager;
        protected RoleManager<IdentityRole> roleManager;
        public AlertService alertService { get; }

        private IPunetoriRepository punetoriRepository;
        private IBonusetRepository bonusetRepository;

        


        public BonusetController(IPunetoriRepository _punetoriRepository, IBonusetRepository _bonusetRepository,RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager, AlertService _alertService)
        : base(_roleManager, _userManager)
        {
            alertService = _alertService;
            userManager = _userManager;
            roleManager = _roleManager;
            punetoriRepository = _punetoriRepository;
            bonusetRepository = _bonusetRepository;

        }
        // GET: BonusetController
        public async Task<ActionResult> IndexAsync()
        {
            string role = User.IsInRole("HR") ? "HR" : "Administrator";
            var bonuset = await bonusetRepository.getBonusetbyKompaniaId(user.KompaniaId, role);
            var listItems = new List<BonusetListViewModel>();

            foreach (var item in bonuset)
            {
                listItems.Add(new BonusetListViewModel
                {
                    Id = item.Id,
                    Muaji = item.Muaji,
                    Viti = item.Viti,
                    PunetoriId = item.PunetoriId,
                    Punetori = item.Punetori.Emri,
                    Pershkrimi = item.Pershkrimi,
                    Vlera = item.Vlera


                });

            }

            return View(listItems);
        }

        // GET: BonusetController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BonusetController/Create
        public async Task<ActionResult> CreateAsync()
        {
            string role = User.IsInRole("HR") ? "HR" : "Administrator";
            ViewBag.Punetori = await punetoriRepository.PunetoretSelectList(user.KompaniaId, role);

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

        // POST: BonusetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(BonusetCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var addBonus = new Bonuset
                    {
                        Id = model.Id,
                        Muaji = model.Muaji,
                        Viti = model.Viti,
                        PunetoriId = model.PunetoriId,
                        Pershkrimi = model.Pershkrimi,
                        Vlera = model.Vlera,
                        Bruto = model.Bruto,
                        Created = DateTime.Now,
                        CreatedBy = user.UserId
                    };

                    var result =await bonusetRepository.AddAsync(addBonus);
                    alertService.Success("Bonusi u shtua me sukses!");

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    alertService.Danger("Diqka shkoi gabim, provoni perseri!");
                    return View(model);
                }
            }
            alertService.Information("Plotesoni te gjitha fushat!");
            return View(model);
        }

        // GET: BonusetController/Edit/5
        public async Task<ActionResult> EditAsync(int? id)
        {
            ViewBag.AddError = false;
            if (id == null)
            {
                ViewBag.ErrorTitle = $"Id cannot be null";
                return View("_NotFound");
            }

            var bonus = await bonusetRepository.Get(id);

            if (bonus == null)
            {
                ViewBag.ErrorTitle = $"Bonusi me këtë { id } nuk është gjetur!";
                return View("_NotFound");
            }

            BonusetEditViewModel model = new BonusetEditViewModel
            {
                Id = bonus.Id,
                Muaji = bonus.Muaji,
                Viti = bonus.Viti,
                PunetoriId = bonus.PunetoriId,
                Pershkrimi = bonus.Pershkrimi,
                Vlera = bonus.Vlera,
                Bruto = bonus.Bruto
            };

            string role = User.IsInRole("HR") ? "HR" : "Administrator";
            ViewBag.Punetori = await punetoriRepository.PunetoretSelectList(user.KompaniaId, role);

            ViewBag.Muaji = new SelectList(Enumerable.Range(1, 12).Select(x =>
                                 new SelectListItem()
                                 {
                                     Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1] + " (" + x + ")",
                                     Value = x.ToString()
                                 }), "Value", "Text", bonus.Muaji);


            ViewBag.Viti = new SelectList(Enumerable.Range(DateTime.Today.Year - 1, 2).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.ToString(),
                               Value = x.ToString()
                           }), "Value", "Text", bonus.Viti);
            return View(model);
        }

        // POST: BonusetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(BonusetEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var editBonus = await bonusetRepository.Get(model.Id);
                    editBonus.Muaji = model.Muaji;
                    editBonus.Viti = model.Viti;
                    editBonus.PunetoriId = model.PunetoriId;
                    editBonus.Pershkrimi = model.Pershkrimi;
                    editBonus.Vlera = model.Vlera;
                    editBonus.Bruto = model.Bruto;
                    editBonus.Created = DateTime.Now;
                    editBonus.CreatedBy = user.UserId;

                    var editedBonus = await bonusetRepository.Update(editBonus);

                    alertService.Success("Punetori u editua me sukses!");

                    return RedirectToAction(nameof(Index));


                }
                catch (Exception)
                {

                    alertService.Danger("Diqka shkoi keq!");
                    return View(model);
                }
            }
            alertService.Information("Mbushi te gjitha fushat!");

            return View(model);
        }

        // GET: BonusetController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BonusetController/Delete/5
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

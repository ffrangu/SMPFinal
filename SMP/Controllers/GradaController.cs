using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMP.Data;
using SMP.Helpers;
using SMP.Models.Grada;
using SMP.ViewModels.Grada;

namespace SMP.Controllers
{
    public class GradaController : BaseController
    {

        protected UserManager<ApplicationUser> userManager;
        protected RoleManager<IdentityRole> roleManager;
        public AlertService alertService { get; }

        private IGradaRepository gradaRepository;

        

        public GradaController(IGradaRepository _gradaRepository, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager,
            AlertService _alertService)
            : base(_roleManager, _userManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            alertService = _alertService;
            gradaRepository = _gradaRepository;
        }


        // GET: GradaController
        public async  Task<ActionResult> IndexAsync()
        {
            var gradat = await gradaRepository.GetAll();
            var listItems = new List<GradaListViewModel>();

            foreach (var item in gradat)
            {
                listItems.Add(new GradaListViewModel
                {
                    Id = item.Id,
                    Emri = item.Emri,
                    PagaMujore = item.PagaMujore,
                    PagaVjetore = item.PagaVjetore


                });
            }

            return View(listItems);
        }

        // GET: GradaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GradaController/Create
        public ActionResult Create()
        {
            ViewBag.AddError = false;
            return View();
        }

        // POST: GradaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(GradaCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var addGrada = new Grada
                    {
                        Emri = model.Emri,
                        PagaMujore = model.PagaMujore,
                        PagaVjetore = model.PagaVjetore
                    };

                    var result = await gradaRepository.AddAsync(addGrada);
                    alertService.Success("Grada u shtua me sukses!");

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

        // GET: GradaController/Edit/5
        public async Task<ActionResult> EditAsync(int? id)
        {
            ViewBag.AddError = false;
            if (id == null)
            {
                ViewBag.ErrorTitle = $"Id cannot be null";
                return View("_NotFound");
            }
            var grada = await gradaRepository.Get(id);

            if (grada == null)
            {
                ViewBag.ErrorTitle = $"Banka me këtë { id } nuk është gjetur!";
                return View("_NotFound");
            }

            GradaEditViewModel model = new GradaEditViewModel
            {
                Id = grada.Id,
                Emri = grada.Emri,
                PagaMujore = grada.PagaMujore,
                PagaVjetore = grada.PagaVjetore
            };

            return View(model);
        }

        // POST: GradaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(GradaEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var editGrada = await gradaRepository.Get(model.Id);
                    editGrada.Emri = model.Emri;
                    editGrada.PagaMujore = model.PagaMujore;
                    editGrada.PagaVjetore = model.PagaVjetore;

                    var editedGrada = await gradaRepository.Update(editGrada);


                    alertService.Success("Grada u editua me sukses!");



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

        // GET: GradaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GradaController/Delete/5
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

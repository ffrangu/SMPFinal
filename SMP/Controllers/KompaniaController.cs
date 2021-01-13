using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMP.Data;
using SMP.Helpers;
using SMP.Models.Kompania;
using SMP.ViewModels.Kompania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class KompaniaController : BaseController
    {
        public AlertService alertService { get; }

        private IKompaniaRepository kompaniaRepository;

        public KompaniaController(IKompaniaRepository _kompaniaRepository, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager,
            AlertService _alertService)
            : base(_roleManager, _userManager)
        {
            alertService = _alertService;
            kompaniaRepository = _kompaniaRepository;
        }

        // GET: KompaniaController
        public async Task<ActionResult> IndexAsync()
        {
            var list = await kompaniaRepository.KompaniaListModel();

            return View(list);
        }

        // GET: KompaniaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KompaniaController/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.KomunaId = await kompaniaRepository.LoadKomuna(null);
            ViewBag.ParentId = await kompaniaRepository.KompaniaSelectList(null, false, false);

            return View();
        }

        // POST: KompaniaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(KompaniaCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var addModel = await kompaniaRepository.KompaniaAddModel(model);

                    var added = await kompaniaRepository.AddAsync(addModel);

                    alertService.Success("Kompania eshte regjistruar me sukses");

                    return RedirectToAction("Edit", new { id = added.Id });
                }
                catch
                {
                    alertService.Information("Ka ndodhur nje gabim gjate insertimit te dhenave, provoni perseri");
                    ViewBag.KomunaId = await kompaniaRepository.LoadKomuna(null);
                    ViewBag.ParentId = await kompaniaRepository.KompaniaSelectList(null, false, false);

                    return View(model);
                }
            }

            alertService.Information("Plotesoni te dhenat obligative");
            ViewBag.KomunaId = await kompaniaRepository.LoadKomuna(null);
            ViewBag.ParentId = await kompaniaRepository.KompaniaSelectList(null, false, false);
            return View(model);
        }

        // GET: KompaniaController/Edit/5
        public async Task<ActionResult> EditAsync(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorTitle = $"Id nuk mund të jetë null";
                return View("_NotFound");
            }

            var kompania = await kompaniaRepository.Get(id);

            if(kompania == null)
            {
                ViewBag.ErrorTitle = $"Kompania me këtë id { id } nuk mund të gjendet.";
                return View("_NotFound");
            }

            var model = kompaniaRepository.KompaniaEditModel(kompania);

            ViewBag.KomunaId = await kompaniaRepository.LoadKomuna(kompania.KomunaId);
            ViewBag.ParentId = await kompaniaRepository.KompaniaSelectList(kompania.Id, false, false);

            return View(model);
        }

        // POST: KompaniaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(KompaniaEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var editModel = await kompaniaRepository.KompaniaOnPostEditModel(model);

                    var edited = await kompaniaRepository.Update(editModel);

                    alertService.Success("Kompania eshte modifikuar me sukses");

                    return RedirectToAction("Edit", new { id = edited.Id });
                }
                catch
                {
                    alertService.Information("Ka ndodhur nje gabim gjate insertimit te dhenave, provoni perseri");
                    ViewBag.KomunaId = await kompaniaRepository.LoadKomuna(model.KomunaId);
                    ViewBag.ParentId = await kompaniaRepository.KompaniaSelectList(model.Id, false, false);

                    return View(model);
                }
            }

            alertService.Information("Plotesoni te dhenat obligative");
            ViewBag.KomunaId = await kompaniaRepository.LoadKomuna(model.KomunaId);
            ViewBag.ParentId = await kompaniaRepository.KompaniaSelectList(model.Id, false, false);

            return View(model);
        }

        // GET: KompaniaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KompaniaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}

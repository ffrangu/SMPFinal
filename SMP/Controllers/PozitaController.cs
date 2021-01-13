using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMP.Data;
using SMP.Helpers;
using SMP.Models.Departamenti;
using SMP.Models.Kompania;
using SMP.Models.Pozita;
using SMP.ViewModels.Pozita;

namespace SMP.Controllers
{
    public class PozitaController : BaseController
    {

        protected UserManager<ApplicationUser> userManager;
        protected RoleManager<IdentityRole> roleManager;
        public AlertService alertService { get; }

        private IPozitaRepository pozitaRepository;

        private IDepartamentiRepository departamentiRepository;

        private IKompaniaRepository kompaniaRepository;


        public PozitaController(IPozitaRepository _pozitaRepository,IDepartamentiRepository _departamentiRepository,IKompaniaRepository _kompaniaRepository, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager,
            AlertService _alertService)
            : base(_roleManager, _userManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            alertService = _alertService;
            pozitaRepository = _pozitaRepository;
            departamentiRepository = _departamentiRepository;
            kompaniaRepository = _kompaniaRepository;
        }


        // GET: PozitaController
        public async Task<ActionResult> IndexAsync()
        {
            var pozitat = await pozitaRepository.GetPozitat();
            var listItems = new List<PozitaListViewModel>();

            foreach (var item in pozitat)
            {
                listItems.Add(new PozitaListViewModel
                {
                    Id = item.Id,
                    DepartmentiId = item.DepartamentiId,
                    Departamenti = item.Departamenti.Emri,
                    KompaniaId = item.KompaniaId,
                    Kompania = item.Kompania.Emri,
                    Emri = item.Emri,
                    Status = item.Status,
                    Created = item.Created,
                    CreatedBy = item.CreatedBy
                    

                });
            }

            return View(listItems);
        }

        // GET: PozitaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PozitaController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.Departamenti = await departamentiRepository.DepartamentiSelectList(null,false,false);
            ViewBag.Kompania = await kompaniaRepository.KompaniaSelectList(null, false, false);
            return View();
        }

        // POST: PozitaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(PozitaCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var addPozite = new Pozita
                    {
                        KompaniaId = model.KompaniaId,
                        DepartamentiId = model.DepartamentiId,
                        Emri = model.Emri,
                        Status = model.Status,
                        Created = DateTime.Now,
                        CreatedBy = user.UserName

                    };

                    var result = await pozitaRepository.AddAsync(addPozite);

                    alertService.Success("Pozita u shtua me sukses!");

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

        // GET: PozitaController/Edit/5
        public async Task<ActionResult> EditAsync(int? id)
        {

            ViewBag.AddError = false;

            if (id == null)
            {
                ViewBag.ErrorTitle = $"Id cannot be null";
                return View("_NotFound");
            }
            var grada = await pozitaRepository.Get(id);

            if (grada == null)
            {
                ViewBag.ErrorTitle = $"Pozita me këtë { id } nuk është gjetur!";
                return View("_NotFound");
            }

            PozitaEditViewModel model = new PozitaEditViewModel
            {
                Id = grada.Id,
                Emri = grada.Emri,
                KompaniaId = grada.KompaniaId,
                DepartamentiId = grada.DepartamentiId,
                Created = grada.Created,
                CreatedBy = grada.CreatedBy,
                Status = grada.Status
            };

            ViewBag.Departamenti = await departamentiRepository.DepartamentiSelectList(null, false, false);
            ViewBag.Kompania = await kompaniaRepository.KompaniaSelectList(null, false, false);

            return View(model);
            
        }

        // POST: PozitaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(PozitaEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var editPozita = await pozitaRepository.Get(model.Id);
                    editPozita.KompaniaId = model.KompaniaId;
                    editPozita.DepartamentiId = model.DepartamentiId;
                    editPozita.Created = DateTime.Now;
                    editPozita.CreatedBy = user.UserName;
                    editPozita.Emri = model.Emri;
                    editPozita.Status = model.Status;

                    var editedPozita = await pozitaRepository.Update(editPozita);

                    alertService.Success("Pozita u editua me sukses!");



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

        // GET: PozitaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PozitaController/Delete/5
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


        public async Task<ActionResult> GetPozita (int DepartamentiId,int KompaniaId, long? selectedValue = null)
        {
            SelectList Pozita = null;
            try
            {
                if(DepartamentiId>0)
                {
                    var pozitat = await pozitaRepository.GetAll();
                    var filteredPozitat = pozitat.Where(x => x.DepartamentiId == DepartamentiId && x.KompaniaId == KompaniaId);
                    Pozita = new SelectList(filteredPozitat, "Id", "Emri");
                    ViewBag.Pozita = pozitat;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return Json(Pozita);
        }
    }
}

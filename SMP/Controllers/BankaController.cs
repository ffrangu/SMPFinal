using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMP.Data;
using SMP.ViewModels.Banka;
using SMP.Helpers;
using SMP.Models.Bank;

namespace SMP.Controllers
{
    public class BankaController : BaseController
    {

        protected UserManager<ApplicationUser> userManager;
        protected RoleManager<IdentityRole> roleManager;
        public AlertService alertService { get; }

        private IBankRepository bankRepository;
        

        public BankaController(IBankRepository _bankRepository,RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager,
            AlertService _alertService)
            : base(_roleManager, _userManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            alertService = _alertService;
            bankRepository = _bankRepository;
        }
        // GET: BankaController
        public async Task<ActionResult> IndexAsync()
        {
            var banks = await bankRepository.GetAll();
            var listItems = new List<BankaListViewModel>();

            foreach (var item in banks)
            {
                listItems.Add(new BankaListViewModel
                {
                    Id = item.Id,
                    Kodi = item.Kodi,
                    Emri  = item.Emri

                });
            }
            return View(listItems);
        }

        // GET: BankaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BankaController/Create
        public ActionResult Create()
        {
            ViewBag.AddError = false;
            return View();
        }

        // POST: BankaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<ActionResult> CreateAsync(BankaCreateViewModel model)
        {
           if(ModelState.IsValid)
            {
                try
                {
                    var addBank = new Banka
                    {
                        Emri = model.Emri,
                        Kodi = model.Kodi
                    };

                    var result = await bankRepository.AddAsync(addBank);
                    alertService.Success("Banka u shtua me sukses!");

                    return RedirectToAction("Index");
                    
                }
                catch
                {
                    alertService.Danger("Diqka shkoi gabim, provoni perseri!");
                    return View(model);
                }
                
            }
            alertService.Information("Plotesoni te gjitha fushat!");
            return View(model);
        }

        // GET: BankaController/Edit/5
        public async Task<ActionResult> EditAsync(int? id)
        {
            ViewBag.AddError = false;
            if (id == null)
            {
                ViewBag.ErrorTitle = $"Id cannot be null";
                return View("_NotFound");
            }
            var bank = await bankRepository.Get(id);

            if (bank==null)
            {
                ViewBag.ErrorTitle = $"Banka me këtë { id } nuk është gjetur!";
                return View("_NotFound");
            }

            BankaEditViewModel model = new BankaEditViewModel
            {
                Id = bank.Id,
                Emri = bank.Emri,
                Kodi = bank.Kodi
            };

            return View(model);
        }

        // POST: BankaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<ActionResult> EditAsync(BankaEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var editBank = await bankRepository.Get(model.Id);

                    
                        editBank.Emri = model.Emri;
                        editBank.Kodi = model.Kodi;


                        var editedBank = await bankRepository.Update(editBank);
                        alertService.Success("Banka u editua me sukses!");

                    

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    alertService.Danger("Diqka shkoi keq!");
                    return View(model);
                    
                }
            }

            alertService.Information("Mbushi te gjitha fushat!");
            
            return View(model);

        }

        // GET: BankaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BankaController/Delete/5
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

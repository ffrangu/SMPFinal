using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SMP.Data;
using SMP.Helpers;
using SMP.Models.Bank;
using SMP.Models.Departamenti;
using SMP.Models.Grada;
using SMP.Models.Kompania;
using SMP.Models.Paga;
using SMP.Models.Pozita;
using SMP.Models.Punetori;
using SMP.Models.PunetoriKontrata;
using SMP.ViewModels.Pozita;
using SMP.ViewModels.Punetori;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Controllers
{
    public class PunetoriController : BaseController
    {

        protected UserManager<ApplicationUser> userManager;
        protected RoleManager<IdentityRole> roleManager;
        public AlertService alertService { get; }

        private IPozitaRepository pozitaRepository;

        private IDepartamentiRepository departamentiRepository;

        private IKompaniaRepository kompaniaRepository;

        private IBankRepository bankaRepository;

        private IGradaRepository gradaRepository;

        private IPunetoriRepository punetoriRepository;

        private IPunetoriKontrataRepository kontrataRepository;

        private IPagaRepository pagaRepository;
        private readonly ILogger<PunetoriController> logger;
        public IWebHostEnvironment _hostEnvironment { get; }

        public PunetoriController(IPunetoriKontrataRepository _kontrataRepository, IPagaRepository _pagaRepository,IPunetoriRepository _punetoriRepository,IGradaRepository _gradaRepository,IPozitaRepository _pozitaRepository, IBankRepository _bankaRepository, IDepartamentiRepository _departamentiRepository, IKompaniaRepository _kompaniaRepository, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager,
            AlertService _alertService, ILogger<PunetoriController> _logger,IWebHostEnvironment hostEnvironment) 
            : base(_roleManager,_userManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            alertService = _alertService;
            pozitaRepository = _pozitaRepository;
            departamentiRepository = _departamentiRepository;
            kompaniaRepository = _kompaniaRepository;
            bankaRepository = _bankaRepository;
            gradaRepository= _gradaRepository;
            punetoriRepository = _punetoriRepository;
            logger = _logger;
            pagaRepository = _pagaRepository;
            kontrataRepository = _kontrataRepository;
            _hostEnvironment = hostEnvironment;

        }

        // GET: PunetoriController
        public async Task<ActionResult> IndexAsync()
        {
            var punetoret = await punetoriRepository.GetPuntor(user.KompaniaId);

            var listItems = new List<PunetoriListViewModel>();

            foreach (var item in punetoret)
            {
                listItems.Add(new PunetoriListViewModel
                {
                    Id = item.Id,
                    Emri = item.Emri,
                    Mbiemri = item.Mbiemri,
                    NumriPersonal = item.NumriPersonal,
                    Datelindja = item.Datelindja,
                    Adresa = item.Adresa,
                    Komuna= item.Komuna.Emri,
                    Kompania = item.Kompania.Emri,
                    Departamenti = item.Departamenti.Emri,
                    Pozita = item.Pozita.Emri,
                    Banka = item.Banka.Emri,
                    Xhirollogaria = item.Xhirollogaria,
                    Grada = item.Grada.Emri

                });

            }

            return View(listItems);
        }

        public async Task<ActionResult> ContractAsync(int? id)
        {
            var punetoriDetails = await punetoriRepository.GetPuntoriDetails(id);
            var model = new PunetoriListViewModel();
            model.Id = punetoriDetails.Id;
            model.Emri = punetoriDetails.Emri;
            model.Mbiemri = punetoriDetails.Mbiemri;
            model.Telefoni = punetoriDetails.Telefoni;
            model.Email = punetoriDetails.Email;
            model.NumriPersonal = punetoriDetails.NumriPersonal;
            model.Datelindja = punetoriDetails.Datelindja;
            model.Adresa = punetoriDetails.Adresa;
            model.Komuna = punetoriDetails.Komuna.Emri;
            model.Kompania = punetoriDetails.Kompania.Emri;
            model.Departamenti = punetoriDetails.Departamenti.Emri;
            model.Pozita = punetoriDetails.Pozita.Emri;
            model.Banka = punetoriDetails.Banka.Emri;
            model.Xhirollogaria = punetoriDetails.Xhirollogaria;
            model.Grada = punetoriDetails.Grada.Emri;
            return View(model);
        }

    
        public async Task<ActionResult> UploadContractAsync (IFormFile file, int id)
        {
            
            try
            {
                string uniqueFileName = null;
                string filePath = null;
                if (file!=null)
                {
                    if (Path.GetExtension(file.FileName).ToLower() == ".docx" ||
                    Path.GetExtension(file.FileName).ToLower() == ".pdf"
                    )
                    {
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "contracts");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }


                        var addKontrate = new PunetoriKontrata
                        {
                            PunetoriId = id,
                            Emri = file.FileName,
                            Path = filePath,
                            Status = true,
                            Created = DateTime.Now,
                            CreatedBy = user.UserId

                        };
                        var result = await kontrataRepository.AddAsync(addKontrate);
                        alertService.Success("Kontrata u shtua me sukses!");

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        alertService.Danger("Formati i fajllit eshte gabim, provoni perseri!");
                        return View();
                    }
                }
                else
                {
                    alertService.Danger("Ngarkoni kontraten, provoni perseri!");
                    return View();
                }

                
            }
            catch (Exception ex)
            {

                alertService.Danger("Diqka shkoi gabim, provoni perseri!"+ex.InnerException);
                return View();
            }
        }

        

        // GET: PunetoriController/Details/5
        public async Task<ActionResult> DetailsAsync(int? id)
        {
            var punetoriDetails = await punetoriRepository.GetPuntoriDetails(id);

            var model = new PunetoriListViewModel();
            model.Id = punetoriDetails.Id;
            model.Emri = punetoriDetails.Emri;
            model.Mbiemri = punetoriDetails.Mbiemri;
            model.NumriPersonal = punetoriDetails.NumriPersonal;
            model.Datelindja = punetoriDetails.Datelindja;
            model.Adresa = punetoriDetails.Adresa;
            model.Komuna = punetoriDetails.Komuna.Emri;
            model.Kompania = punetoriDetails.Kompania.Emri;
            model.Departamenti = punetoriDetails.Departamenti.Emri;
            model.Pozita = punetoriDetails.Pozita.Emri;
            model.Banka = punetoriDetails.Banka.Emri;
            model.Xhirollogaria = punetoriDetails.Xhirollogaria;
            model.Grada = punetoriDetails.Grada.Emri;

            var pagat = await pagaRepository.GetAll();
            var pagatDetails = pagat.Where(x => x.PunetoriId == id).OrderByDescending(x=>x.Muaji).Take(6);
            var kontratat = await kontrataRepository.GetAll();
            var kontratatDetails = kontratat.Where(x => x.PunetoriId == id).OrderByDescending(x => x.Created).Take(6);

            foreach (var item in pagatDetails)
            {
                model.PagaList.Add(new PagaList
                {
                    Id = item.Id,
                    Viti = item.Viti,
                    Muaji = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Muaji),
                    PagaFinale = item.PagaFinale,
                    Pershkrimi = item.Pershkrimi

                });

            }


            foreach (var item in kontratatDetails)
            {
                model.KontrataList.Add(new KontrataList
                {
                    Id = item.Id,
                    Emri = item.Emri,
                    Status = item.Status,
                    Created = item.Created
                });

            }


            return View(model);
        }

        // GET: PunetoriController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.AddError = false;
            ViewBag.KomunaId = await kompaniaRepository.LoadKomuna(null);
            ViewBag.Departamenti = await departamentiRepository.DepartamentiSelectList(null, false, false);
            ViewBag.Kompania = await kompaniaRepository.KompaniaSelectList(null, false, false);
            ViewBag.Pozita = await pozitaRepository.PozitaSelectList(null, false, false);
            ViewBag.Banka = await bankaRepository.BankaSelectList(null, false, false);
            ViewBag.Grada = await gradaRepository.GradaSelectList(null, false, false);
            return View();
        }

        // POST: PunetoriController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(PunetoriCreateViewModel model)
        {
            //bool showError = false;
            if (ModelState.IsValid)
            {
                
                try
                {
                    var role = await roleManager.FindByNameAsync("User");
                    var addPunetor = new Punetori
                    {
                        UserId = null,
                        Emri = model.Emri,
                        Mbiemri = model.Mbiemri,
                        NumriPersonal = model.NumriPersonal,
                        Datelindja = model.Datelindja,
                        Adresa = model.Adresa,
                        KomunaId = model.KomunaId,
                        KompaniaId = model.KompaniaId,
                        DepartamentiId = model.DepartamentiId,
                        PozitaId = model.PozitaId,
                        BankaId = model.BankaId,
                        Xhirollogaria = model.Xhirollogaria,
                        GradaId = model.GradaId,
                        Created = DateTime.Now,
                        CreatedBy = user.UserId,
                        Email = model.Email,
                        Telefoni = model.Telefoni
                    };

                    var result = await punetoriRepository.AddAsync(addPunetor);

                    var addUser = new ApplicationUser
                    {
                        FirstName = model.Emri,
                        LastName = model.Mbiemri,
                        Email = model.Email,
                        UserName = model.NumriPersonal,
                        Address = model.Adresa,
                        PhoneNumber = model.Telefoni,
                        KompaniaId = model.KompaniaId,
                        EmailConfirmed = true,
                        DepartamentiId = model.DepartamentiId
                    };

                    var resultUser = await userManager.CreateAsync(addUser, "123456789Aa#");

                    if(resultUser.Succeeded)
                    {
                        logger.LogInformation("Administrator created new user.");


                        resultUser = await userManager.AddToRoleAsync(addUser, role.Name);
                    }

                    var updatePunetoriUserID = await punetoriRepository.Get(addPunetor.Id);

                    updatePunetoriUserID.UserId = addUser.Id;

                    var updatedPunetori = await punetoriRepository.Update(updatePunetoriUserID);

                    alertService.Success("Punetori u regjistrua me sukses!");

                    return RedirectToAction("Index");


                }
                catch (Exception ex)
                {
                    var exception = ex;
                    alertService.Danger("Diqka shkoi keq!");
                    ViewBag.KomunaId = await kompaniaRepository.LoadKomuna(null);
                    ViewBag.Departamenti = await departamentiRepository.DepartamentiSelectList(null, false, false);
                    ViewBag.Kompania = await kompaniaRepository.KompaniaSelectList(null, false, false);
                    ViewBag.Pozita = await pozitaRepository.PozitaSelectList(null, false, false);
                    ViewBag.Banka = await bankaRepository.BankaSelectList(null, false, false);
                    ViewBag.Grada = await gradaRepository.GradaSelectList(null, false, false);
                    ViewBag.AddError = true;

                    return View(model);

                }
            }

            alertService.Information("Mbushi te gjitha fushat!");
            ViewBag.KomunaId = await kompaniaRepository.LoadKomuna(null);
            ViewBag.Departamenti = await departamentiRepository.DepartamentiSelectList(null, false, false);
            ViewBag.Kompania = await kompaniaRepository.KompaniaSelectList(null, false, false);
            ViewBag.Pozita = await pozitaRepository.PozitaSelectList(null, false, false);
            ViewBag.Banka = await bankaRepository.BankaSelectList(null, false, false);
            ViewBag.Grada = await gradaRepository.GradaSelectList(null, false, false);
            return View(model);
        }

        // GET: PunetoriController/Edit/5
        public async Task<ActionResult> EditAsync(int? id)
        {
            ViewBag.AddError = false;
            if (id == null)
            {
                ViewBag.ErrorTitle = $"Id cannot be null";
                return View("_NotFound");
            }

            var punetori = await punetoriRepository.Get(id);

            if(punetori == null)
            {
                ViewBag.ErrorTitle = $"Punetori me këtë { id } nuk është gjetur!";
                return View("_NotFound");
            }

            PunetoriEditViewModel model = new PunetoriEditViewModel
            {
                Id = punetori.Id,
                UserId = punetori.UserId,
                Emri = punetori.Emri,
                Mbiemri = punetori.Mbiemri,
                NumriPersonal = punetori.NumriPersonal,
                Datelindja = punetori.Datelindja,
                Email = punetori.Email,
                Adresa = punetori.Adresa,
                KompaniaId = punetori.KompaniaId,
                KomunaId = punetori.KomunaId,
                DepartamentiId = punetori.DepartamentiId,
                PozitaId = punetori.PozitaId,
                BankaId = punetori.BankaId,
                Xhirollogaria = punetori.Xhirollogaria,
                GradaId = punetori.GradaId,
                Telefoni = punetori.Telefoni,
            };
            ViewBag.KomunaId = await kompaniaRepository.LoadKomuna(null);
            ViewBag.Departamenti = await departamentiRepository.DepartamentiSelectList(null, false, false);
            ViewBag.Kompania = await kompaniaRepository.KompaniaSelectList(null, false, false);
            ViewBag.Pozita = await pozitaRepository.PozitaSelectList(null, false, false);
            ViewBag.Banka = await bankaRepository.BankaSelectList(null, false, false);
            ViewBag.Grada = await gradaRepository.GradaSelectList(null, false, false);

            return View(model);
        }

        // POST: PunetoriController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(PunetoriEditViewModel model)
        {
           if(ModelState.IsValid)
            {
                try
                {
                    var editPunetori = await punetoriRepository.Get(model.Id);
                    editPunetori.Emri = model.Emri;
                    editPunetori.Mbiemri = model.Mbiemri;
                    editPunetori.NumriPersonal = model.NumriPersonal;
                    editPunetori.Datelindja = model.Datelindja;
                    editPunetori.Adresa = model.Adresa;
                    editPunetori.KomunaId = model.KomunaId;
                    editPunetori.KompaniaId = model.KompaniaId;
                    editPunetori.DepartamentiId = model.DepartamentiId;
                    editPunetori.PozitaId = model.PozitaId;
                    editPunetori.BankaId = model.BankaId;
                    editPunetori.Xhirollogaria = model.Xhirollogaria;
                    editPunetori.GradaId = model.GradaId;
                    editPunetori.CreatedBy = user.UserId;
                    editPunetori.Email = model.Email;
                    editPunetori.Telefoni = model.Telefoni;
                    var editedPunetor = await punetoriRepository.Update(editPunetori);

                    var editUser = await userManager.FindByIdAsync(editPunetori.UserId);
                    editUser.FirstName = model.Emri;
                    editUser.LastName = model.Mbiemri;
                    editUser.Email = model.Email;
                    editUser.KompaniaId = model.KompaniaId;
                    editUser.DepartamentiId = model.DepartamentiId;
                    editUser.UserName = model.NumriPersonal;
                    editUser.PhoneNumber = model.Telefoni;
                    editUser.Address = model.Adresa;

                    var editedUser = await userManager.UpdateAsync(editUser);

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

        // GET: PunetoriController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PunetoriController/Delete/5
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


        public async Task<JsonResult> Search(string value)
        {
            var searched = await punetoriRepository.Search(value);

            var list = searched.Select(x => new { x.Emri, x.Mbiemri, x.Id });

            
            


            return Json(list);
        }
    }
}

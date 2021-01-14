using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMP.Data;
using SMP.Helpers;
using SMP.Models.Bank;
using SMP.Models.Grada;
using SMP.Models.Kompania;
using SMP.Models.Paga;
using SMP.Models.Punetori;
using SMP.Models.Raport;
using SMP.ViewModels.Raport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FastMember;
using SMP.ViewModels.Paga;
using System.Globalization;
using SMP.ViewModels.Punetori;
using static SMP.Helpers.ReportGenerator;

namespace SMP.Controllers
{
    public class RaportController : BaseController
    {
        private IKompaniaRepository kompaniaRepository;
        private IPunetoriRepository punetoriRepository;
        private IPagaRepository pagaRepository;
        private IGradaRepository gradaRepository;
        private IBankRepository bankRepository;
        private IRaportRepository raportRepository;
        private ISession session;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment webHostEnvironment;

        public RaportController(IKompaniaRepository _kompaniaRepository, IPunetoriRepository _punetoriRepository, IPagaRepository _pagaRepository,
            IGradaRepository _gradaRepository, IBankRepository _bankRepository, IRaportRepository _raportRepository, IHttpContextAccessor _httpContextAccessor, IWebHostEnvironment _webHostEnvironment,
            RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager, AlertService _alertService) 
            : base(_roleManager, _userManager)
        {
            kompaniaRepository = _kompaniaRepository;
            punetoriRepository = _punetoriRepository;
            pagaRepository = _pagaRepository;
            gradaRepository = _gradaRepository;
            bankRepository = _bankRepository;
            raportRepository = _raportRepository;
            httpContextAccessor = _httpContextAccessor;
            session = httpContextAccessor.HttpContext.Session;
            webHostEnvironment = _webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        // GET: RaportController
        public ActionResult Index()
        {
            int[] raportet = new int[Enum.GetNames(typeof(Raportet)).Length];
            if (User.IsInRole("Administrator"))
            {
                raportet = new int[] { 1, 2,3 };
            }
            else if (User.IsInRole("HR"))
            {
                raportet = new int[] { 1, 2, 3 };

            }
            else
            {
                raportet = new int[] { 1, 3 };
                ViewBag.KompaniaId = new SelectList(new List<string>(), "Value", "Text");
            }
            ViewBag.RaportiId = EnumsToSelectList<Raportet>.GetSelectList(raportet);

            return View();
        }

        public async Task<ActionResult> LoadFields(int? raportid)
        {
            string role = User.IsInRole("HR") ? "HR" : "Administrator";

            ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectListBasedOnRole(role, user.KompaniaId);


            ViewBag.PunetoriId = new SelectList(new List<string>(), "Value", "Text");
            ViewBag.GradaId = await gradaRepository.GradaSelectList(null, false, false);
            ViewBag.BankaId = await bankRepository.BankaSelectList(null, false, false);
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

            ViewBag.RaportiId = raportid;
            return View();
        }

        public async Task<IActionResult> GenerateReport(RaportViewModel model)
        {
            RaportModel returmodel = new RaportModel();

            if (model.RaportiId == (int)Raportet.PagatTabelare)
            {
                if(User.IsInRole("User"))
                {
                    var punetori = await punetoriRepository.GetPunetoriByUserId(user.UserId);
                    model.PunetoriId = punetori.Id;
                }

                var pagat = await raportRepository.GetAllPagat(model.PunetoriId, model.KompaniaId, model.Viti, model.Muaji, model.BankaId, model.GradaId);
                returmodel.allPagat = pagat;

                Models.SessionExtensions.Set(session, "PagatTabelare", pagat);
                ViewBag.RaportiId = model.RaportiId;
                return View(returmodel);
            }
            else if(model.RaportiId == (int)Raportet.Punetoret)
            {
                ViewBag.RaportiId = model.RaportiId;
                var punetoret = await raportRepository.GetAllPunetoret(model.PunetoriId, model.KompaniaId, model.BankaId, model.GradaId);
                returmodel.allPunetoret = punetoret;
                Models.SessionExtensions.Set(session, "Punetoret", punetoret);

                return View(returmodel);
            }
            else if(model.RaportiId == (int)Raportet.Payslip)
            {
                ViewBag.RaportiId = model.RaportiId;
                var pagat = await raportRepository.Payslip(model.PunetoriId, model.KompaniaId,model.Viti,model.Muaji, model.BankaId, model.GradaId);
                if(User.IsInRole("User"))
                {
                    var punetori = await punetoriRepository.GetPunetoriByUserId(user.UserId);
                    pagat = pagat.Where(q => q.PunetoriId == punetori.Id).ToList();
                }
                returmodel.paySlip = pagat;
                Models.SessionExtensions.Set(session, "Payslip", pagat);

                return View(returmodel);
            }

            return null;
        }

        public ActionResult Print(int id)
        {
            DataTable dt = new DataTable();
            string reportPath = "";
            ReportType reportType = new ReportType();
            int raportid = id;
            if(raportid == (int)Raportet.PagatTabelare)
            {
                var lista = Models.SessionExtensions.Get<List<AllPagat>>(session, "PagatTabelare");

                using (var reader = ObjectReader.Create(lista, null))
                {
                    dt.Load(reader);
                }

                reportPath = $"{ webHostEnvironment.WebRootPath }\\Reports\\PagatTabelare.rdlc";
                reportType = ReportType.PagaTabelare;
            }
            else if(raportid == (int)Raportet.Punetoret)
            {
                var lista = Models.SessionExtensions.Get<List<PunetoriListViewModel>>(session, "Punetoret");
                using (var reader = ObjectReader.Create(lista, null))
                {
                    dt.Load(reader);
                }

                reportPath = $"{ webHostEnvironment.WebRootPath }\\Reports\\Punetoret.rdlc";
                reportType = ReportType.Punetoret;
            }
            else if (raportid == (int)Raportet.Payslip)
            {
                var lista = Models.SessionExtensions.Get<List<AllPagat>>(session, "Payslip");

                using (var reader = ObjectReader.Create(lista, null))
                {
                    dt.Load(reader);
                }

                reportPath = $"{ webHostEnvironment.WebRootPath }\\Reports\\Payslip.rdlc";
                reportType = ReportType.Payslip;
            }
            else
            {
                return null;
            }

            List<DataTable> dataSets = new List<DataTable>();
            dataSets.Add(dt);

             
            var reportGenerator = new ReportGenerator();

            var bytes = reportGenerator.GenerateReport(reportType, reportPath, dataSets);
            return File(bytes, "application/pdf");
        }

        public JsonResult LoadPunetoret(int? KompaniaId)
        {
            SelectList Punetoret = null;
            try
            {
                if (KompaniaId > 0)
                {
                    Punetoret = punetoriRepository.LoadPunetoret(KompaniaId);
                    ViewBag.PunetoriId = Punetoret;
                }
            }
            catch (Exception ex)
            {

                var exception = ex;
            }
            return Json(Punetoret);
        }

        // GET: RaportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RaportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RaportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: RaportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RaportController/Edit/5
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

        // GET: RaportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RaportController/Delete/5
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

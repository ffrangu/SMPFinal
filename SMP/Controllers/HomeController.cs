using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SMP.Data;
using SMP.Models;
using SMP.Models.Home;

namespace SMP.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHomeRepository homeRepository;
        public HomeController(IHomeRepository _homeRepository, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager) : base(_roleManager, _userManager)
        {
            homeRepository = _homeRepository;
        }

        public IActionResult Index()
        {
            ViewBag.KompaniaId = user.KompaniaId;
            ViewBag.UserId = user.UserId;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

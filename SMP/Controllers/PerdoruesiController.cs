using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SMP.Data;
using SMP.Helpers;
using SMP.Models.Kompania;
using SMP.ViewModels;

namespace SMP.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PerdoruesiController : BaseController
    {
        // GET: PerdoruesiController

        protected UserManager<ApplicationUser> userManager;
        protected RoleManager<IdentityRole> roleManager;
        public AlertService alertService { get; }
        private readonly IEmailSender emailSender;
        private readonly ILogger<PerdoruesiController> logger;
        private IKompaniaRepository kompaniaRepository;

        public PerdoruesiController(RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager,
            ILogger<PerdoruesiController> _logger, AlertService _alertService, IEmailSender _emailSender, IKompaniaRepository _kompaniaRepository) 
            : base(_roleManager, _userManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            emailSender = _emailSender;
            alertService = _alertService;
            logger = _logger;
            kompaniaRepository = _kompaniaRepository;
        }

        public async Task<ActionResult> IndexAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var roles = await roleManager.Roles.ToListAsync();

            ViewBag.Roles = roles;

            return View(users);
        }

        // GET: PerdoruesiController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PerdoruesiController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.RoleId = await LoadRoles(null, "User");
            ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectList(null, false, false);
            ViewBag.AddError = false;

            return View();
        }

        // POST: PerdoruesiController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(PerdoruesiCreateViewModel model)
        {
            bool adderrors = true;

            if(ModelState.IsValid)
            {
                try
                {
                    var role = await roleManager.FindByIdAsync(model.RoleId);

                    if(role.Name == "HR")
                    {
                        if(!model.KompaniaId.HasValue)
                        {
                            alertService.Information("Plotesoni te dhenat obligative");

                            ViewBag.RoleId = await LoadRoles(null, "User");
                            ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectList(null, false, false);

                            return View(model);
                        }
                    }

                    var addUser = new ApplicationUser
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.Email,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        KompaniaId = model.KompaniaId
                    };

                    var result = await userManager.CreateAsync(addUser, model.Password);

                    if(result.Succeeded)
                    {
                        adderrors = false;
                        logger.LogInformation("Administrator created new user.");


                        result = await userManager.AddToRoleAsync(addUser, role.Name);

                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, "Cannot add user to selected role");
                            await userManager.DeleteAsync(addUser);
                            ViewBag.RoleId = await LoadRoles(model.RoleId,  "User");
                            ViewBag.AddError = adderrors;
                            return View(model);
                        }

                        var code = await userManager.GenerateEmailConfirmationTokenAsync(addUser);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = addUser.Id, code = code },
                        protocol: Request.Scheme);

                        await emailSender.SendEmailAsync(model.Email,
                        "Konfirmo email adresen",
                         $"Ju lutem konfirmoni llogarinë tuaj duke <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikuar këtu</a>.");

                        alertService.Information("Para se përdoruesi i krijuar të qaset në sistem, përdoruesi duhet të konfirmoj llogarinë e tij.", true);

                        return RedirectToAction("Index");
                    }

                    AddErrors(result);
                }
                catch
                {
                    ViewBag.RoleId = await LoadRoles(model.RoleId, "User");
                    ViewBag.AddError = adderrors;

                    return View(model);
                }
            }

            ViewBag.AddError = adderrors;

            ViewBag.RoleId = await LoadRoles(model.RoleId, "User");

            return View(model);
        }

        // GET: PerdoruesiController/Edit/5
        public async Task<ActionResult> EditAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorTitle = $"Id nuk mund të jetë null";
                return View("_NotFound");
            }

            var userFound = await userManager.FindByIdAsync(id);

            if (userFound == null)
            {
                ViewBag.ErrorTitle = $"Përdoruesi me këtë id { id } nuk mund të gjendet.";
                return View("_NotFound");
            }

            var userRoles = await userManager.GetRolesAsync(userFound);
            var role = await roleManager.FindByNameAsync(userRoles.FirstOrDefault());

            PerdoruesiEditViewModel model = new PerdoruesiEditViewModel
            {
                Id = userFound.Id,
                FirstName = userFound.FirstName,
                LastName = userFound.LastName,
                Email = userFound.Email,
                RoleId = role.Id,
                Address = userFound.Address,
                PhoneNumber = userFound.PhoneNumber,
                UserProfile = userFound.Image,
                KompaniaId = userFound.KompaniaId,
                RoleName = role.Name
            };


            ViewBag.RoleId = await LoadRoles(userRoles.FirstOrDefault(), "User");
            ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectList(null, false, false);

            model.resetPassword.UserId = userFound.Id;

            //random hashed password
            var random = userManager.PasswordHasher.HashPassword(userFound, RandomNumberGenerator.Create().ToString());
            model.resetPassword.Password = random;
            model.resetPassword.ConfirmPassword = random;
            return View(model);
        }

        // POST: PerdoruesiController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(PerdoruesiEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var userFound = await userManager.FindByIdAsync(model.Id);

                    if (userFound == null)
                    {
                        ViewBag.ErrorTitle = $"Përdoruesi me këtë id { model.Id } nuk mund të gjendet.";
                        return View("_NotFound");
                    }

                    var roleFound = await roleManager.FindByIdAsync(model.RoleId);

                    if (roleFound.Name == "HR")
                    {
                        if (!model.KompaniaId.HasValue)
                        {
                            alertService.Information("Plotesoni te dhenat obligative");

                            ViewBag.RoleId = await LoadRoles(null, "User");
                            ViewBag.KompaniaId = await kompaniaRepository.KompaniaSelectList(null, false, false);

                            return View(model);
                        }
                    }
                    else
                        model.KompaniaId = null;

                    userFound.FirstName = model.FirstName;
                    userFound.LastName = model.LastName;
                    userFound.Email = model.Email;
                    userFound.Address = model.Address;
                    userFound.PhoneNumber = model.PhoneNumber;
                    userFound.KompaniaId = model.KompaniaId;

                    var userRoles = await userManager.GetRolesAsync(userFound);
                    var role = await roleManager.FindByNameAsync(userRoles.FirstOrDefault());

                    if (role.Id != model.RoleId)
                    {
                        await userManager.RemoveFromRolesAsync(userFound, userRoles);

                        role = await roleManager.FindByIdAsync(model.RoleId);

                        await userManager.AddToRoleAsync(userFound, role.Name);
                    }

                    var result = await userManager.UpdateAsync(userFound);

                    if (result.Succeeded)
                    {
                        alertService.Success("Të dhënat e përdoruesit u modifikuan me sukses", true);

                        return RedirectToAction("Edit", new { id = userFound.Id });
                    }
                }
                catch
                {
                    ViewBag.RoleId = await LoadRoles(model.RoleId, "User");

                    return View();
                }
            }


            IEnumerable<string> allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

            foreach (var item in allErrors)
            {
                alertService.Information(item, true);
            }

            ViewBag.RoleId = await LoadRoles(model.RoleId, "User");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordAsync(PerdoruesiEditViewModel model)
        {
            string userId = null;

            var userFound = await userManager.FindByIdAsync(model.resetPassword.UserId);

            if (userFound != null)
            {
                userId = userFound.Id;
                var code = await userManager.GeneratePasswordResetTokenAsync(userFound);

                var result = await userManager.ResetPasswordAsync(userFound, code, model.resetPassword.Password);

                if (result.Succeeded)
                {
                    alertService.Success("Fjalëkalimi u resetua me sukses", true);

                    return RedirectToAction("Edit", new { id = userFound.Id });
                }
                else
                {
                    alertService.Information("Fjalëkalimi nuk është ndryshuar", true);

                    return RedirectToAction("Edit", new { id = userFound.Id });
                }
            }

            if (!string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Edit", new { id = userId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: PerdoruesiController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PerdoruesiController/Delete/5
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

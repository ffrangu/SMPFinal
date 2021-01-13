using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SMP.Data;

namespace SMP.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : BaseModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger) : base(userManager, signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Plotësoni fushën")]
            [Display(Name = "Fjalëkalimi i tanishëm")]
            public string CurrentPassword { get; set; }

            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Plotësoni fushën")]
            [Display(Name = "Fjalëkalimi i ri")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Konfirmo fjalëkalimin")]
            [Compare("NewPassword", ErrorMessage = "Fjalëkalimet nuk përputhen")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Perdoruesi me kete ID '{_userManager.GetUserId(User)}' nuk mund te gjendet.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            AddUserToSession();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Perdoruesi me kete ID '{_userManager.GetUserId(User)}' nuk mund te gjendet.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                AddUserToSession();
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Fjalëkalimi është ndryshuar me sukses";

            return RedirectToPage();
        }
    }
}

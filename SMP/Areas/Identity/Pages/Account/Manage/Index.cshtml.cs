using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SMP.Data;
using SMP.Helpers;

namespace SMP.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : BaseModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AlertService alertService { get; }
        public IWebHostEnvironment _hostEnvironment { get; }
        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, AlertService _alertService, IWebHostEnvironment hostEnvironment) : base(userManager, signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            alertService = _alertService;
            _hostEnvironment = hostEnvironment;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Plotësoni fushën")]
            [Display(Name = "Emri")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Plotësoni fushën")]
            [Display(Name = "Mbiemri")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Plotësoni fushën")]
            [Display(Name = "Adresa")]
            public string Address { get; set; }

            [RegularExpression(@"^([+]3[83]{1}[0-9]{9})$", ErrorMessage = "Format jo valid")]
            [Display(Name = "Telefoni")]
            public string PhoneNumber { get; set; }

            public IFormFile Picture { get; set; }

            public string ExistingPhotoPath { get; set; }

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                ExistingPhotoPath = user.Image
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Perdoruesi me kete ID '{_userManager.GetUserId(User)}' nuk mund te gjendet.");
            }

            await LoadAsync(user);

            AddUserToSession();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Perdoruesi me kete ID '{_userManager.GetUserId(User)}' nuk mund te gjendet.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.Address = Input.Address;

            if (Input.Picture != null)
            {
                if (Path.GetExtension(Input.Picture.FileName).ToLower() == ".jpg" ||
                    Path.GetExtension(Input.Picture.FileName).ToLower() == ".png" ||
                    Path.GetExtension(Input.Picture.FileName).ToLower() == ".jpeg" ||
                    Path.GetExtension(Input.Picture.FileName).ToLower() == ".gif")
                {
                    if (Input.ExistingPhotoPath != null)
                    {
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");

                        var inputtest = Input.ExistingPhotoPath.Substring(8);

                        string filePath = Path.Combine(uploadsFolder, inputtest);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    user.Image = ProcessUploadedFile(Input);
                }
            }

            await _userManager.UpdateAsync(user);

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Të dhënat e profilin u ndryshuan me sukses";
            return RedirectToPage();
        }

        private string ProcessUploadedFile(InputModel model)
        {
            string uniqueFileName = null;
            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }

            return "/images/" + uniqueFileName;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SMP.Data;
using SMP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Areas.Identity.Pages.Account.Manage
{
    public class BaseModel : PageModel
    {
        protected UserModel user;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public BaseModel(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        protected void AddUserToSession()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _userManager.Users.FirstOrDefault(t => t.UserName == User.Identity.Name);
                var currentRole = _userManager.GetRolesAsync(currentUser).Result[0];

                user = new UserModel
                {
                    FullName = $"{currentUser.FirstName} {currentUser.LastName}",
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Email = currentUser.Email,
                    PhoneNumber = currentUser.PhoneNumber,
                    UserId = currentUser.Id,
                    UserName = currentUser.UserName,
                    RoleDescription = currentRole,
                    Picture = currentUser.Image
                };
            }
            else
            {
                user = new UserModel
                {
                    Role = "0",
                    RoleDescription = "Public",
                };
            }

            ViewData["Menus"] = GetMenus(user.RoleDescription);
            ViewData["User"] = user;
        }

        private List<MenuItems> GetMenus(string role)
        {
            List<MenuItems> menus = new List<MenuItems>();
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                menus = new List<MenuItems>
                {
                   #region Roli Administrator

                   role.In("Administrator") ? new MenuItems{ Text ="Dashboard", Controller="Home", Action="Index", Selected = false, Icon = "menu-icon flaticon-home"  } : null,
                   role.In("Administrator") ? new MenuItems{ Text = "Përdoruesi", Controller="", Action="", Selected = false, Icon = "menu-icon flaticon-user",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Përdorues i ri", Controller="Perdoruesi", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       new MenuItems { Text= "Lista e përdoruesve", Controller="Perdoruesi", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                   }} : null,
                   role.In("Administrator") ? new MenuItems{ Text = "Administrimi i formave", Controller="", Action="", Selected = false, Icon = "menu-icon fa fa-chart-area",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Kompania", Controller="", Action="", Icon = "menu-bullet menu-bullet-dot", Selected = false,
                       SubMenu = new List<MenuItems>
                       {
                           new MenuItems { Text= "Regjistro kompani", Controller="Kompania", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                           new MenuItems { Text= "Lista e kompanive", Controller="Kompania", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       }},
                       new MenuItems { Text= "Departamenti", Controller="", Action="", Icon = "menu-bullet menu-bullet-dot", Selected = false,
                       SubMenu = new List<MenuItems>
                       {
                           new MenuItems { Text= "Regjistro departament", Controller="Departamenti", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                           new MenuItems { Text= "Lista e departamenteve", Controller="Departamenti", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       }},
                       new MenuItems { Text= "Pozita", Controller="", Action="", Icon = "menu-bullet menu-bullet-dot", Selected = false,
                       SubMenu = new List<MenuItems>
                       {
                           new MenuItems { Text= "Regjistro pozitë", Controller="Pozita", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                           new MenuItems { Text= "Lista e pozitave", Controller="Pozita", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       }},
                       new MenuItems { Text= "Grada", Controller="", Action="", Icon = "menu-bullet menu-bullet-dot", Selected = false,
                       SubMenu = new List<MenuItems>
                       {
                           new MenuItems { Text= "Regjistro gradë", Controller="Grada", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                           new MenuItems { Text= "Lista e gradave", Controller="Grada", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       }},
                       new MenuItems { Text= "Banka", Controller="", Action="", Icon = "menu-bullet menu-bullet-dot", Selected = false,
                       SubMenu = new List<MenuItems>
                       {
                           new MenuItems { Text= "Regjistro bank", Controller="Banka", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                           new MenuItems { Text= "Lista e bankave", Controller="Banka", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       }},

                   }} : null,

                   role.In("Administrator") ? new MenuItems{ Text = "Bonuset", Controller="", Action="", Selected = false, Icon = "menu-icon fa fa-tree",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Bonuset", Controller="Bonuset", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       new MenuItems { Text= "Regjistro bonus", Controller="Bonuset", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                   }} : null,

                   role.In("Administrator") ? new MenuItems{ Text = "Punëtori", Controller="", Action="", Selected = false, Icon = "menu-icon fa fa-tree",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Lista e punëtorëve", Controller="Punetori", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       new MenuItems { Text= "Regjistro punëtor", Controller="Punetori", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                   }} : null,

                   role.In("Administrator") ? new MenuItems{ Text = "Pagat", Controller="", Action="", Selected = false, Icon = "menu-icon flaticon2-check-mark",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Lista e pagave", Controller="Paga", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       new MenuItems { Text= "Gjenero pagat", Controller="Paga", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                   }} : null,

                   role.In("Administrator") ? new MenuItems{ Text = "Raportet", Controller="", Action="", Selected = false, Icon = "menu-icon flaticon2-contract",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Shfleto raportet", Controller="Raport", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                   }} : null,

                    #endregion

                   #region Roli HR
                   role.In("HR") ? new MenuItems{ Text ="Dashboard", Controller="Home", Action="Index", Selected = false, Icon = "menu-icon flaticon-home"  } : null,
                   role.In("HR") ? new MenuItems{ Text = "Bonuset", Controller="", Action="", Selected = false, Icon = "menu-icon fa fa-tree",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Bonuset", Controller="Bonus", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       new MenuItems { Text= "Regjistro bonus", Controller="Bonus", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                   }} : null,

                   role.In("HR") ? new MenuItems{ Text = "Punëtori", Controller="", Action="", Selected = false, Icon = "menu-icon fa fa-tree",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Lista e punëtorëve", Controller="Punetori", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       new MenuItems { Text= "Regjistro punëtor", Controller="Punetori", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                   }} : null,

                   role.In("HR") ? new MenuItems{ Text = "Pagat", Controller="", Action="", Selected = false, Icon = "menu-icon flaticon2-check-mark",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Lista e pagave", Controller="Paga", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                       new MenuItems { Text= "Gjenero pagat", Controller="Paga", Action="Create", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                   }} : null,
                   role.In("HR") ? new MenuItems{ Text = "Raportet", Controller="", Action="", Selected = false, Icon = "menu-icon flaticon2-contract",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Shfleto raportet", Controller="Raport", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                   }} : null,
                    #endregion

                    #region Roli User
                   role.In("User") ? new MenuItems{ Text ="Dashboard", Controller="Home", Action="Index", Selected = false, Icon = "menu-icon flaticon-home"  } : null,
                   role.In("User") ? new MenuItems{ Text = "Raportet", Controller="", Action="", Selected = false, Icon = "menu-icon flaticon2-contract",
                   SubMenu = new List<MenuItems>
                   {
                       new MenuItems { Text= "Shfleto raportet", Controller="Raport", Action="Index", Icon = "menu-bullet menu-bullet-dot", Selected = false },
                   }} : null,
                    #endregion

                };
            }
            else
            {
                menus = new List<MenuItems>
                {
                 role.In("Public") ? new MenuItems { Text = "Dashboard", Controller = "Home", Action = "Index", Selected = false, Icon = "menu-icon flaticon-home" } : null
                };
            }

            menus.RemoveAll(item => item == null);

            //string action = ControllerContext.RouteData.Values["action"].ToString();
            //string controller = ControllerContext.RouteData.Values["controller"].ToString();

            //foreach (var item in menus)
            //{
            //    if (item.Controller == controller || item.SubMenu.Select(x => x.Controller).Contains(controller))
            //    {
            //        item.Selected = true;
            //    }

            //    foreach (var subItem in item.SubMenu)
            //    {
            //        if (subItem.Controller == controller && (subItem.Action == action || subItem.IncludedActions.Contains(action)))
            //        {
            //            item.Selected = true;
            //            subItem.Selected = true;
            //        }
            //    }
            //}


            return menus;
        }


    }
}

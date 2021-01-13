using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMP
{
    public class MenuItems
    {
        public MenuItems()
        {
            SubMenu = new List<MenuItems>();
            IncludedActions = new List<string>();
        }

        public string Text { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public List<string> IncludedActions { get; set; }
        public string Icon { get; set; }
        public bool Selected { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }

        public List<MenuItems> SubMenu { get; set; }
    }
}
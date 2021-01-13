using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.ViewModels.Banka
{

    public class BankaListViewModel
    {
        public int Id { get; set; }

        public string Kodi { get; set; }

        public string Emri { get; set; }

    }


    public class BankaCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Plotësoni fushën")]
        public string Emri { get; set; }

        [Required(ErrorMessage = "Plotësoni fushën")]
        public string Kodi { get; set; }

    }


    public class BankaEditViewModel : BankaCreateViewModel
    {
        
    }
}

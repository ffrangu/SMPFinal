using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.ViewModels.Grada
{
    public class GradaListViewModel
    {
        public int Id { get; set; }

        public string Emri { get; set; }

        public decimal PagaMujore { get; set; }

        public decimal PagaVjetore { get; set; }


    }


    public class GradaCreateViewModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Plotësoni fushën")]
        public string Emri { get; set; }

        [Required(ErrorMessage = "Plotësoni fushën")]
        public decimal PagaMujore { get; set; }

        [Required(ErrorMessage = "Plotësoni fushën")]
        public decimal PagaVjetore { get; set; }

    }

    public class GradaEditViewModel : GradaCreateViewModel
    {

    }
}

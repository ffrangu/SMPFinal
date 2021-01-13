using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.ViewModels.Kompania
{
    public class KompaniaListViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Emri { get; set; }

        public string Komuna { get; set; }

        public string Kodi { get; set; }

        public int Niveli { get; set; }
    }

    public class KompaniaCreateViewModel
    {
        public int? ParentId { get; set; }

        [Required(ErrorMessage = "Plotësoni fushën")]
        public int KomunaId { get; set; }

        [StringLength(10, MinimumLength = 2, ErrorMessage = "Mund te shenoni 2 deri ne 10 shkronja")]
        [Required(ErrorMessage = "Plotësoni fushën")]
        public string Kodi { get; set; }

        [Required(ErrorMessage = "Plotësoni fushën")]
        public string Emri { get; set; }
    }

    public class KompaniaEditViewModel : KompaniaCreateViewModel
    {
        public int Id { get; set; }
    }
}

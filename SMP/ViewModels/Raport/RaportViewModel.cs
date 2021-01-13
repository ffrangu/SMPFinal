using SMP.ViewModels.Paga;
using SMP.ViewModels.Punetori;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.ViewModels.Raport
{
    public class RaportViewModel
    {
        [Required]
        public int RaportiId { get; set; }
        public int? KompaniaId { get; set; }
        public int? PunetoriId { get; set; }

        public int? GradaId { get; set; }

        public int? BankaId { get; set; }

        public int? Viti { get; set; }

        public int? Muaji { get; set; }
    }

    public class RaportModel
    {
        public RaportModel()
        {
            allPagat = new List<AllPagat>();
            paySlip = new List<AllPagat>();
            allPunetoret = new List<PunetoriListViewModel>();
        }

        public List<AllPagat> allPagat { get; set; }
        public List<PunetoriListViewModel> allPunetoret { get; set; }

        public List<AllPagat> paySlip { get; set; }
    }
}

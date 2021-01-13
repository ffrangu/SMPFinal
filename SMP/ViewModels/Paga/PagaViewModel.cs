using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.ViewModels.Paga
{
    public class PagaViewModel
    {
        public int Viti { get; set; }

        public string Muaji { get; set; }

        public int Month { get; set; }

        public string Data { get; set; }

        public string Kompania { get; set; }

        public string Pershkrimi { get; set; }

        public int KompaniaId { get; set; }
    }

    public class AllPagat
    {
        public int Id { get; set; }

        public string Punetori { get; set; }

        public string Kompania { get; set; }

        public string Pozita { get; set; }

        public string Grada { get; set; }

        public int Viti { get; set; }

        public string Muaji { get; set; }

        public decimal Bruto { get; set; }

        public decimal Kontributi { get; set; }

        public decimal PagaPaTatimuar { get; set; }

        public decimal Tatimi { get; set; }

        public decimal PagaNeto { get; set; }

        public decimal? Bonuse { get; set; }

        public decimal PagaFinale { get; set; }

        public int KompaniaId { get; set; }

        public int PunetoriId { get; set; }

        public int GradaId { get; set; }

        public int BankaId { get; set; }

        public decimal? BonuseNeto { get; set; }

        public int MuajiInt { get; set; }

        public string Banka { get; set; }

        public string NumriPersonal { get; set; }

    }

    public class PagaCreateViewModel
    {
        [Required(ErrorMessage = "Plotësoni fushën")]
        public int KompaniaId { get; set; }
        [Required(ErrorMessage = "Plotësoni fushën")]
        public string Pershkrimi { get; set; }
        [Required(ErrorMessage = "Plotësoni fushën")]
        public int Muaji { get; set; }
        [Required(ErrorMessage = "Plotësoni fushën")]
        public int Viti { get; set; }
    }
}

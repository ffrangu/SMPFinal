using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMP.Data
{
    public partial class Punetori
    {
        public Punetori()
        {
            Bonuset = new HashSet<Bonuset>();
            Paga = new HashSet<Paga>();
            PunetoriKontrata = new HashSet<PunetoriKontrata>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Emri { get; set; }
        public string Mbiemri { get; set; }
        public string NumriPersonal { get; set; }
        public DateTime Datelindja { get; set; }
        public string Adresa { get; set; }
        public int KomunaId { get; set; }
        public int KompaniaId { get; set; }
        public int DepartamentiId { get; set; }
        public int PozitaId { get; set; }
        public int BankaId { get; set; }
        public string Xhirollogaria { get; set; }
        public int GradaId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Telefoni { get; set; }

        public virtual Banka Banka { get; set; }
        public virtual Departamenti Departamenti { get; set; }
        public virtual Grada Grada { get; set; }
        public virtual Kompania Kompania { get; set; }
        public virtual Komuna Komuna { get; set; }
        public virtual Pozita Pozita { get; set; }
        public virtual ICollection<Bonuset> Bonuset { get; set; }
        public virtual ICollection<Paga> Paga { get; set; }
        public virtual ICollection<PunetoriKontrata> PunetoriKontrata { get; set; }
    }
}

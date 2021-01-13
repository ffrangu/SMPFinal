using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public override string UserName { get; set; }

        [Required]
        public override string Email { get; set; }

        [StringLength(64, MinimumLength = 3)]
        public string FirstName { get; set; }

        [StringLength(64, MinimumLength = 3)]
        public string LastName { get; set; }

        [MaxLength]
        public string Image { get; set; }


        [StringLength(200, MinimumLength = 3)]
        public string Address { get; set; }

        public string GetFullName { get { return FirstName + " " + LastName; } }

        public int? KompaniaId { get; set; }

        public int? DepartamentiId { get; set; }

        public virtual Kompania Kompania { get; set; }
        public virtual Departamenti Departamenti { get; set; }
    }
}

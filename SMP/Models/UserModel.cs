using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP
{
    public class UserModel
    {
        public string Role { get; set; }
        public string RoleDescription { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public int? KompaniaId { get; set; }
        public string Picture { get; set; }
        //public LanguageEnum Language { get; set; }
    }
}

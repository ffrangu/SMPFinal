using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.ViewModels.Departamenti
{

    public class DepartamentiListViewModel
    {
        public int Id { get; set; }

        public int KompaniaId { get; set; }

        public string Kompania { get; set; }

        public string Emri { get; set; }

        public string Shkurtesa { get; set; }

        public bool Status { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }
    }
    public class DepartamentiCreateViewModel
    {
        public int Id { get; set; }

        public int KompaniaId { get; set; }

        public string Emri { get; set; }

        public string Shkurtesa { get; set; }

        public bool Status { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }
    }

    public class DepartamentiEditViewModel : DepartamentiCreateViewModel
    {

    }
}

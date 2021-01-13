using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Departamenti
{
    public interface IDepartamentiRepository :IGenericRepository<Data.Departamenti>
    {
        Task<List<Data.Departamenti>> GetDepartments();

        Task<SelectList> DepartamentiSelectList(int? DepartamentiId, bool isList, bool isEdit);
    }
}

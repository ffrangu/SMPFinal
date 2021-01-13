using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMP.Data;
using SMP.ViewModels.Departamenti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Departamenti
{
    public class DepartamentiRepository : GenericRepository<Data.Departamenti>, IDepartamentiRepository
    {

        public DepartamentiRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Data.Departamenti>> GetDepartments()
        {
            var departments = await context.Departamenti.Include(q => q.Kompania).ToListAsync();

            return departments;
        }

        public async Task<SelectList> DepartamentiSelectList(int? DepartamentiId, bool isList, bool isEdit)
        {
            var departments = await GetDepartments();

            return new SelectList(departments, "Id", "Emri");
        }

    }
}

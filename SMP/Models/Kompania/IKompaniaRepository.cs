using Microsoft.AspNetCore.Mvc.Rendering;
using SMP.ViewModels.Kompania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Kompania
{
    public interface IKompaniaRepository : IGenericRepository<Data.Kompania>
    {
        Task<List<Data.Kompania>> GetCompanies();

        Task<SelectList> KompaniaSelectList(int? KompaniaId, bool isList, bool isEdit);
        Task<SelectList> KompaniaSelectListBasedOnRole(string Role, int? KompaniaId);

        void KompaniaSubTree(IEnumerable<Data.Kompania> companies, Data.Kompania company, IList<Data.Kompania> items, bool isList, IList<KompaniaListViewModel> listItems);

        Task<List<KompaniaListViewModel>> KompaniaListModel();

        Task<Data.Kompania> KompaniaAddModel(KompaniaCreateViewModel model);

        KompaniaEditViewModel KompaniaEditModel(Data.Kompania model);

        Task<Data.Kompania> KompaniaOnPostEditModel(KompaniaEditViewModel model);


        Task<SelectList> LoadKomuna(int? selected);
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMP.Data;
using SMP.ViewModels.Kompania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models.Kompania
{
    public class KompaniaRepository : GenericRepository<Data.Kompania>, IKompaniaRepository
    {
        public KompaniaRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Data.Kompania>> GetCompanies()
        {
            var companies = await context.Kompania.Include(q => q.Komuna).ToListAsync();

            return companies;
        }

        public void KompaniaSubTree(IEnumerable<Data.Kompania> companies, Data.Kompania company, IList<Data.Kompania> items, bool isList, IList<KompaniaListViewModel> listItems)
        {
            var subCompanies = companies.Where(q => q.ParentId == company.Id);

            foreach (var item in subCompanies)
            {
                string space = null;

                for (int i = 0; i < item.Niveli; i++)
                {
                    space += "\xA0\xA0";
                }

                if (isList)
                {
                    listItems.Add(new KompaniaListViewModel { 
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Emri = item.Emri,
                        Komuna = item.Komuna.Emri,
                        Kodi = item.Kodi,
                        Niveli = item.Niveli
                    
                    });
                }
                else
                {
                    items.Add(new Data.Kompania { Id = item.Id, Emri = space + item.Emri });
                }

                space = null;

                KompaniaSubTree(companies, item, items, isList, listItems);
            }
        }

        public async Task<SelectList> KompaniaSelectList(int? KompaniaId, bool isList, bool isEdit)
        {
            var kompanite = await GetCompanies();

            var returnItems = new List<Data.Kompania>();
            var listItems = new List<KompaniaListViewModel>();

            foreach (var item in kompanite.Where(q=>!q.ParentId.HasValue))
            {
                returnItems.Add(new Data.Kompania { Id = item.Id, Emri = item.Emri.ToUpper() });

                KompaniaSubTree(kompanite, item, returnItems, isList, listItems);
            }

            if(KompaniaId.HasValue)
            {
                var excludeKompania = await Get(KompaniaId);

                if(isEdit)
                {
                    returnItems = returnItems.Where(q => q.Id == KompaniaId).ToList();
                }
                else
                {
                    returnItems = returnItems.Where(q => q.Id != KompaniaId).ToList();
                }
            }

            return new SelectList(returnItems, "Id", "Emri");

        }

        public async Task<SelectList> KompaniaSelectListBasedOnRole(string Role, int? KompaniaId)
        {
            var kompanite = await GetCompanies();
            var filteredCompanies = new List<Data.Kompania>();

            var returnItems = new List<Data.Kompania>();
            var listItems = new List<KompaniaListViewModel>();

            if (Role == "HR")
            {
                filteredCompanies = kompanite.Where(q => q.Id == KompaniaId.Value).ToList();
                foreach (var item in filteredCompanies)
                {
                    returnItems.Add(new Data.Kompania { Id = item.Id, Emri = item.Emri.ToUpper() });

                    //KompaniaSubTree(filteredCompanies, item, returnItems, false, listItems);
                }
            }
            else
            {
                filteredCompanies = kompanite.Where(q=>!q.ParentId.HasValue).ToList();

                foreach (var item in filteredCompanies)
                {
                    returnItems.Add(new Data.Kompania { Id = item.Id, Emri = item.Emri.ToUpper() });

                    //KompaniaSubTree(kompanite, item, returnItems, false, listItems);
                }
            }

            return new SelectList(returnItems, "Id", "Emri");

        }

        public async Task<List<KompaniaListViewModel>> KompaniaListModel()
        {
            var companies = await GetCompanies();

            var returnItems = new List<Data.Kompania>();
            var listItems = new List<KompaniaListViewModel>();

            foreach (var company in companies.Where(q=>!q.ParentId.HasValue))
            {
                listItems.Add(new KompaniaListViewModel { 
                    Id = company.Id,
                    Emri = company.Emri,
                    Kodi = company.Kodi,
                    Komuna = company.Komuna.Emri,
                    Niveli = company.Niveli,
                    ParentId = company.ParentId
                });

                KompaniaSubTree(companies, company, returnItems, true, listItems);
            }

            return listItems;
        }

        public async Task<Data.Kompania> KompaniaAddModel(KompaniaCreateViewModel model)
        {
            var kompania = model.ParentId.HasValue ? await Get(model.ParentId.Value) : new Data.Kompania();

            int niveli = kompania != null ? kompania.Niveli + 1 : 1;

            var add = new Data.Kompania
            {
                Emri = model.Emri,
                Kodi = model.Kodi,
                KomunaId = model.KomunaId,
                ParentId = model.ParentId,
                Niveli = niveli
            };

            return add;
        }

        public KompaniaEditViewModel KompaniaEditModel(Data.Kompania model)
        {
            KompaniaEditViewModel editModel = new KompaniaEditViewModel
            {
                Id = model.Id,
                Emri = model.Emri,
                Kodi = model.Kodi,
                KomunaId = model.KomunaId,
                ParentId = model.ParentId
            };

            return editModel;
        }

        public async Task<Data.Kompania> KompaniaOnPostEditModel(KompaniaEditViewModel model)
        {
            var kompania = model.ParentId.HasValue ? await Get(model.ParentId.Value) : new Data.Kompania();
            int niveli = kompania != null ? kompania.Niveli + 1 : 1;

            var editKompania = await Get(model.Id);

            editKompania.Emri = model.Emri;
            editKompania.Kodi = model.Kodi;
            editKompania.KomunaId = model.KomunaId;
            editKompania.ParentId = model.ParentId;
            editKompania.Niveli = niveli;

            return editKompania;
        }

        public async Task<SelectList> LoadKomuna(int? selected)
        {
            var komunat = await context.Komuna.ToListAsync();

            if (selected.HasValue)
            {
                return new SelectList(komunat, "Id", "Emri", selected);
            }
            else
            {
                return new SelectList(komunat, "Id", "Emri");
            }
        }
    }
}

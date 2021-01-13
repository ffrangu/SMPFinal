using Microsoft.EntityFrameworkCore;
using SMP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models
{
    public class GenericRepository<T>: IGenericRepository<T> where T: class
    {

        protected readonly ApplicationDbContext context;
        private DbSet<T> table = null;

        public GenericRepository(ApplicationDbContext _context)
        {
            context = _context;
            table = context.Set<T>();
        }

        public async Task<T> AddAsync (T obj)
        {
            table.Add(obj);
            await context.SaveChangesAsync();
            return obj;
        }

        public async Task<T> Delete (object Id)
        {
            T obj = table.Find(Id);
            if(obj !=null)
            {
                table.Remove(obj);
                await context.SaveChangesAsync();
            }

            return obj;

        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await table.ToListAsync();
        }

        public async Task<T> Get(object Id)
        {
            return await table.FindAsync(Id);
        }

        public async Task<T> Update(T obj)
        {
            //var record = table.Attach(obj);
            context.Entry(obj).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return obj;
        }

    }
}
